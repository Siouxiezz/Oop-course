namespace Lab02;

public static class Task2
{
    public static void Run()
    {
        Console.WriteLine("Enter number of visits:");
        int n = int.Parse(Console.ReadLine()!);

        if(n <= 0)
        {
            Console.WriteLine("Number of visits must be greater than 0.");
            return;
        }

        int[] costOfVisits = new int[n];

        for(int i = 0; i < n; i++)
        {
            Console.WriteLine($"Enter cost of visits (int):");
            costOfVisits[i] = int.Parse(Console.ReadLine()!);
        }

        int[] cloneCostOfVisits = new int[n];
        for(int i = 0; i < n; i++)
        {
            cloneCostOfVisits[i] = costOfVisits[i];
        }

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n - 1; j++)
            {
                if(cloneCostOfVisits[j] > cloneCostOfVisits[j + 1])
                {
                    int temp = cloneCostOfVisits[j];
                    cloneCostOfVisits[j] = cloneCostOfVisits[j + 1];
                    cloneCostOfVisits[j + 1] = temp;
                }
            }
        }

        Console.WriteLine("Queue (before):");
        foreach(int cost in costOfVisits)
        {
            Console.Write($"{cost} ");
        }
        Console.WriteLine();

        Console.WriteLine("Queue (after):");
        foreach(int cost in cloneCostOfVisits)
        {
            Console.Write($"{cost} ");
        }
        Console.WriteLine();

        Console.WriteLine($"Min: {cloneCostOfVisits[0]}");
        Console.WriteLine($"Max: {cloneCostOfVisits[n - 1]}");
    }
}