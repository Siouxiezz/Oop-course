using System;
using System.IO;
using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic ?? throw new ArgumentNullException(nameof(clinic));

        if (string.IsNullOrWhiteSpace(baseDir))
        {
            throw new ArgumentException("Base directory cannot be empty.", nameof(baseDir));
        }

        _baseDir = baseDir;
    }

    public string ExportPatients()
    {
        string path = CreateReportPath("patients.txt");
        Patient[] patients = _clinic.Patients.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Пацієнти — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < patients.Length; i++)
        {
            Patient patient = patients[i];
            writer.WriteLine(
                $"{i + 1,3}. [{patient.Id}] {patient.FirstName} {patient.LastName} | " +
                $"{ClinicFormatter.FormatAge(patient.Age)} | " +
                $"{ClinicFormatter.FormatBloodType(patient.BloodType)} | " +
                $"{ClinicFormatter.FormatPhone(patient.Phone)}");
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"Всього: {patients.Length}");
        return path;
    }

    public string ExportAppointments()
    {
        string path = CreateReportPath("appointments.txt");
        Appointment[] appointments = _clinic.Appointments.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Записи — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < appointments.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {appointments[i]}");
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"Всього: {appointments.Length}");
        return path;
    }

    public string ExportBilling()
    {
        string path = CreateReportPath("billing.txt");
        IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
        decimal totalDebt = _clinic.Billing.GetTotalDebt();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Рахунки — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < unpaid.Length; i++)
        {
            if (unpaid[i] is Appointment appointment)
            {
                writer.WriteLine($"{i + 1,3}. {appointment} | Сума: {appointment.GetCost():F2} грн");
            }
            else
            {
                writer.WriteLine($"{i + 1,3}. Сума: {unpaid[i].GetCost():F2} грн");
            }
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"Всього: {unpaid.Length}");
        writer.WriteLine($"Загальний борг: {totalDebt:F2} грн");
        return path;
    }

    public string ExportTreatmentPlans()
    {
        string path = CreateReportPath("treatment_plans.txt");
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Плани лікування — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < plans.Length; i++)
        {
            TreatmentPlan plan = plans[i];
            writer.WriteLine(
                $"{i + 1,3}. [План {plan.Id}] Пацієнт #{plan.PatientId} | " +
                $"Діагноз: {plan.Diagnosis} | Тривалість: {plan.DurationDays} дн. | " +
                $"Статус: {plan.Status}");
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"Всього: {plans.Length}");
        return path;
    }

    public string[] ExportAll()
    {
        return new[]
        {
            ExportPatients(),
            ExportAppointments(),
            ExportBilling(),
            ExportTreatmentPlans()
        };
    }

    private string CreateReportPath(string fileName)
    {
        string dir = Path.Combine(_baseDir, DateTime.Today.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, fileName);
    }
}
