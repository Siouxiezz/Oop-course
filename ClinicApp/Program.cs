using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;


Patient p1 = new("Ivan", "Petrenko", new DateTime(1985, 8, 9), BloodType.APositive, "0501234567");
Patient p2 = new("Olena", "Koval", new DateTime(1993, 9, 10), BloodType.BNegative, "0672345678");
Patient p3 = new("Maxim", "Boyko", new DateTime(2010, 8, 9), BloodType.ONegative, "0933456789");
Patient p4 = new("Olena", "Koval", new DateTime(2010, 9, 10), BloodType.Unknown, "0000000000");
Patient p5 = new("Maria", "Tkach", new DateTime(2000, 7, 17), BloodType.Unknown, "0000000000");

Console.WriteLine("\tPatient List\n");
Console.WriteLine(p1);
Console.WriteLine(p2);
Console.WriteLine(p3);
Console.WriteLine(p4);
Console.WriteLine(p5);

Doctor d1 = new("Oleg", "Sidorenko", Speciality.Cardiology, "LIC-001", "0441234567", new WorkSchedule(8, 16));
Doctor d2 = new("Natalia", "Moroz", Speciality.Neurology, "LIC-002", "0442345678", new WorkSchedule(9, 18));
Doctor d3 = new("Andriy", "Vlasenko", Speciality.Pediatrics, "LIC-003", "0443456789", new WorkSchedule(8, 17));

Console.WriteLine("\tDoctor List");
Console.WriteLine(d1);
Console.WriteLine(d2);
Console.WriteLine(d3);

PatientManager patientManager = new();
patientManager.Add(p1);
patientManager.Add(p2);
patientManager.Add(p3);
patientManager.Add(p4);
patientManager.Add(p5);

Console.WriteLine("\nAll Patients:");
patientManager.DisplayAll();

Console.WriteLine("\nPatient Statistics:");
patientManager.DisplayStats();

DoctorManager doctorManager = new();
Console.WriteLine("\nAdding Doctors:");
doctorManager.Add(d1);
doctorManager.Add(d2);
doctorManager.Add(d3);

Console.WriteLine("\nDoctor List:");
doctorManager.DisplayAll();

Console.WriteLine("\nDoctor Statistics:");
doctorManager.DisplayStats();

string specialitySearch = "Cardiology";
Console.WriteLine($"\nSearching for doctors by specialty \"{specialitySearch}\":");
Doctor[] cardiologistsSearch = doctorManager.FindBySpeciality(specialitySearch);
foreach (Doctor doctor in cardiologistsSearch)
{
    Console.WriteLine($"Found: [{doctor.Id}] {doctor.FullName} ({doctor.Speciality})");
}

Console.WriteLine($"\nSearching for doctor with ID {d1.Id}:");
Doctor? foundDoctor = doctorManager.FindById(d1.Id);
if (foundDoctor != null)
{
    Console.WriteLine($"Found: {foundDoctor.FullName}, License: {foundDoctor.LicenseNumber}");
}

Console.WriteLine($"\nRemoving doctor with ID {d2.Id}:");
bool isDoctorRemoved = doctorManager.Remove(d2.Id);
Console.WriteLine(isDoctorRemoved ? "Doctor successfully removed." : "Doctor not found.");

Console.WriteLine("\nUpdated Doctor List:");
doctorManager.DisplayAll();

Appointment a1 = new(p1.Id, d1.Id, DateTime.Now.AddDays(1).AddHours(2), 30);
Appointment a2 = new(p2.Id, d2.Id, DateTime.Now.AddDays(2).AddHours(1), 45);
Appointment a3 = new(p1.Id, d2.Id, DateTime.Now.AddHours(-5), 20);

Console.WriteLine("\nInitial Appointments:");
Console.WriteLine(a1);
Console.WriteLine(a2);
Console.WriteLine(a3);

