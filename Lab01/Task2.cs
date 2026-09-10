namespace Lab01;

public static class Task2
{
    public static void Run()
    {
        Console.WriteLine("Enter cost in hrn:");
        double cost = double.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter quantity of visits:");
        int quantity = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter discount in %:");
        int discount = int.Parse(Console.ReadLine()!);

        double totalCost = cost * quantity * (1 - discount / 100.0);
        Console.WriteLine($"Your total cost is: {totalCost:F2}");

    }
}