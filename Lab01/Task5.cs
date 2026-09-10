namespace Lab01;

public class Task5
{
    public static void Run()
    {
        Console.WriteLine("Enter day of the week (1-7):");
        int dayOfWeek = int.Parse(Console.ReadLine()!);

        switch (dayOfWeek)
        {
            case 1:
                Console.WriteLine("Day: Monday, 08:00 - 18:00");
                break;
            case 2:
                Console.WriteLine("Day: Tuesday, 08:00 - 18:00");
                break;
            case 3:
                Console.WriteLine("Day: Wednesday, 09:00 - 17:00");
                break;
            case 4:
                Console.WriteLine("Day: Thursday, 08:00 - 18:00");
                break;
            case 5:
                Console.WriteLine("Day: Friday, 08:00 - 16:00");
                break;
            case 6:
                Console.WriteLine("Day: Saturday, 09:00 - 14:00");
                break;
            case 7:
                Console.WriteLine("Day: Sunday, day off");
                break;
            default:
                Console.WriteLine("Invalid day of the week");
                break;
        }
    }
}