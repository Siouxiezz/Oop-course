using System;
using System.Collections.Generic;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    private readonly List<Patient> _patients = new();

    public int Count => _patients.Count;

    public Patient? this[int index] => index >= 0 && index < _patients.Count ? _patients[index] : null;

    public void Add(Patient patient)
    {
        if (patient is null)
        {
            throw new ArgumentNullException(nameof(patient));
        }

        _patients.Add(patient);
        Console.WriteLine($"Patient [{patient.Id}] {patient.FullName} added.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }

    public bool TryFindById(int id, out Patient patient)
    {
        Patient? foundPatient = FindById(id);
        if (foundPatient is null)
        {
            patient = null!;
            return false;
        }

        patient = foundPatient;
        return true;
    }

    public Patient[] FindByName(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Array.Empty<Patient>();
        }

        string searchText = query.Trim();
        string normalizedQuery = searchText.ToLower();

        var results = new List<Patient>();
        for (int i = 0; i < _patients.Count; i++)
        {
            var patient = _patients[i];
            string fullName = $"{patient.FirstName} {patient.LastName}".ToLower();

            if (patient.FirstName.ToLower().Contains(normalizedQuery)
                || patient.LastName.ToLower().Contains(normalizedQuery)
                || fullName.Contains(normalizedQuery))
            {
                results.Add(patient);
            }
        }

        return results.ToArray();
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        var results = new List<Patient>();
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                results.Add(_patients[i]);
            }
        }

        return results.ToArray();
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                _patients.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("=== Patients (0) ===");
            Console.WriteLine("Patient list is empty.");
            return;
        }

        Console.WriteLine($"=== Patients ({_patients.Count}) ===");
        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }

        Console.WriteLine(new string('─', 72));
    }

    public void DisplayStats()
    {
        Console.WriteLine("=== Patient Statistics ===");

        if (_patients.Count == 0)
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

        for (int i = 0; i < _patients.Count; i++)
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

        double averageAge = (double)totalAge / _patients.Count;

        Console.WriteLine($"Total:\t{_patients.Count}");
        Console.WriteLine($"Average Age:\t{averageAge:F1} years");
        Console.WriteLine($"Youngest:\t{_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} years)");
        Console.WriteLine($"Oldest:\t{_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} years)");
        Console.WriteLine($"Adults:\t{adultCount} of {_patients.Count}");
        Console.WriteLine("============================");
    }
}
