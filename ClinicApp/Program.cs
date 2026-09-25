using System;
using ClinicApp;

static void ShowAppointmentsMenu()
{
    var patientManager = new PatientManager();
    var doctorManager = new DoctorManager();

    var patients = new[]
    {
        new Patient("Ivan", "Petrenko", new DateTime(1985, 4, 12), "A+", "0501234567"),
        new Patient("Olena", "Koval", new DateTime(1993, 2, 8), "B-", "0672345678"),
        new Patient("Maxim", "Boyko", new DateTime(2010, 5, 15), "O+", "0933456789")
    };

    var doctors = new[]
    {
        new Doctor("Oleg", "Sidorenko", "Cardiology", "LIC-001", "0441234567")
        {
            WorkStartHour = 8,
            WorkEndHour = 16
        },
        new Doctor("Natalia", "Moroz", "Neurology", "LIC-002", "0442345678")
        {
            WorkStartHour = 9,
            WorkEndHour = 18
        },
        new Doctor("Andriy", "Vlasenko", "Pediatrics", "LIC-003", "0443456789")
        {
            WorkStartHour = 8,
            WorkEndHour = 17
        }
    };

    foreach (var patient in patients)
    {
        patientManager.Add(patient);
    }

    foreach (var doctor in doctors)
    {
        doctorManager.Add(doctor);
    }

    var appointmentManager = new AppointmentManager(patientManager, doctorManager);

    appointmentManager.Book(patients[0].Id, doctors[0].Id, new DateTime(2026, 5, 9, 10, 0, 0));
    appointmentManager.Book(patients[1].Id, doctors[1].Id, new DateTime(2026, 5, 9, 11, 0, 0), 45);
    appointmentManager.Book(patients[2].Id, doctors[2].Id, new DateTime(2026, 5, 10, 9, 0, 0), 20);

    while (true)
    {
        Console.WriteLine("=== Appointments ===");
        Console.WriteLine("1. Show all");
        Console.WriteLine("2. Book");
        Console.WriteLine("3. Cancel");
        Console.WriteLine("4. Complete");
        Console.WriteLine("5. Patient appointments");
        Console.WriteLine("6. Doctor appointments");
        Console.WriteLine("7. Appointments for date");
        Console.WriteLine("8. Upcoming");
        Console.WriteLine("0. Back");
        Console.Write("Choose an option: ");

        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Invalid choice. Please try again.");
            Console.WriteLine();
            continue;
        }

        switch (choice)
        {
            case 1:
                Console.WriteLine("=== All Appointments ===");
                appointmentManager.DisplayList(appointmentManager.GetAll());
                break;

            case 2:
                Console.WriteLine("Available patients:");
                patientManager.DisplayAll();
                Console.WriteLine("Available doctors:");
                doctorManager.DisplayAll();

                Console.Write("Patient ID: ");
                int patientId = int.TryParse(Console.ReadLine(), out int parsedPatientId) ? parsedPatientId : 0;
                Console.Write("Doctor ID: ");
                int doctorId = int.TryParse(Console.ReadLine(), out int parsedDoctorId) ? parsedDoctorId : 0;
                Console.Write("Date and Time (yyyy-MM-dd HH:mm): ");
                string scheduleText = Console.ReadLine() ?? string.Empty;
                DateTime scheduledAt = DateTime.TryParse(scheduleText, out DateTime parsedScheduledAt)
                    ? parsedScheduledAt
                    : DateTime.Now.AddDays(1);
                Console.Write("Duration (min): ");
                int durationMinutes = int.TryParse(Console.ReadLine(), out int parsedDuration) ? parsedDuration : 30;

                appointmentManager.Book(patientId, doctorId, scheduledAt, durationMinutes);
                break;

            case 3:
                Console.Write("Enter appointment ID to cancel: ");
                if (int.TryParse(Console.ReadLine(), out int cancelId) && appointmentManager.Cancel(cancelId, "Patient couldn't make it"))
                {
                    Console.WriteLine($"Appointment [{cancelId}] cancelled.");
                }
                else
                {
                    Console.WriteLine("Appointment not found or cannot be cancelled.");
                }
                break;

            case 4:
                Console.Write("Enter appointment ID to complete: ");
                if (int.TryParse(Console.ReadLine(), out int completeId) && appointmentManager.Complete(completeId))
                {
                    Console.WriteLine($"Appointment [{completeId}] completed.");
                }
                else
                {
                    Console.WriteLine("Appointment not found or cannot be completed.");
                }
                break;

            case 5:
                Console.Write("Enter patient ID: ");
                if (int.TryParse(Console.ReadLine(), out int patientFilterId))
                {
                    var patientAppointments = appointmentManager.GetByPatient(patientFilterId);
                    Console.WriteLine($"Appointments for patient #{patientFilterId}:");
                    appointmentManager.DisplayList(patientAppointments);
                }
                else
                {
                    Console.WriteLine("Invalid patient ID.");
                }
                break;

            case 6:
                Console.Write("Enter doctor ID: ");
                if (int.TryParse(Console.ReadLine(), out int doctorFilterId))
                {
                    var doctorAppointments = appointmentManager.GetByDoctor(doctorFilterId);
                    Console.WriteLine($"Appointments for doctor #{doctorFilterId}:");
                    appointmentManager.DisplayList(doctorAppointments);
                }
                else
                {
                    Console.WriteLine("Invalid doctor ID.");
                }
                break;

            case 7:
                Console.Write("Enter date (yyyy-MM-dd): ");
                string dateText = Console.ReadLine() ?? string.Empty;
                if (DateTime.TryParse(dateText, out DateTime dateFilter))
                {
                    var dateAppointments = appointmentManager.GetByDate(dateFilter);
                    Console.WriteLine($"Appointments for date {dateFilter:dd.MM.yyyy}:");
                    appointmentManager.DisplayList(dateAppointments);
                }
                else
                {
                    Console.WriteLine("Invalid date.");
                }
                break;

            case 8:
                var upcomingAppointments = appointmentManager.GetUpcoming();
                Console.WriteLine("Upcoming appointments:");
                appointmentManager.DisplayList(upcomingAppointments);
                break;

            case 0:
                return;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
        Console.Clear();
    }
}

