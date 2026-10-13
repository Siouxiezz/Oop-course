using System.Globalization;
using System.Text;
using System.Threading;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
Console.OutputEncoding = Encoding.UTF8;

Clinic clinic = new("Medical Clinic");

Patient patient1 = new("Ivan", "Petrenko", new DateTime(1985, 8, 9), BloodType.APositive, "0501234567");
Patient patient2 = new("Olena", "Koval", new DateTime(1993, 9, 10), BloodType.BNegative, "0672345678");
Patient patient3 = new("Maxim", "Boyko", new DateTime(2010, 8, 9), BloodType.ONegative, "0933456789");
Patient patient4 = new("Olena", "Koval", new DateTime(2010, 9, 10), BloodType.Unknown, "0000000000");
Patient patient5 = new("Maria", "Tkach", new DateTime(2000, 7, 17), BloodType.Unknown, "0000000000");

clinic.Patients.Add(patient1);
clinic.Patients.Add(patient2);
clinic.Patients.Add(patient3);
clinic.Patients.Add(patient4);
clinic.Patients.Add(patient5);

Doctor doctor1 = new("Oleg", "Sidorenko", Speciality.Cardiology, "LIC-001", "0441234567", new WorkSchedule(8, 16));
Doctor doctor2 = new("Natalia", "Moroz", Speciality.Neurology, "LIC-002", "0442345678", new WorkSchedule(9, 18));
Doctor doctor3 = new("Andriy", "Vlasenko", Speciality.Pediatrics, "LIC-003", "0443456789", new WorkSchedule(8, 17));

clinic.Doctors.Add(doctor1);
clinic.Doctors.Add(doctor2);
clinic.Doctors.Add(doctor3);

clinic.Appointments.Book(patient1.Id, doctor1.Id, DateTime.Today.AddDays(1).AddHours(10), 30);
clinic.Appointments.Book(patient2.Id, doctor2.Id, DateTime.Today.AddDays(2).AddHours(11), 45);

clinic.MedicalRecords.Add(new Diagnosis(
    patient1.Id, doctor1.Id, DateTime.Today.AddDays(-30),
    "I10", "Гіпертонічна хвороба", isChronic: true));
clinic.MedicalRecords.Add(new Diagnosis(
    patient1.Id, doctor2.Id, DateTime.Today.AddDays(-5),
    "J06.9", "Гострий ринофарингіт"));
clinic.MedicalRecords.Add(new LabResult(
    patient1.Id, doctor1.Id, DateTime.Today.AddDays(-4),
    "Гемоглобін", 145, "г/л", "120–160", isNormal: true));
clinic.MedicalRecords.Add(new LabResult(
    patient1.Id, doctor1.Id, DateTime.Today.AddDays(-3),
    "Холестерин", 6.2, "ммоль/л", "< 5.2", isNormal: false));
