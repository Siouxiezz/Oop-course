using System;
using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;
    private string _firstName = "";
    private string _lastName = "";
    private string _licenceNumber = "";
    private string _phone = "";
    public int Id 
    { 
        get; 
    }
    public string FirstName 
    { 
        get => _firstName; 
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ім’я не може бути порожнім.", nameof(FirstName));

            if (value.Length > 50)
                throw new ArgumentException("Ім’я не може бути довшим за 50 символів.", nameof(FirstName));

            _firstName = value;
        }
    }
    public string LastName 
    { 
        get => _lastName; 
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Прізвище не може бути порожнім.", nameof(LastName));

            if (value.Length > 50)
                throw new ArgumentException("Прізвище не може бути довшим за 50 символів.", nameof(LastName));

            _lastName = value;
        }
    }
    public Speciality Speciality { get; set; }
    public string LicenseNumber 
    { 
        get => _licenceNumber; 
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Номер ліцензії не може бути порожнім.", nameof(LicenseNumber));

            _licenceNumber = value;
        }
    }
    public string Phone 
    { 
        get => _phone; 
        set
        {
            if (value == null || value.Length != 10)
                throw new ArgumentException("Телефон має містити рівно 10 цифр.", nameof(Phone));

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsDigit(value[i]))
                    throw new ArgumentException("Телефон має містити лише цифри.", nameof(Phone));
            }

            _phone = value;
        } 
    }
    public WorkSchedule Schedule { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int WorkingHoursPerDay => Schedule.HoursPerDay;

    public string WorkSchedule => Schedule.Display;
    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor()
        : this("Unknown", "Doctor", Speciality.General)
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
        : this(firstName, lastName, speciality, licenseNumber, phone, new WorkSchedule(8, 17))
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone, WorkSchedule schedule)
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = schedule;
        Id = _nextId++;
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "available" : "not available";
        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | {LicenseNumber} | Phone: {ClinicFormatter.FormatPhone(Phone)} | {WorkSchedule} ({WorkingHoursPerDay} hours) | {status}";
    }
}
