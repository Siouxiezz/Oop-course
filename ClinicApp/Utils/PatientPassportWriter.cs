using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class PatientPassportWriter
{
    private readonly Clinic _clinic;
    private readonly string _directory;

    public PatientPassportWriter(Clinic clinic, string directory = "patients")
    {
        _clinic = clinic ?? throw new ArgumentNullException(nameof(clinic));
        if (string.IsNullOrWhiteSpace(directory))
        {
            throw new ArgumentException("Passport directory cannot be empty.", nameof(directory));
        }

        _directory = directory;
        Directory.CreateDirectory(_directory);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) =>
        WritePassport(e.PatientId);

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) =>
        WritePassport(e.PatientId);

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) =>
        WritePassport(e.PatientId);

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);
        if (patient is null)
        {
            return;
        }

        string path = Path.Combine(_directory, $"passport_{patientId}.txt");
        using StreamWriter writer = new(path, false, Encoding.UTF8);

        writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)}");
        writer.WriteLine();
        writer.WriteLine($"Ім'я: {patient.FirstName} {patient.LastName}");
        writer.WriteLine($"Дата народження: {patient.DateOfBirth.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)} ({ClinicFormatter.FormatAge(patient.Age)})");
        writer.WriteLine($"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)}   Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");
        writer.WriteLine();

        MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patientId);

        writer.WriteLine("-- Діагнози --");
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Diagnosis diagnosis)
            {
                writer.WriteLine(diagnosis.GetSummary());
            }
        }

        writer.WriteLine("-- Аналізи --");
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is LabResult labResult)
            {
                writer.WriteLine(labResult.GetSummary());
            }
        }

        writer.WriteLine("-- Рецепти --");
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Prescription prescription)
            {
                writer.WriteLine(prescription.GetSummary());
            }
        }

        writer.WriteLine();
        writer.WriteLine("-- Записи --");
        Appointment[] appointments = _clinic.Appointments.GetByPatient(patientId);
        for (int i = 0; i < appointments.Length; i++)
        {
            Appointment appointment = appointments[i];
            writer.WriteLine(
                $"[{appointment.Id}] {appointment.GetDescription()} | " +
                $"{appointment.ScheduledAt.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)} | " +
                $"{appointment.Status} | {appointment.GetCost().ToString("F2", CultureInfo.InvariantCulture)} грн");
        }

        writer.WriteLine();
        writer.WriteLine("-- Плани лікування --");
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patientId);
        for (int i = 0; i < plans.Length; i++)
        {
            TreatmentPlan plan = plans[i];
            writer.WriteLine($"[{plan.Id}] {plan.Diagnosis} | {plan.DurationDays} днів | {plan.Status}");
        }

        writer.WriteLine();
        writer.WriteLine($"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId).ToString("F2", CultureInfo.InvariantCulture)} грн");
    }
}
