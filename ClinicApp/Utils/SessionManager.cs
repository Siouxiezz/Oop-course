using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        if (string.IsNullOrWhiteSpace(sessionPath))
        {
            throw new ArgumentException("Session path cannot be empty.", nameof(sessionPath));
        }

        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        if (clinic is null)
        {
            throw new ArgumentNullException(nameof(clinic));
        }

        string? directory = Path.GetDirectoryName(_sessionPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using StreamWriter writer = new(_sessionPath, false, Encoding.UTF8);
        writer.WriteLine("[PATIENTS]");

        Patient[] patients = clinic.Patients.GetAll();
        for (int i = 0; i < patients.Length; i++)
        {
            Patient patient = patients[i];
            writer.WriteLine(
                $"{patient.FirstName},{patient.LastName},{patient.DateOfBirth:dd.MM.yyyy}," +
                $"{patient.BloodType},{patient.Phone}");
        }

        writer.WriteLine("[END]");
    }

    public int Load(Clinic clinic)
    {
        if (clinic is null)
        {
            throw new ArgumentNullException(nameof(clinic));
        }

        if (!Exists())
        {
            return 0;
        }

        string[] lines;
        try
        {
            lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
        }
        catch (Exception)
        {
            return 0;
        }

        int addedCount = 0;
        string currentSection = string.Empty;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Length == 0)
            {
                continue;
            }

            if (line.StartsWith("[", StringComparison.Ordinal))
            {
                currentSection = line;
                if (line.Equals("[END]", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                continue;
            }

            if (!currentSection.Equals("[PATIENTS]", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                {
                    continue;
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                string dateText = parts[2].Trim();
                string bloodTypeText = parts[3].Trim();
                string phone = parts[4].Trim();

                if (!DateTime.TryParseExact(
                        dateText,
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime dateOfBirth))
                {
                    continue;
                }

                BloodType bloodType =
                    (BloodType)Enum.Parse(typeof(BloodType), bloodTypeText);

                Patient[] existingPatients = clinic.Patients.GetAll();
                bool duplicate = false;
                for (int patientIndex = 0; patientIndex < existingPatients.Length; patientIndex++)
                {
                    Patient existing = existingPatients[patientIndex];
                    if (existing.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
                        && existing.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)
                        && existing.DateOfBirth.Date == dateOfBirth.Date)
                    {
                        duplicate = true;
                        break;
                    }
                }

                if (duplicate)
                {
                    continue;
                }

                Patient patient = new(firstName, lastName, dateOfBirth, bloodType, phone);
                clinic.Patients.Add(patient);
                addedCount++;
            }
            catch (Exception)
            {
                continue;
            }
        }

        return addedCount;
    }
}
