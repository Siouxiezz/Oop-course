namespace Lab01;

public class Task1
{
    public static void Run()
    {
        Console.WriteLine("Enter weight in kilograms:");
        double weight = double.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter height in meters and centimeters (for example 1,75):");
        double height = double.Parse(Console.ReadLine()!);

        double calculateBMI = weight / (height * height);
        Console.WriteLine($"Your BMI is: {calculateBMI:F2}");

    }
}