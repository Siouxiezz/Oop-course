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
        set => _firstName = value;
    }
    public string LastName 
    { 
        get => _lastName;
        set => _lastName = value;
    }
    public DateTime DateOfBirth 
    { 
        get => _dateOfBirth; 
        set => _dateOfBirth = value;
    }
    public BloodType BloodType 
    { 
        get; 
        set; 
    }
    public string Phone 
    { 
        get => _phone; 
        set => _phone = value;
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
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
        Email = string.Empty;
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
