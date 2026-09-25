using System;
using ClinicApp;

var doctor1 = new Doctor("Oleg", "Sidorenko", "Cardiology", "LIC-001", "0441234567")
{
    WorkStartHour = 8,
    WorkEndHour = 16
};

var doctor2 = new Doctor("Natalia", "Moroz", "Neurology", "LIC-002", "0442345678")
{
    WorkStartHour = 9,
    WorkEndHour = 18
};

var doctor3 = new Doctor("Andriy", "Vlasenko", "Pediatrics", "LIC-003", "0443456789");

var doctor4 = new Doctor("Maria", "Boiko", "Therapy", "LIC-004", "0444567890")
{
    WorkStartHour = 10,
    WorkEndHour = 14
};

Console.WriteLine(doctor1);
Console.WriteLine(doctor2);
Console.WriteLine(doctor3);
Console.WriteLine(doctor4);
