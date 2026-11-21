using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class CsvImporter
{
    public ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new();

        if (!File.Exists(filePath))
        {
            result.AddError(0, "Файл не знайдено.");
            return result;
        }

        string[] lines;
        try
        {
            lines = File.ReadAllLines(filePath, Encoding.UTF8);
        }
        catch (Exception exception)
        {
            result.AddError(0, exception.Message);
            return result;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            int lineNumber = i + 1;
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                {
                    string fieldWord = parts.Length == 1 ? "поле" : "поля";
                    result.AddError(
                        lineNumber,
                        $"Рядок має {parts.Length} {fieldWord} замість 5.");
                    continue;
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                string dateText = parts[2].Trim();
                string bloodTypeText = parts[3].Trim();
                string phone = parts[4].Trim();

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    result.AddError(lineNumber, "Ім'я не може бути порожнім.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    result.AddError(lineNumber, "Прізвище не може бути порожнім.");
                    continue;
                }

                DateTime dateOfBirth;
                if (!DateTime.TryParseExact(
                    dateText,
                    "dd.MM.yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateOfBirth))
                {
                    result.AddError(lineNumber, "Некоректна дата народження.");
                    continue;
                }

                BloodType bloodType;
                try
                {
                    bloodType = (BloodType)Enum.Parse(typeof(BloodType), bloodTypeText);
                }
                catch (Exception)
                {
                    result.AddError(lineNumber, "Некоректний тип крові.");
                    continue;
                }

                Patient patient = new(
                    firstName,
                    lastName,
                    dateOfBirth,
                    bloodType,
                    phone);

                clinic.Patients.Add(patient);
                result.AddSuccess();
            }
            catch (Exception exception)
            {
                result.AddError(lineNumber, exception.Message);
            }
        }

        return result;
    }
}
