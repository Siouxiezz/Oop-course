using System;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private readonly Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count;
    private readonly PatientManager _patients;
    private readonly DoctorManager _doctors;

    public int Count => _count;

    public Appointment? this[int index] => index >= 0 && index < _count ? _appointments[index] : null;

    public static int CancelAll(ICancellable[] items, string reason = "")
    {
        int cancelledCount = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Cancel(reason))
            {
                cancelledCount++;
            }
        }

        return cancelledCount;
    }

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

    public Appointment[] GetUrgent()
    {
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment)
            {
                matches++;
            }
        }

        var appointments = new Appointment[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment)
            {
                appointments[index++] = _appointments[i];
            }
        }

        return appointments;
    }

    public Appointment[] GetSpecialist()
    {
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment)
            {
                matches++;
            }
        }

        var appointments = new Appointment[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment)
            {
                appointments[index++] = _appointments[i];
            }
        }

        return appointments;
    }

    public Appointment[] GetRegular()
    {
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment)
            {
                matches++;
            }
        }

        var appointments = new Appointment[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment)
            {
                appointments[index++] = _appointments[i];
            }
        }

        return appointments;
    }

    public bool Book(
        int patientId,
        int doctorId,
        DateTime scheduledAt,
        int durationMinutes = 30,
        decimal discountMultiplier = 1m)
    {
        return TryBook(new RegularAppointment(patientId, doctorId, scheduledAt, durationMinutes)
        {
            DiscountMultiplier = discountMultiplier
        });
    }

    public bool BookUrgent(
        int patientId,
        int doctorId,
        DateTime scheduledAt,
        string urgencyNote = "",
        int durationMinutes = 30,
        decimal discountMultiplier = 1m)
    {
        return TryBook(new UrgentAppointment(patientId, doctorId, scheduledAt, urgencyNote, durationMinutes)
        {
            DiscountMultiplier = discountMultiplier
        });
    }

    public bool BookSpecialist(
        int patientId,
        int doctorId,
        DateTime scheduledAt,
        string topic = "",
        int durationMinutes = 45,
        decimal discountMultiplier = 1m)
    {
        return TryBook(new SpecialistAppointment(patientId, doctorId, scheduledAt, topic, durationMinutes)
        {
            DiscountMultiplier = discountMultiplier
        });
    }

    private bool TryBook(Appointment appointment)
    {
        var patient = _patients.FindById(appointment.PatientId);
        if (patient == null)
        {
            Console.WriteLine($"Error: patient with ID {appointment.PatientId} was not found.");
            return false;
        }

        var doctor = _doctors.FindById(appointment.DoctorId);
        if (doctor == null)
        {
            Console.WriteLine($"Error: doctor with ID {appointment.DoctorId} was not found.");
            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine($"Error: appointment limit reached ({MaxAppointments}).");
            return false;
        }

        _appointments[_count] = appointment;
        _count++;

        Console.WriteLine($"Appointment [{appointment.Id}] created: {patient.FullName} → {doctor.FullName} at {appointment.ScheduledAt:dd.MM.yyyy HH:mm}");
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

        Console.WriteLine($"[{appointment.Id}] {appointment.GetDescription()} | {patientName} → {doctorName} | {appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm} | {appointment.Status} | {appointment.GetCost():F2} грн");
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
