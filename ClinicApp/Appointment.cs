using System;

namespace ClinicApp;

public class Appointment
{
    private static int _nextId = 1;

    public int Id { get; }

    public int PatientId { get; }

    public int DoctorId { get; }

    public DateTime ScheduledAt { get; set; }

    public int DurationMinutes { get; set; }

    public string Status { get; private set; }

    public string Notes { get; private set; }

    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);

    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == "Scheduled";

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Id = _nextId++;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = "Scheduled";
        Notes = string.Empty;
    }

    public bool Cancel(string reason = "")
    {
        if (Status != "Scheduled")
        {
            Console.WriteLine("Can't cancel appointment. It's not scheduled.");
            return false;
        }

        Status = "Cancelled";
        Notes = reason ?? string.Empty;
        return true;
    }

    public bool Complete()
    {
        if (Status != "Scheduled")
        {
            return false;
        }

        Status = "Completed";
        return true;
    }

    public override string ToString()
    {
        string notePart = string.IsNullOrEmpty(Notes) ? string.Empty : $" | {Notes}";
        return $"[{Id}] Patient #{PatientId} → Doctor #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}{notePart}";
    }
}
