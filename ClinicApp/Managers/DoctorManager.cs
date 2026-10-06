using System;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private readonly Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public Doctor? this[int index] => index >= 0 && index < _count ? _doctors[index] : null;

    public void Add(Doctor doctor)
    {
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

    public bool TryFindById(int id, out Doctor doctor)
    {
        Doctor? foundDoctor = FindById(id);
        if (foundDoctor is null)
        {
            doctor = null!;
            return false;
        }

        doctor = foundDoctor;
        return true;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        if (string.IsNullOrWhiteSpace(speciality))
        {
            return Array.Empty<Doctor>();
        }

        string query = speciality.Trim();

        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                matches++;
            }
        }

        Doctor[] result = new Doctor[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToString().Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                result[index] = _doctors[i];
                index++;
            }
        }
        return result;
    }

    public Doctor[] FindBySpeciality(Speciality speciality)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
            {
                matches++;
            }
        }

        Doctor[] result = new Doctor[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
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
                if (_doctors[i].Speciality == _doctors[j].Speciality)
                {
                    alreadySeen = true;
                    break;
                }
            }

            if (alreadySeen)
            {
                continue;
            }

            Speciality speciality = _doctors[i].Speciality;
            int count = 0;

            for (int j = 0; j < _count; j++)
            {
                if (_doctors[j].Speciality == speciality)
                {
                    count++;
                }
            }

            Console.WriteLine($"  {speciality}: {count}");
        }

        Console.WriteLine("==========================");
    }
}
