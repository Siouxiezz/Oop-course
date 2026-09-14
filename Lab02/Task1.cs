namespace Lab02;

public static class Task1
{
    public static void Run()
    {
        Console.WriteLine("Enter number of patients:");
        int n = int.Parse(Console.ReadLine()!);

        double[] weightOfPatients = new double[n];

        for(int i = 0; i < n; i++)
        {
            Console.WriteLine($"Enter weight of patient {i + 1}:");
            weightOfPatients[i] = double.Parse(Console.ReadLine()!);
        }

        double min = weightOfPatients[0];
        double max = weightOfPatients[0];
        double sum = 0;

        foreach(double weight in weightOfPatients)
        {
            if(weight < min)
            {
                min = weight;
            }
            if(weight > max)
            {
                max = weight;
            }
            sum += weight;
        }

        double average = sum / n;

        int countAboveAverage = 0;
        foreach(double weight in weightOfPatients)
        {
            if(weight > average)
            {
                countAboveAverage++;
            }
        }

        Console.WriteLine($"Quantity: {n} / Average: {average:F1} / Min: {min:F1} / Max: {max:F1} / Count above average: {countAboveAverage} out of {n}");

    }
}