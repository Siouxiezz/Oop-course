namespace Lab01;

public class Task3
{
    public static void Run()
    {
        Console.WriteLine("Enter your year of birth:");
        int yearOfBirth = int.Parse(Console.ReadLine()!);

        double age = 2026 - yearOfBirth;
        if(age < 18)
        {
            Console.WriteLine($"Your category is baby");
        }
        else if(age >= 18 && age < 60)
        {
            Console.WriteLine($"Your category is adult");
        }
        else
        {
            Console.WriteLine($"Your category is senior");
        }
    }
}