using System;
using System.IO;
using System.Text;
using System.Globalization;
using ClinicApp.Events;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public void LogInfo(string message)
    {
        Write("INFO", message);
    }

    public void LogWarning(string message)
    {
        Write("WARN", message);
    }

    public void LogError(string message)
    {
        Write("ERROR", message);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) =>
        LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} → лікар #{e.DoctorId}");

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} скасовано");

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} завершено");

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        string note = string.IsNullOrWhiteSpace(e.Notes) ? "термінова потреба" : e.Notes;
        LogWarning($"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {note}");

        Directory.CreateDirectory("alerts");
        string path = Path.Combine("alerts", $"urgent_{DateTime.Now:yyyy-MM-dd}.txt");
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        File.AppendAllText(path, $"[{timestamp}] [WARN ] ТЕРМІНОВИЙ запис #{e.AppointmentId}: {note}{Environment.NewLine}", Encoding.UTF8);
    }

    public void OnPaymentReceived(object? sender, PaymentEventArgs e) =>
        LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) =>
        LogInfo($"План #{e.PlanId} завершено: пацієнт #{e.PatientId}, діагноз: {e.Diagnosis}");

    public string[] GetLastLines(int n)
    {
        if (n <= 0 || !File.Exists(_logPath))
        {
            return Array.Empty<string>();
        }

        string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int skipCount = Math.Max(0, lines.Length - n);
        string[] result = new string[lines.Length - skipCount];
        Array.Copy(lines, skipCount, result, 0, result.Length);
        return result;
    }

    public void Clear()
    {
        using (StreamWriter writer = new(_logPath, false, Encoding.UTF8))
        {
        }
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }

    private void Write(string level, string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        string line = $"[{timestamp}] [{level.PadRight(5)}] {message}{Environment.NewLine}";
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }
}
