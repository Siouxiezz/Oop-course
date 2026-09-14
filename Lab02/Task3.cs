namespace Lab02;

public static class Task3
{
    public static void Run()
    {
        string[] days =
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };

        int[] patientsCount = new int[7];

        for (int i = 0; i < 7; i++)
        {
            Console.Write($"Enter number of patients for {days[i]}: ");
            patientsCount[i] = int.Parse(Console.ReadLine()!);
        }

        int total = 0;
        int maxIdx = 0;
        int minIdx = 0;

        for (int i = 0; i < patientsCount.Length; i++)
        {
            total += patientsCount[i];

            if (patientsCount[i] > patientsCount[maxIdx])
            {
                maxIdx = i;
            }

            if (patientsCount[i] < patientsCount[minIdx])
            {
                minIdx = i;
            }
        }

        Console.WriteLine();
        for (int i = 0; i < days.Length; i++)
        {
            Console.WriteLine($"{days[i],-12}: {patientsCount[i],2} patients");
        }

        Console.WriteLine($"Total:{total,10}");
        Console.WriteLine($"Maximum:{days[maxIdx],12} ({patientsCount[maxIdx]})");
        Console.WriteLine($"Minimum:{days[minIdx],11} ({patientsCount[minIdx]})");
    }
}