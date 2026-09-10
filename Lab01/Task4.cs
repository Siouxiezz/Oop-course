namespace Lab01;

public class Task4
{
    public static void Run()
    {
        Console.WriteLine("Enter your systolic blood pressure:");
        int systolicBP = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter your diastolic blood pressure:");
        int diastolicBP = int.Parse(Console.ReadLine()!);

        if(systolicBP < 120 && diastolicBP < 80)
        {
            Console.WriteLine($"Your blood pressure: {systolicBP}/{diastolicBP} - Normal");
        }
        else if(systolicBP < 130 && diastolicBP < 80)
        {
            Console.WriteLine($"Your blood pressure: {systolicBP}/{diastolicBP} - Elevated");
        }
        else if(systolicBP < 140 || diastolicBP < 90)
        {
            Console.WriteLine($"Your blood pressure: {systolicBP}/{diastolicBP} - Hypertension Stage 1");
        }
        else
        {
            Console.WriteLine($"Your blood pressure: {systolicBP}/{diastolicBP} - Hypertensive Stage 2");
        }
    }
}