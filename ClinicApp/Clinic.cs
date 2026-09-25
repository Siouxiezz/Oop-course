using System;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; }

    public PatientManager Patients { get; }

    public DoctorManager Doctors { get; }

    public AppointmentManager Appointments { get; }

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
