using System;

namespace ClinicApp;

public class PatientManager
{
    private const int MaxPatients = 100;
    private readonly Patient[] _patients = new Patient[MaxPatients];
    private int _count;

    public int Count => _count;

    public void Add(Patient patient)
    {
        if (patient is null)
        {
            throw new ArgumentNullException(nameof(patient));
        }

        if (_count >= MaxPatients)
        {
            Console.WriteLine($"Cannot add patient. Limit reached ({MaxPatients}).");
            return;
        }

        _patients[_count] = patient;
        _count++;
        Console.WriteLine($"Patient [{patient.Id}] {patient.FullName} added.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }

    public Patient[] FindByName(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Array.Empty<Patient>();
        }

        string searchText = query.Trim();
        string normalizedQuery = searchText.ToLower();

        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            var patient = _patients[i];
            string fullName = $"{patient.FirstName} {patient.LastName}".ToLower();

            if (patient.FirstName.ToLower().Contains(normalizedQuery)
                || patient.LastName.ToLower().Contains(normalizedQuery)
                || fullName.Contains(normalizedQuery))
            {
                matches++;
            }
        }

        var results = new Patient[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            var patient = _patients[i];
            string fullName = $"{patient.FirstName} {patient.LastName}".ToLower();

            if (patient.FirstName.ToLower().Contains(normalizedQuery)
                || patient.LastName.ToLower().Contains(normalizedQuery)
                || fullName.Contains(normalizedQuery))
            {
                results[index++] = patient;
            }
        }

        return results;
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                for (int j = i; j < _count - 1; j++)
                {
                    _patients[j] = _patients[j + 1];
                }

                _patients[_count - 1] = null!;
                _count--;
                return true;
            }
        }

        return false;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine($"=== Patients (0 / {MaxPatients}) ===");
            Console.WriteLine("Patient list is empty.");
            return;
        }

        Console.WriteLine($"=== Patients ({_count} / {MaxPatients}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }

        Console.WriteLine(new string('─', 72));
    }

    public void DisplayStats()
    {
        Console.WriteLine("=== Patient Statistics ===");

        if (_count == 0)
        {
            Console.WriteLine("Total:       0");
            Console.WriteLine("Average Age: 0.0 years");
            Console.WriteLine("Youngest:    -");
            Console.WriteLine("Oldest:      -");
            Console.WriteLine("Adults:      0 of 0");
            Console.WriteLine("============================");
            return;
        }

        int totalAge = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultCount = 0;

        for (int i = 0; i < _count; i++)
        {
            totalAge += _patients[i].Age;

            if (_patients[i].Age < _patients[youngestIndex].Age)
            {
                youngestIndex = i;
            }

            if (_patients[i].Age > _patients[oldestIndex].Age)
            {
                oldestIndex = i;
            }

            if (_patients[i].IsAdult)
            {
                adultCount++;
            }
        }

        double averageAge = (double)totalAge / _count;

        Console.WriteLine($"Total:\t{_count}");
        Console.WriteLine($"Average Age:\t{averageAge:F1} years");
        Console.WriteLine($"Youngest:\t{_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} years)");
        Console.WriteLine($"Oldest:\t{_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} years)");
        Console.WriteLine($"Adults:\t{adultCount} of {_count}");
        Console.WriteLine("============================");
    }
}