static void ShowDoctorsDemo()
{
    var doctor1 = new Doctor("Oleg", "Sidorenko", "Кардіологія", "LIC-001", "0441234567")
    {
        WorkStartHour = 8,
        WorkEndHour = 16
    };

    var doctor2 = new Doctor("Natalia", "Moroz", "Неврологія", "LIC-002", "0442345678")
    {
        WorkStartHour = 9,
        WorkEndHour = 18
    };

    var doctor3 = new Doctor("Andriy", "Vlasenko", "Педіатрія", "LIC-003", "0443456789")
    {
        WorkStartHour = 8,
        WorkEndHour = 17
    };

    var doctor4 = new Doctor("Maria", "Boiko", "Терапія", "LIC-004", "0444567890")
    {
        WorkStartHour = 10,
        WorkEndHour = 14
    };

    Console.WriteLine("=== Doctors ===");
    Console.WriteLine(doctor1);
    Console.WriteLine(doctor2);
    Console.WriteLine(doctor3);
    Console.WriteLine(doctor4);
    Console.WriteLine();
}

static void ShowDoctorsMenu()
{
    var manager = new DoctorManager();

    var doctors = new[]
    {
        new Doctor("Oleg", "Sidorenko", "Cardiology", "LIC-001", "0441234567")
        {
            WorkStartHour = 8,
            WorkEndHour = 16
        },
        new Doctor("Natalia", "Moroz", "Neurology", "LIC-002", "0442345678")
        {
            WorkStartHour = 9,
            WorkEndHour = 18
        },
        new Doctor("Andriy", "Vlasenko", "Pediatrics", "LIC-003", "0443456789")
        {
            WorkStartHour = 8,
            WorkEndHour = 17
        },
        new Doctor("Maria", "Boiko", "Therapy", "LIC-004", "0444567890")
        {
            WorkStartHour = 10,
            WorkEndHour = 14
        }
    };

    foreach (var doctor in doctors)
    {
        manager.Add(doctor);
    }

    while (true)
    {
        Console.WriteLine("=== Doctors ===");
        Console.WriteLine("1. Show all");
        Console.WriteLine("2. Add");
        Console.WriteLine("3. Find by specialty");
        Console.WriteLine("4. Delete");
        Console.WriteLine("5. Statistics");
        Console.WriteLine("0. Back");
        Console.Write("Choose an option: ");

        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Invalid choice. Please try again.");
            Console.WriteLine();
            continue;
        }

        switch (choice)
        {
            case 1:
                manager.DisplayAll();
                break;

            case 2:
                Console.Write("First Name: ");
                string firstName = Console.ReadLine() ?? string.Empty;
                Console.Write("Last Name: ");
                string lastName = Console.ReadLine() ?? string.Empty;
                Console.Write("Specialty: ");
                string speciality = Console.ReadLine() ?? "General Medicine";
                Console.Write("License Number: ");
                string licenseNumber = Console.ReadLine() ?? "LIC-000";
                Console.Write("Phone: ");
                string phone = Console.ReadLine() ?? "0000000000";
                Console.Write("Work Start Time (hour): ");
                int startHour = int.TryParse(Console.ReadLine(), out int parsedStartHour) ? parsedStartHour : 8;
                Console.Write("Work End Time (hour): ");
                int endHour = int.TryParse(Console.ReadLine(), out int parsedEndHour) ? parsedEndHour : 17;

                var newDoctor = new Doctor(firstName, lastName, speciality, licenseNumber, phone)
                {
                    WorkStartHour = startHour,
                    WorkEndHour = endHour
                };
                manager.Add(newDoctor);
                break;

            case 3:
                Console.Write("Enter specialty: ");
                string searchSpeciality = Console.ReadLine() ?? string.Empty;
                var specialityMatches = manager.FindBySpeciality(searchSpeciality);
                if (specialityMatches.Length == 0)
                {
                    Console.WriteLine("No doctors found with that specialty.");
                }
                else
                {
                    foreach (var doctor in specialityMatches)
                    {
                        Console.WriteLine(doctor);
                    }
                }
                break;

            case 4:
                Console.Write("Enter doctor ID to delete: ");
                if (int.TryParse(Console.ReadLine(), out int idToDelete) && manager.Remove(idToDelete))
                {
                    Console.WriteLine($"Doctor with ID {idToDelete} deleted.");
                }
                else
                {
                    Console.WriteLine("Doctor with that ID not found.");
                }
                break;

            case 5:
                manager.DisplayStats();
                break;

            case 0:
                return;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
        Console.Clear();
    }
}

