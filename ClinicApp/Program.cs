using System;
using ClinicApp;

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
        new Doctor("Oleg", "Sidorenko", "Кардіологія", "LIC-001", "0441234567")
        {
            WorkStartHour = 8,
            WorkEndHour = 16
        },
        new Doctor("Natalia", "Moroz", "Неврологія", "LIC-002", "0442345678")
        {
            WorkStartHour = 9,
            WorkEndHour = 18
        },
        new Doctor("Andriy", "Vlasenko", "Педіатрія", "LIC-003", "0443456789")
        {
            WorkStartHour = 8,
            WorkEndHour = 17
        },
        new Doctor("Maria", "Boiko", "Терапія", "LIC-004", "0444567890")
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
        Console.WriteLine("=== Лікарі ===");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати");
        Console.WriteLine("3. Знайти за спеціальністю");
        Console.WriteLine("4. Видалити");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть опцію: ");

        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
            Console.WriteLine();
            continue;
        }

        switch (choice)
        {
            case 1:
                manager.DisplayAll();
                break;

            case 2:
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine() ?? string.Empty;
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine() ?? string.Empty;
                Console.Write("Спеціальність: ");
                string speciality = Console.ReadLine() ?? "Загальна медицина";
                Console.Write("Номер ліцензії: ");
                string licenseNumber = Console.ReadLine() ?? "LIC-000";
                Console.Write("Телефон: ");
                string phone = Console.ReadLine() ?? "0000000000";
                Console.Write("Час початку роботи (година): ");
                int startHour = int.TryParse(Console.ReadLine(), out int parsedStartHour) ? parsedStartHour : 8;
                Console.Write("Час кінця роботи (година): ");
                int endHour = int.TryParse(Console.ReadLine(), out int parsedEndHour) ? parsedEndHour : 17;

                var newDoctor = new Doctor(firstName, lastName, speciality, licenseNumber, phone)
                {
                    WorkStartHour = startHour,
                    WorkEndHour = endHour
                };
                manager.Add(newDoctor);
                break;

            case 3:
                Console.Write("Введіть спеціальність: ");
                string searchSpeciality = Console.ReadLine() ?? string.Empty;
                var specialityMatches = manager.FindBySpeciality(searchSpeciality);
                if (specialityMatches.Length == 0)
                {
                    Console.WriteLine("Лікарів з такою спеціальністю не знайдено.");
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
                Console.Write("Введіть ID лікаря для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int idToDelete) && manager.Remove(idToDelete))
                {
                    Console.WriteLine($"Лікаря з ID {idToDelete} видалено.");
                }
                else
                {
                    Console.WriteLine("Лікаря з таким ID не знайдено.");
                }
                break;

            case 5:
                manager.DisplayStats();
                break;

            case 0:
                return;

            default:
                Console.WriteLine("Невірна опція.");
                break;
        }

        Console.WriteLine();
        Console.WriteLine("Натисніть Enter, щоб продовжити...");
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
