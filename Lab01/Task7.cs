namespace Lab01;

public class Task7
{
    public static void Run()
    {

        int NOTFOUND = -1;

        Console.WriteLine("Enter number of your visits (>0):");
        int visits = int.Parse(Console.ReadLine()!);

        if(visits < 1)
        {
            Console.WriteLine("Invalid number of visits");
            return;
        }

        decimal[] prices = new decimal[visits];

        for (int i = 0; i < visits; i++)
        {
            Console.WriteLine("Enter price of each visit:");
            prices[i] = decimal.Parse(Console.ReadLine()!);
        }

        decimal average = 0;
        decimal min = prices[0];
        decimal max = prices[0];
        decimal sum = 0;

        foreach (var price in prices)
        {
            sum += price;
            if (price < min)
            {
                min = price;
            }
            if (price > max)
            {
                max = price;
            }
        }
        average = sum / prices.Length;

        int aboveAverageCount = 0;

        for(int i = 0; i < prices.Length; i++)
        {
            if(prices[i] > average)
            {
                aboveAverageCount++;
            }
        }

        int firstAbove1000 = NOTFOUND;
        int j = 0;

        while(j < prices.Length)
        {
            if(prices[j] > 1000)
            {
                firstAbove1000 = j;
                break;
            }
            j++;
        }

        Console.WriteLine($"======= Report on Admissions =======");
        Console.WriteLine($"Number of visits: {visits}");
        Console.WriteLine($"Total: {sum}");
        Console.WriteLine($"Average: {average:F2}");
        Console.WriteLine($"Min/max: {min}/{max}");
        Console.WriteLine($"Number of visits above average: {aboveAverageCount}/{visits}");
        if (firstAbove1000 == NOTFOUND)
        {
            Console.WriteLine("First > 1000: none");
        }
        else
        {
            Console.WriteLine($"First > 1000: #{firstAbove1000 + 1} — {prices[firstAbove1000]:F2}");
        }
        Console.WriteLine($"=====================================");

    }
}