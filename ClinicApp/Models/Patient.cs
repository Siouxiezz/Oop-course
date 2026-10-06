using System;
using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;
public class Patient
{
    private string _phone = "";
    private DateTime _dateOfBirth;
    private string _lastName = "";
     private string _firstName = "";
    private static int _nextId = 1;
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
    public DateTime DateOfBirth 
    { 
        get => _dateOfBirth; 
        set
        {
            if (value > DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата народження не може бути в майбутньому.");

            if (value.Year < 1900)
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Рік народження не може бути раніше 1900.");

            _dateOfBirth = value;
        }
    }
    public BloodType BloodType 
    { 
        get; 
        set; 
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
    public string Email 
    { 
        get; 
        set; 
    }

    public string FullName
    {
        get
        {
            return $"{FirstName}, {LastName}";
        }
    }
    public int Age
    {
        get
        {
            int age = DateTime.Today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date.AddYears(age) > DateTime.Today)
            {
                age--;
            }

            return age;
        }
    }

    public bool IsAdult
    {
        get
        {
            return Age >= 18;   
        }
    }

    public Patient()
        : this("Unknown", "Patient", new DateTime(2000, 1, 1), BloodType.Unknown, "0000000000")
    {
    }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, new DateTime(2000, 1, 1), BloodType.Unknown, "0000000000")
    {
    }

    public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
        Email = string.Empty;
        Id = _nextId++;
    }

    public string GetAgeCategory()
    {
        if (Age < 18)
        {
            return "child";
        }

        if (Age < 60)
        {
            return "adult";
        }

        return "senior";
    }

    public override string ToString()
    {
        return $"[{Id}] {FullName} | Age: {ClinicFormatter.FormatAge(Age)} ({GetAgeCategory()}) | Blood Type: {ClinicFormatter.FormatBloodType(BloodType)} | Phone: {ClinicFormatter.FormatPhone(Phone)}";
    }
}
