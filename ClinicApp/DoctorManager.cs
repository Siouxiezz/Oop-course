using System;

namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private readonly Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count;

    public int Count => _count;

    public void Add(Doctor doctor)
    {
        if (doctor is null)
        {
            throw new ArgumentNullException(nameof(doctor));
        }

        if (_count >= MaxDoctors)
        {
            Console.WriteLine($"Can't add doctor. Limit reached ({MaxDoctors}).");
            return;
        }

        _doctors[_count] = doctor;
        _count++;
        Console.WriteLine($"Doctor [{doctor.Id}] {doctor.FullName} added.");
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }

        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        if (string.IsNullOrWhiteSpace(speciality))
        {
            return Array.Empty<Doctor>();
        }

        string normalizedSpeciality = speciality.Trim();
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (string.Equals(_doctors[i].Speciality, normalizedSpeciality, StringComparison.OrdinalIgnoreCase))
            {
                matches++;
            }
        }

        var results = new Doctor[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (string.Equals(_doctors[i].Speciality, normalizedSpeciality, StringComparison.OrdinalIgnoreCase))
            {
                results[index++] = _doctors[i];
            }
        }

        return results;
    }

    public Doctor[] GetAll()
    {
        var doctors = new Doctor[_count];

        for (int i = 0; i < _count; i++)
        {
            doctors[i] = _doctors[i];
        }

        return doctors;
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                for (int j = i; j < _count - 1; j++)
                {
                    _doctors[j] = _doctors[j + 1];
                }

                _doctors[_count - 1] = null!;
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
            Console.WriteLine($"=== Doctors (0 / {MaxDoctors}) ===");
            Console.WriteLine("List of doctors is empty.");
            return;
        }

        Console.WriteLine($"=== Doctors ({_count} / {MaxDoctors}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }

        Console.WriteLine(new string('─', 72));
    }

    public void DisplayStats()
    {
        Console.WriteLine("=== Doctor Statistics ===");

        if (_count == 0)
        {
            Console.WriteLine("Total:\t0");
            Console.WriteLine("Available now:\t0");
            Console.WriteLine("By Specialties:");
            Console.WriteLine("==========================");
            return;
        }

        int availableNow = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
            {
                availableNow++;
            }
        }

        Console.WriteLine($"Total:\t{_count}");
        Console.WriteLine($"Available now:\t{availableNow}");
        Console.WriteLine("By Specialties:");

        for (int i = 0; i < _count; i++)
        {
            bool alreadySeen = false;

            for (int j = 0; j < i; j++)
            {
                if (string.Equals(_doctors[i].Speciality, _doctors[j].Speciality, StringComparison.OrdinalIgnoreCase))
                {
                    alreadySeen = true;
                    break;
                }
            }

            if (alreadySeen)
            {
                continue;
            }

            string speciality = _doctors[i].Speciality;
            int count = 0;

            for (int j = 0; j < _count; j++)
            {
                if (string.Equals(_doctors[j].Speciality, speciality, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            Console.WriteLine($"  {speciality}: {count}");
        }

        Console.WriteLine("==========================");
    }
}