static void ShowPatientsMenu()
{
    var manager = new PatientManager();

    var patients = new[]
    {
        new Patient("Ivan", "Petrenko", new DateTime(1985, 4, 12), "A+", "0501234567"),
        new Patient("Olena", "Koval", new DateTime(1993, 2, 8), "B-", "0672345678"),
        new Patient("Maxim", "Boyko", new DateTime(2010, 5, 15), "O+", "0933456789"),
        new Patient("Maria", "Tkach", new DateTime(1999, 11, 5), "Unknown", "0000000000")
    };

    foreach (var patient in patients)
    {
        manager.Add(patient);
    }

    while (true)
    {
        Console.WriteLine("=== Patients ===");
        Console.WriteLine("1. Show all");
        Console.WriteLine("2. Add");
        Console.WriteLine("3. Find by name");
        Console.WriteLine("4. Delete");
        Console.WriteLine("5. Statistics");
        Console.WriteLine("0. Back");
        Console.Write("Choose an option: ");

        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Invalid choice. Please try again.");
            Console.WriteLine();
            continue;
        }

        switch (choice)
        {
            case 1:
                manager.DisplayAll();
                break;

            case 2:
                Console.Write("First Name: ");
                string firstName = Console.ReadLine() ?? string.Empty;
                Console.Write("Last Name: ");
                string lastName = Console.ReadLine() ?? string.Empty;
                Console.Write("Birth Year: ");
                int year = int.TryParse(Console.ReadLine(), out int parsedYear) ? parsedYear : DateTime.Today.Year;
                Console.Write("Birth Month: ");
                int month = int.TryParse(Console.ReadLine(), out int parsedMonth) ? parsedMonth : 1;
                Console.Write("Birth Day: ");
                int day = int.TryParse(Console.ReadLine(), out int parsedDay) ? parsedDay : 1;
                Console.Write("Blood Type: ");
                string bloodType = Console.ReadLine() ?? "Unknown";
                Console.Write("Phone: ");
                string phone = Console.ReadLine() ?? "0000000000";

                var newPatient = new Patient(firstName, lastName, new DateTime(year, month, day), bloodType, phone);
                manager.Add(newPatient);
                break;

            case 3:
                Console.Write("Enter part of the name or surname: ");
                string search = Console.ReadLine() ?? string.Empty;
                var matches = manager.FindByName(search);
                if (matches.Length == 0)
                {
                    Console.WriteLine("No results found.");
                }
                else
                {
                    foreach (var patient in matches)
                    {
                        Console.WriteLine(patient);
                    }
                }
                break;

            case 4:
                Console.Write("Enter the ID of the patient to delete: ");
                if (int.TryParse(Console.ReadLine(), out int idToDelete) && manager.Remove(idToDelete))
                {
                    Console.WriteLine($"Patient with ID {idToDelete} has been deleted.");
                }
                else
                {
                    Console.WriteLine("Patient with that ID not found.");
                }
                break;

            case 5:
                manager.DisplayStats();
                break;

            case 0:
                return;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
        Console.Clear();
    }
}

ShowDoctorsDemo();
ShowDoctorsMenu();
ShowPatientsMenu();
ShowAppointmentsMenu();
