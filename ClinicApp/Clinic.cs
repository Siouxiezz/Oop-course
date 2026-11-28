using System;
using ClinicApp.Events;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }

    public PatientManager Patients { get; }

    public DoctorManager Doctors { get; }

    public AppointmentManager Appointments { get; }

    public AnalyticsManager Analytics { get; }

    public ReportManager Reports { get; }

    public BillingManager Billing { get; }

    public MedicalRecordManager MedicalRecords { get; }

    public TreatmentPlanManager TreatmentPlans { get; }

    public WaitingQueue<Patient> WaitingRoom { get; }

    public ClinicLogger Logger { get; }

    public ClinicExporter Exporter { get; }

    public PatientPassportWriter Passport { get; }

    public SessionEventTracker Tracker { get; }

    public SessionManager Session { get; set; }

    public Clinic(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Clinic name cannot be empty.", nameof(name));
        }

        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
        Analytics = new AnalyticsManager(Appointments, Doctors, Patients);
        Reports = new ReportManager(Appointments, Doctors, Patients);
        Billing = new BillingManager(Appointments);
        MedicalRecords = new MedicalRecordManager();
        TreatmentPlans = new TreatmentPlanManager();
        WaitingRoom = new WaitingQueue<Patient>();
        Logger = new ClinicLogger();
        Exporter = new ClinicExporter(this);
        Passport = new PatientPassportWriter(this);
        Tracker = new SessionEventTracker(this);
        Session = new SessionManager();
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        Appointments.AppointmentBooked += Logger.OnAppointmentBooked;
        Appointments.AppointmentCancelled += Logger.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Logger.OnAppointmentCompleted;
        Appointments.UrgentAppointmentBooked += Logger.OnUrgentAppointmentBooked;
        Patients.PatientAdded += Logger.OnPatientAdded;
        Patients.PatientAdded += Passport.OnPatientAdded;
        Appointments.AppointmentCompleted += Passport.OnAppointmentCompleted;
        Billing.PaymentReceived += Logger.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Logger.OnPlanCompleted;
        TreatmentPlans.PlanCompleted += Passport.OnPlanCompleted;
        Patients.PatientAdded += Tracker.OnPatientAdded;
        Appointments.AppointmentBooked += Tracker.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Tracker.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Tracker.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Tracker.OnAppointmentCompleted;
        Billing.PaymentReceived += Tracker.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Tracker.OnPlanCompleted;
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"=== Schedule on {date:dd.MM.yyyy} ===");

        var appointments = Appointments.GetByDate(date);
        if (appointments.Length == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }

        Appointments.DisplayList(appointments);
    }

    public void GenerateReport()
    {
        const int width = 42;
        var upcomingAppointments = Appointments.GetUpcoming();
        var doctors = Doctors.GetAll();

        Console.WriteLine(" " + new string('═', width) + " ");
        Console.WriteLine(" " + $"  Report — {Name}".PadRight(width - 1) + " ");
        Console.WriteLine(" " + new string('═', width) + " ");
        Console.WriteLine("  Patients: " + Patients.Count.ToString().PadLeft(18) + " ");
        Console.WriteLine("  Doctors: " + Doctors.Count.ToString().PadLeft(19) + " ");
        Console.WriteLine("  Upcoming Appointments: " + upcomingAppointments.Length.ToString().PadLeft(10) + " ");
        Console.WriteLine(" " + new string('═', width) + " ");
        Console.WriteLine("  Doctor Workload (Upcoming Appointments):" + " ".PadRight(width - 39) + " ");

        if (doctors.Length == 0)
        {
            Console.WriteLine("   No doctors in the system." + " ".PadRight(width - 26) + " ");
        }
        else
        {
            for (int i = 0; i < doctors.Length; i++)
            {
                Doctor doctor = doctors[i];
                int count = 0;

                for (int j = 0; j < upcomingAppointments.Length; j++)
                {
                    if (upcomingAppointments[j].DoctorId == doctor.Id)
                    {
                        count++;
                    }
                }

                Console.WriteLine($"    {doctor.FullName} ({doctor.Speciality}): {count} appointments".PadRight(width - 1) + " ");
            }
        }

        Console.WriteLine(" " + new string('═', width) + " ");
    }
}
