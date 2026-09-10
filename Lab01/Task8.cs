namespace Lab01;

public static class Task8
{

    public static double CalculateBMI(double weight, double height)
    {
        double calculateBMI = weight / (height * height);
        return calculateBMI;
    }

    public static string GetBMICategory(double bmi)
    {
        if (bmi < 18.5)
        {
            return "Underweight";
        }
        else if (bmi < 25)
        {
            return "Normal weight";
        }
        else if (bmi < 30)
        {
            return "Overweight";
        }
        else
        {
            return "Obesity";
        }
    }

    public static double CalculateCost(double cost, int quantity, int discount)
    {
        double totalCost = cost * quantity * (1 - discount / 100.0);
        return totalCost;
    }
    
    public static string GetAgeCategory(int age)
    {
        if(age < 18)
        {
            return "baby";
        }
        else if(age >= 18 && age < 60)
        {
            return "adult";
        }
        else
        {
            return "senior";
        }
    }    
    
    public static string GetPressureStatus(int systolicBP, int diastolicBP)
    {
        if (systolicBP < 120 && diastolicBP < 80)
        {
            return "Normal";
        }
        else if (systolicBP < 130 && diastolicBP < 80)
        {
            return "Elevated";
        }
        else if (systolicBP < 140 || diastolicBP < 90)
        {
            return "Hypertension Stage 1";
        }
        else
        {
            return "Hypertension Stage 2";
        }
    }

    public static void Run()
    {
        Console.WriteLine("Enter weight in kilograms:");
        double weight = double.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter height in meters and centimeters (for example 1.75):");
        double height = double.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter cost in hrn:");
        double cost = double.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter quantity of visits:");
        int quantity = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter discount in %:");
        int discount = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter your year of birth:");
        int yearOfBirth = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter your systolic blood pressure:");
        int systolicBP = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter your diastolic blood pressure:");
        int diastolicBP = int.Parse(Console.ReadLine()!);

        Console.WriteLine("======= Results =======");
        double bmi = CalculateBMI(weight, height);
        string bmiCategory = GetBMICategory(bmi);
        Console.WriteLine($"Your BMI is: {bmi:F2} - {bmiCategory}");

        double totalCost = CalculateCost(cost, quantity, discount);
        Console.WriteLine($"Sum: {totalCost:F2} hrn");

        int age = 2026 - yearOfBirth;
        string ageCategory = GetAgeCategory(age);
        Console.WriteLine($"Age: {age} y., category: {ageCategory}");

        string pressureStatus = GetPressureStatus(systolicBP, diastolicBP);
        Console.WriteLine($"Pressure: {systolicBP}/{diastolicBP} - {pressureStatus}");
        Console.WriteLine("=======================");
    }
}