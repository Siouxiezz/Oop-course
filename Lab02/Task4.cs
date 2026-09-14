namespace Lab02;

public static class Task4
{
    public static void Run()
    {
        Console.WriteLine("Enter number of doctors:");
        int n = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter number of days:");
        int m = int.Parse(Console.ReadLine()!);

        int[,] doctorMatrix = new int[n, m];

        for (int i = 0; i < n; i++)
        {
            Console.Write($"Enter number of visits for doctor {i + 1} (with spaces): ");
            string[] row = Console.ReadLine()!.Split();

            for (int j = 0; j < m; j++)
            {
                doctorMatrix[i, j] = int.Parse(row[j]);
            }
        }

        int[] doctorSums = new int[n];
        int[] daySums = new int[m];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                doctorSums[i] += doctorMatrix[i, j];
                daySums[j] += doctorMatrix[i, j];
            }
        }

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"Doctor {i + 1}: {doctorSums[i]} visits");
        }

        Console.Write("By days: ");
        Console.WriteLine(string.Join(", ", daySums));

        int maxValue = doctorMatrix[0, 0];
        int maxRow = 0;
        int maxCol = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (doctorMatrix[i, j] > maxValue)
                {
                    maxValue = doctorMatrix[i, j];
                    maxRow = i;
                    maxCol = j;
                }
            }
        }

        Console.WriteLine($"Maximum: {maxValue} (Doctor {maxRow + 1}, Day {maxCol + 1})");
    }
}