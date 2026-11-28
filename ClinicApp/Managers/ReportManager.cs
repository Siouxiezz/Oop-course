using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class ReportManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;
    private readonly AnalyticsManager _analytics;

    public ReportManager(
        AppointmentManager appointments,
        DoctorManager doctors,
        PatientManager patients)
    {
        _appointments = appointments ?? throw new ArgumentNullException(nameof(appointments));
        _doctors = doctors ?? throw new ArgumentNullException(nameof(doctors));
        _patients = patients ?? throw new ArgumentNullException(nameof(patients));
        _analytics = new AnalyticsManager(_appointments, _doctors, _patients);
    }

    public IEnumerable<SpecialityReport> GetSpecialityStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        return doctors
            .GroupBy(doctor => doctor.Speciality)
            .Select(group =>
            {
                int[] doctorIds = group.Select(doctor => doctor.Id).ToArray();
                Appointment[] specialityAppointments = appointments
                    .Where(appointment => doctorIds.Contains(appointment.DoctorId))
                    .ToArray();

                return new SpecialityReport(
                    group.Key,
                    group.Count(),
                    specialityAppointments.Length,
                    specialityAppointments.Sum(appointment => appointment.GetCost()));
            })
            .OrderByDescending(report => report.TotalRevenue);
    }

    public string? FindBusiestDoctorName()
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(stat => stat.AppointmentCount)
            .FirstOrDefault()?.FullName;
    }

    public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
    {
        Appointment[] appointments = _appointments.GetAll();
        Patient[] patients = _patients.GetAll();

        return appointments
            .GroupBy(appointment => appointment.PatientId)
            .Where(group => group.Count() >= minVisits)
            .Join(
                patients,
                group => group.Key,
                patient => patient.Id,
                (group, patient) => patient.FullName);
    }

    public IEnumerable<DoctorStats> GetTopEarners(int n)
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(stat => stat.TotalRevenue)
            .Take(n);
    }

    public bool HasAnyUrgentAppointments()
    {
        return _appointments.GetAll()
            .Any(appointment => appointment is UrgentAppointment);
    }

    public IEnumerable<Speciality> GetActiveSpecialities()
    {
        return _doctors.GetAll()
            .Select(doctor => doctor.Speciality)
            .Distinct()
            .OrderBy(speciality => speciality);
    }

    public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
    {
        return _appointments.GetAll()
            .GroupBy(appointment => new
            {
                appointment.ScheduledAt.Year,
                appointment.ScheduledAt.Month
            })
            .Select(group => (
                Year: group.Key.Year,
                Month: group.Key.Month,
                Total: group.Sum(appointment => appointment.GetCost())))
            .OrderBy(month => month.Year)
            .ThenBy(month => month.Month);
    }
}
