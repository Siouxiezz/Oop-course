using System;
using ClinicApp;

static void ShowDoctorsDemo()
{
    var doctor1 = new Doctor("Oleg", "Sidorenko", "Cardiology", "LIC-001", "0441234567")
    {
        WorkStartHour = 8,
        WorkEndHour = 16
    };

    var doctor2 = new Doctor("Natalia", "Moroz", "Neurology", "LIC-002", "0442345678")
    {
        WorkStartHour = 9,
        WorkEndHour = 18
    };

    var doctor3 = new Doctor("Andriy", "Vlasenko", "Pediatrics", "LIC-003", "0443456789");

    var doctor4 = new Doctor("Maria", "Boiko", "Therapy", "LIC-004", "0444567890")
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
ShowPatientsMenu();
