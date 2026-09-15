namespace Lab02;

public static class Task7
{
    public static void Run()
    {
        Console.WriteLine("Enter the number of patients:");
        int n = int.Parse(Console.ReadLine()!);

        if (n <= 0)
        {
            Console.WriteLine("Invalid number of patients.");
            return;
        }

        string[] names = new string[n];
        double[] bmis = new double[n];

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"Enter the name for patient {i + 1}:");
            names[i] = Console.ReadLine()!;

            Console.WriteLine($"Enter the BMI for patient {i + 1}:");
            bmis[i] = double.Parse(Console.ReadLine()!);
        }

        for(int i = 0; i < n; i++)
        {
            for(int j = i + 1; j < n - 1 - i; j++)
            {
                if (bmis[j] < bmis[j + 1])
                {
                    double tempBmi = bmis[j];
                    bmis[j] = bmis[j + 1];
                    bmis[j + 1] = tempBmi;

                    string tempName = names[j];
                    names[j] = names[j + 1];
                    names[j + 1] = tempName;
                }
            }
        }

        Console.WriteLine("=== Rating IMT ===:");
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"{i + 1}. {names[i]}: {bmis[i]:F2}");
        }
    }
}