Console.WriteLine("\nChanging Appointment Statuses:");
a3.Complete();
Console.WriteLine($"Appointment #{a3.Id} completed: {a3}");

a2.Cancel("Patient is ill");
Console.WriteLine($"Appointment #{a2.Id} cancelled: {a2}");

AppointmentManager appointmentManager = new(patientManager, doctorManager);
appointmentManager.Book(p1.Id, d1.Id, DateTime.Now.AddDays(1).Date.AddHours(10), 30);
appointmentManager.Book(p2.Id, d3.Id, DateTime.Now.AddDays(1).Date.AddHours(11), 45);
appointmentManager.Book(p3.Id, d1.Id, DateTime.Now.AddDays(2).Date.AddHours(9), 20);

Console.WriteLine("\nUpcoming Appointments:");
appointmentManager.DisplayList(appointmentManager.GetUpcoming());

int appointmentToCancelId = appointmentManager.GetAll()[0].Id;
Console.WriteLine($"\nCancelling appointment #{appointmentToCancelId}:");
appointmentManager.Cancel(appointmentToCancelId, "Patient could not make it");

Console.WriteLine($"\nAppointments for patient #{p2.Id}:");
appointmentManager.DisplayList(appointmentManager.GetByPatient(p2.Id));

Clinic clinic = new("Medical Clinic");
clinic.Patients.Add(p1);
clinic.Patients.Add(p2);
clinic.Doctors.Add(d1);
clinic.Doctors.Add(d2);
clinic.Appointments.Book(p1.Id, d1.Id, DateTime.Now.AddDays(1).Date.AddHours(10), 30);
clinic.Appointments.Book(p2.Id, d2.Id, DateTime.Now.AddDays(2).Date.AddHours(11), 45);

Console.WriteLine();
clinic.DisplaySchedule(DateTime.Now.AddDays(1));

Console.WriteLine("\nDoctors by exact speciality:");
foreach (Doctor doctor in clinic.Doctors.FindBySpeciality(Speciality.Cardiology))
{
    Console.WriteLine($"Found: {doctor.FullName} ({doctor.Speciality})");
}

Console.WriteLine("\nDoctors matching \"Neuro\":");
foreach (Doctor doctor in clinic.Doctors.FindBySpeciality("Neuro"))
{
    Console.WriteLine($"Found: {doctor.FullName} ({doctor.Speciality})");
}

Console.WriteLine("\nAppointments for tomorrow:");
DateTime tomorrow = DateTime.Today.AddDays(1);
clinic.Appointments.DisplayList(clinic.Appointments.GetByDate(tomorrow.Year, tomorrow.Month, tomorrow.Day));

if (clinic.Patients.TryFindById(p1.Id, out Patient foundPatient))
{
    Console.WriteLine($"Found patient: {foundPatient.FullName}");
}
else
{
    Console.WriteLine("Patient not found.");
}

if (clinic.Doctors.TryFindById(d1.Id, out Doctor foundDoctorByTry))
{
    Console.WriteLine($"Found doctor: {foundDoctorByTry.FullName}");
}
else
{
    Console.WriteLine("Doctor not found.");
}

Console.WriteLine("\nPatients with unknown blood type:");
foreach (Patient patient in patientManager.FindByBloodType(BloodType.Unknown))
{
    Console.WriteLine(patient);
}

Console.WriteLine();
clinic.GenerateReport();

Patient[] patientsNamedOlena = patientManager.FindByName("Olena");
Console.WriteLine("\nPatients matching \"Olena\":");
foreach (Patient patient in patientsNamedOlena)
{
    Console.WriteLine(patient);
}

Patient? patientFoundById = patientManager.FindById(p3.Id);
if (patientFoundById != null)
{
    Console.WriteLine($"\nFound patient: {patientFoundById.FullName}");
}

string patientName = clinic.Patients.FindById(99)?.FullName ?? "Unknown patient";
Console.WriteLine($"Patient with ID 99: {patientName}");
