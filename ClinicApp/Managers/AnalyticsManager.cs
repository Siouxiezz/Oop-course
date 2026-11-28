using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;

    public AnalyticsManager(
        AppointmentManager appointments,
        DoctorManager doctors,
        PatientManager patients)
    {
        _appointments = appointments ?? throw new ArgumentNullException(nameof(appointments));
        _doctors = doctors ?? throw new ArgumentNullException(nameof(doctors));
        _patients = patients ?? throw new ArgumentNullException(nameof(patients));
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        return doctors.Select(doctor =>
        {
            IEnumerable<Appointment> doctorAppointments = appointments
                .Where(appointment => appointment.DoctorId == doctor.Id);
            int appointmentCount = doctorAppointments.Count();
            decimal totalRevenue = doctorAppointments.Sum(appointment => appointment.GetCost());
            DateTime lastAppointmentDate = doctorAppointments.Any()
                ? doctorAppointments.Max(appointment => appointment.ScheduledAt)
                : DateTime.MinValue;

            return new DoctorStats(
                doctor.Id,
                doctor.FullName,
                appointmentCount,
                totalRevenue,
                lastAppointmentDate);
        });
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] patients = _patients.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        return patients.Select(patient =>
        {
            IEnumerable<Appointment> patientAppointments = appointments
                .Where(appointment => appointment.PatientId == patient.Id);
            int visitCount = patientAppointments.Count();
            decimal totalSpent = patientAppointments.Sum(appointment => appointment.GetCost());
            DateTime lastVisitDate = patientAppointments.Any()
                ? patientAppointments.Max(appointment => appointment.ScheduledAt)
                : DateTime.MinValue;

            return new PatientStats(
                patient.Id,
                patient.FullName,
                visitCount,
                totalSpent,
                lastVisitDate);
        });
    }
}
