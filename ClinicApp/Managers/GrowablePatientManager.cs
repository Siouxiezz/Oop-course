using System;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class GrowablePatientManager
{
    private Patient[] _patients;
    private int _count;

    public int Count => _count;

    public int Capacity => _patients.Length;

    public GrowablePatientManager()
    {
        _patients = new Patient[4];
    }

    private void Grow()
    {
        int oldCapacity = _patients.Length;
        int newCapacity = oldCapacity * 2;

        var newPatients = new Patient[newCapacity];

        for (int i = 0; i < _count; i++)
        {
            newPatients[i] = _patients[i];
        }

        _patients = newPatients;
        Console.WriteLine($"Array filled! Expansion: {oldCapacity} → {newCapacity}");
    }

    public void Add(Patient patient)
    {
        if (patient is null)
        {
            throw new ArgumentNullException(nameof(patient));
        }

        if (_count == _patients.Length)
        {
            Grow();
        }

        _patients[_count] = patient;
        _count++;

        Console.WriteLine($"Added [{patient.Id}]. Size: {_count} / {Capacity}");
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

    public Patient[] GetAll()
    {
        var patients = new Patient[_count];

        for (int i = 0; i < _count; i++)
        {
            patients[i] = _patients[i];
        }

        return patients;
    }

    public Patient[] FindByName(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Array.Empty<Patient>();
        }

        string normalizedQuery = query.Trim();
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            string fullName = $"{_patients[i].FirstName} {_patients[i].LastName}";
            if (_patients[i].FirstName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                || _patients[i].LastName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                || fullName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            {
                matches++;
            }
        }

        var results = new Patient[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            string fullName = $"{_patients[i].FirstName} {_patients[i].LastName}";
            if (_patients[i].FirstName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                || _patients[i].LastName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                || fullName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            {
                results[index++] = _patients[i];
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
        Console.WriteLine($"=== Patients ({_count} / {Capacity}) ===");

        if (_count == 0)
        {
            Console.WriteLine("No patients in the system.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }

        Console.WriteLine(new string('─', 72));
    }
}