clinic.MedicalRecords.Add(new Prescription(
    patient1.Id, doctor1.Id, DateTime.Today.AddDays(-5),
    "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
clinic.MedicalRecords.Add(new Prescription(
    patient2.Id, doctor2.Id, DateTime.Today.AddDays(-40),
    "Ібупрофен", "200 мг", 10, "за потреби"));
clinic.MedicalRecords.Add(new Diagnosis(
    patient2.Id, doctor3.Id, DateTime.Today.AddDays(-12),
    "G43.9", "Мігрень"));
clinic.MedicalRecords.Add(new LabResult(
    patient3.Id, doctor3.Id, DateTime.Today.AddDays(-2),
    "Глюкоза", 5.1, "ммоль/л", "3.9–5.5", isNormal: true));

RunMainMenu(clinic);

void RunMainMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n── Головне меню ──────────────");
        Console.WriteLine("  1 — Пацієнти");
        Console.WriteLine("  2 — Лікарі");
        Console.WriteLine("  3 — Записи на прийом");
        Console.WriteLine("  4 — Медична картка");
        Console.WriteLine("  5 — Звіт");
        Console.WriteLine("  0 — Вихід");

        int? choice = ReadMenuChoice("Оберіть: ");
        if (choice is null or 0)
        {
            return;
        }

        switch (choice)
        {
            case 1:
                PatientsMenu(clinic);
                break;
            case 2:
                DoctorsMenu(clinic);
                break;
            case 3:
                AppointmentsMenu(clinic);
                break;
            case 4:
                MedicalRecordsMenu(clinic);
                break;
            case 5:
                clinic.GenerateReport();
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

void PatientsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n── Пацієнти ──────────────────");
        Console.WriteLine("  1 — Список пацієнтів");
        Console.WriteLine("  2 — Статистика");
        Console.WriteLine("  0 — Назад");

        int? choice = ReadMenuChoice("Оберіть: ");
        if (choice is null or 0)
        {
            return;
        }

        switch (choice)
        {
            case 1:
                clinic.Patients.DisplayAll();
                break;
            case 2:
                clinic.Patients.DisplayStats();
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

void DoctorsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n── Лікарі ────────────────────");
        Console.WriteLine("  1 — Список лікарів");
        Console.WriteLine("  2 — Статистика");
        Console.WriteLine("  0 — Назад");

        int? choice = ReadMenuChoice("Оберіть: ");
        if (choice is null or 0)
        {
            return;
        }

        switch (choice)
        {
            case 1:
                clinic.Doctors.DisplayAll();
                break;
            case 2:
                clinic.Doctors.DisplayStats();
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

void AppointmentsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n── Записи на прийом ──────────");
        Console.WriteLine("  1 — Усі записи");
        Console.WriteLine("  2 — Майбутні записи");
        Console.WriteLine("  3 — Розклад на завтра");
        Console.WriteLine("  0 — Назад");

        int? choice = ReadMenuChoice("Оберіть: ");
        if (choice is null or 0)
        {
            return;
        }

        switch (choice)
        {
            case 1:
                clinic.Appointments.DisplayList(clinic.Appointments.GetAll());
                break;
            case 2:
                clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                break;
            case 3:
                clinic.DisplaySchedule(DateTime.Today.AddDays(1));
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

void MedicalRecordsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n── Медична картка ────────────");
        Console.WriteLine("  1 — Картка пацієнта");
        Console.WriteLine("  2 — Усі записи пацієнта");
        Console.WriteLine("  3 — Додати діагноз");
        Console.WriteLine("  4 — Додати аналіз");
        Console.WriteLine("  5 — Додати рецепт");
        Console.WriteLine("  6 — Записи лікаря");
        Console.WriteLine("  0 — Назад");

        int? choice = ReadMenuChoice("Оберіть: ");
        if (choice is null or 0)
        {
            return;
        }

        switch (choice)
        {
            case 1:
                DisplayPatientRecordSummary(clinic);
                break;
            case 2:
                DisplayPatientRecords(clinic);
                break;
            case 3:
                AddDiagnosis(clinic);
                break;
            case 4:
                AddLabResult(clinic);
                break;
            case 5:
                AddPrescription(clinic);
                break;
            case 6:
                DisplayDoctorRecords(clinic);
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

void DisplayPatientRecordSummary(Clinic clinic)
{
    if (!TryReadPatient(clinic, out Patient patient))
    {
        return;
    }

    clinic.MedicalRecords.DisplayPatientSummary(patient.Id);
}

void DisplayPatientRecords(Clinic clinic)
{
    if (!TryReadPatient(clinic, out Patient patient))
    {
        return;
    }

    MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(patient.Id);
    clinic.MedicalRecords.DisplayList(records);
}

void DisplayDoctorRecords(Clinic clinic)
{
    clinic.Doctors.DisplayAll();
    if (!TryReadInt("ID лікаря: ", out int doctorId))
    {
        return;
    }

    if (clinic.Doctors.FindById(doctorId) is null)
    {
        Console.WriteLine($"Лікаря з ID {doctorId} не знайдено.");
        return;
    }

    clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByDoctor(doctorId));
}

void AddDiagnosis(Clinic clinic)
{
    if (!TryReadRecordOwners(clinic, out Patient patient, out Doctor doctor))
    {
        return;
    }

    string diagnosisCode = ReadText("Код діагнозу (напр. J06.9): ");
    string description = ReadText("Опис: ");
    if (!TryReadYesNo("Хронічне? (1=так, 0=ні): ", out bool isChronic))
    {
        return;
    }

    try
    {
        clinic.MedicalRecords.Add(new Diagnosis(
            patient.Id, doctor.Id, DateTime.Today, diagnosisCode, description, isChronic));
    }
    catch (ArgumentOutOfRangeException exception)
    {
        Console.WriteLine("Помилка: " + exception.Message);
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine("Помилка: " + exception.Message);
    }
}

void AddLabResult(Clinic clinic)
{
    if (!TryReadRecordOwners(clinic, out Patient patient, out Doctor doctor))
    {
        return;
    }

    string testName = ReadText("Назва аналізу: ");
    Console.Write("Значення (число): ");
    string valueText = Console.ReadLine() ?? string.Empty;
    if (!double.TryParse(valueText, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
        || !double.IsFinite(value))
    {
        Console.WriteLine("Некоректне число.");
        return;
    }

    string unit = ReadText("Одиниці виміру: ");
    string referenceRange = ReadText("Норма (напр. 4.0–9.0): ");
    if (!TryReadYesNo("В нормі? (1=так, 0=ні): ", out bool isNormal))
    {
        return;
    }

    try
    {
        clinic.MedicalRecords.Add(new LabResult(
            patient.Id, doctor.Id, DateTime.Today,
            testName, value, unit, referenceRange, isNormal));
    }
    catch (ArgumentOutOfRangeException exception)
    {
        Console.WriteLine("Помилка: " + exception.Message);
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine("Помилка: " + exception.Message);
    }
}

void AddPrescription(Clinic clinic)
{
    if (!TryReadRecordOwners(clinic, out Patient patient, out Doctor doctor))
    {
        return;
    }

    string medicationName = ReadText("Препарат: ");
    string dosage = ReadText("Дозування: ");
    if (!TryReadInt("Кількість днів: ", out int durationDays))
    {
        return;
    }

    Console.Write("Інструкція (Enter — пропустити): ");
    string instructions = Console.ReadLine() ?? string.Empty;

    try
    {
        clinic.MedicalRecords.Add(new Prescription(
            patient.Id, doctor.Id, DateTime.Today,
            medicationName, dosage, durationDays, instructions));
    }
    catch (ArgumentOutOfRangeException exception)
    {
        Console.WriteLine("Помилка: " + exception.Message);
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine("Помилка: " + exception.Message);
    }
}

bool TryReadRecordOwners(Clinic clinic, out Patient patient, out Doctor doctor)
{
    clinic.Patients.DisplayAll();
    clinic.Doctors.DisplayAll();

    if (!TryReadInt("ID пацієнта: ", out int patientId))
    {
        patient = null!;
        doctor = null!;
        return false;
    }

    Patient? foundPatient = clinic.Patients.FindById(patientId);
    if (foundPatient is null)
    {
        Console.WriteLine($"Пацієнта з ID {patientId} не знайдено.");
        patient = null!;
        doctor = null!;
        return false;
    }

    if (!TryReadInt("ID лікаря: ", out int doctorId))
    {
        patient = null!;
        doctor = null!;
        return false;
    }

    Doctor? foundDoctor = clinic.Doctors.FindById(doctorId);
    if (foundDoctor is null)
    {
        Console.WriteLine($"Лікаря з ID {doctorId} не знайдено.");
        patient = null!;
        doctor = null!;
        return false;
    }

    patient = foundPatient;
    doctor = foundDoctor;
    return true;
}

bool TryReadPatient(Clinic clinic, out Patient patient)
{
    if (!TryReadInt("ID пацієнта: ", out int patientId))
    {
        patient = null!;
        return false;
    }

    Patient? foundPatient = clinic.Patients.FindById(patientId);
    if (foundPatient is null)
    {
        Console.WriteLine($"Пацієнта з ID {patientId} не знайдено.");
        patient = null!;
        return false;
    }

    patient = foundPatient;
    return true;
}

bool TryReadYesNo(string prompt, out bool answer)
{
    if (!TryReadInt(prompt, out int value))
    {
        answer = false;
        return false;
    }

    if (value != 0 && value != 1)
    {
        Console.WriteLine("Введіть 1 або 0.");
        answer = false;
        return false;
    }

    answer = value == 1;
    return true;
}

int? ReadMenuChoice(string prompt)
{
    Console.Write(prompt);
    string? input = Console.ReadLine();
    if (input is null)
    {
        return null;
    }

    if (!int.TryParse(input, out int choice))
    {
        Console.WriteLine("Некоректне число.");
        return -1;
    }

    return choice;
}

bool TryReadInt(string prompt, out int value)
{
    Console.Write(prompt);
    if (!int.TryParse(Console.ReadLine(), out value))
    {
        Console.WriteLine("Некоректне число.");
        return false;
    }

    return true;
}

string ReadText(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? string.Empty;
}
