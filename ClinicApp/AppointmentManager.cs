using System;

namespace ClinicApp;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private readonly Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count;
    private readonly PatientManager _patients;
    private readonly DoctorManager _doctors;

    public int Count => _count;

    public Appointment? this[int index] => index >= 0 && index < _count ? _appointments[index] : null;

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients ?? throw new ArgumentNullException(nameof(patients));
        _doctors = doctors ?? throw new ArgumentNullException(nameof(doctors));
    }

    public Appointment[] GetAll()
    {
        var appointments = new Appointment[_count];

        for (int i = 0; i < _count; i++)
        {
            appointments[i] = _appointments[i];
        }

        return appointments;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        var patient = _patients.FindById(patientId);
        if (patient == null)
        {
            Console.WriteLine($"Error: patient with ID {patientId} was not found.");
            return false;
        }

        var doctor = _doctors.FindById(doctorId);
        if (doctor == null)
        {
            Console.WriteLine($"Error: doctor with ID {doctorId} was not found.");
            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine($"Error: appointment limit reached ({MaxAppointments}).");
            return false;
        }

        var appointment = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
        _appointments[_count] = appointment;
        _count++;

        Console.WriteLine($"Appointment [{appointment.Id}] created: {patient.FullName} → {doctor.FullName} at {scheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i];
            }
        }

        return null;
    }

    public bool Cancel(int id, string reason = "")
    {
        var appointment = FindById(id);
        if (appointment == null)
        {
            return false;
        }

        bool result = appointment.Cancel(reason);
        if (result)
        {
            Console.WriteLine($"Appointment [{appointment.Id}] cancelled.");
        }

        return result;
    }

    public bool Complete(int id)
    {
        var appointment = FindById(id);
        if (appointment == null)
        {
            return false;
        }

        bool result = appointment.Complete();
        if (result)
        {
            Console.WriteLine($"Appointment [{appointment.Id}] completed.");
        }

        return result;
    }

    public Appointment[] GetByPatient(int patientId)
    {
        return FilterAppointments(appointment => appointment.PatientId == patientId);
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        return FilterAppointments(appointment => appointment.DoctorId == doctorId);
    }

    public Appointment[] GetByDate(DateTime date)
    {
        DateTime dateOnly = date.Date;
        return FilterAppointments(appointment => appointment.ScheduledAt.Date == dateOnly);
    }

    public Appointment[] GetByDate(int year, int month, int day)
    {
        return GetByDate(new DateTime(year, month, day));
    }

    public Appointment[] GetUpcoming()
    {
        return FilterAppointments(appointment => appointment.IsUpcoming);
    }

    public void DisplayAppointment(Appointment appointment)
    {
        if (appointment == null)
        {
            throw new ArgumentNullException(nameof(appointment));
        }

        var patient = _patients.FindById(appointment.PatientId);
        var doctor = _doctors.FindById(appointment.DoctorId);

        string patientName = patient?.FullName ?? $"Patient #{appointment.PatientId}";
        string doctorName = doctor?.FullName ?? $"Doctor #{appointment.DoctorId}";
        string notePart = string.IsNullOrEmpty(appointment.Notes) ? string.Empty : $" | {appointment.Notes}";

        Console.WriteLine($"[{appointment.Id}] {patientName} → {doctorName} | {appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm} | {appointment.Status}{notePart}");
    }

    public void DisplayList(Appointment[] appointments)
    {
        if (appointments == null || appointments.Length == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }

        for (int i = 0; i < appointments.Length; i++)
        {
            DisplayAppointment(appointments[i]);
        }
    }

    private Appointment[] FilterAppointments(Func<Appointment, bool> predicate)
    {
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (predicate(_appointments[i]))
            {
                matches++;
            }
        }

        var result = new Appointment[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (predicate(_appointments[i]))
            {
                result[index++] = _appointments[i];
            }
        }

        return result;
    }
}
