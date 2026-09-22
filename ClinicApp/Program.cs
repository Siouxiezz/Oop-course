namespace ClinicApp;

var patient1 = new Patient("Іван", "Петренко", new DateTime(1985, 4, 12), "A+", "0501234567");
var patient2 = new Patient("Олена", "Коваль", new DateTime(1993, 2, 8), "B-", "0672345678");
var patient3 = new Patient("Максим", "Бойко", new DateTime(2010, 5, 15), "O+", "0933456789");
var patient4 = new Patient();
var patient5 = new Patient("Марія", "Ткач");

Console.WriteLine(patient1);
Console.WriteLine(patient2);
Console.WriteLine(patient3);
Console.WriteLine(patient4);
Console.WriteLine(patient5);
