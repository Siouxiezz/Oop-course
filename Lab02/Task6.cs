namespace Lab02;

public static class Task6
{
    public static void Run()
    {
        Console.WriteLine("Enter the number of doctors:");
        int n = int.Parse(Console.ReadLine()!);

        if (n <= 0)
        {
            Console.WriteLine("Invalid number of doctors.");
            return;
        }

        int[][] costs = new int[n][];
        int largestIncome = 0;
        int largestIncomeDoctor = 0;

        for (int doctorIndex = 0; doctorIndex < n; doctorIndex++)
        {
            Console.WriteLine($"Enter the number of appointments for doctor {doctorIndex + 1}:");
            int appointmentsCount = int.Parse(Console.ReadLine()!);
            costs[doctorIndex] = new int[appointmentsCount];

            int income = 0;
            for (int appointmentIndex = 0; appointmentIndex < appointmentsCount; appointmentIndex++)
            {
                Console.WriteLine($"Enter the cost for appointment {appointmentIndex + 1} for doctor {doctorIndex + 1}:");
                costs[doctorIndex][appointmentIndex] = int.Parse(Console.ReadLine()!);
                income += costs[doctorIndex][appointmentIndex];
            }

            if (income > largestIncome)
            {
                largestIncome = income;
                largestIncomeDoctor = doctorIndex + 1;
            }
        }

        for (int doctorIndex = 0; doctorIndex < n; doctorIndex++)
        {
            int appointmentsCount = costs[doctorIndex].Length;
            int income = costs[doctorIndex].Sum();
            double average = appointmentsCount == 0 ? 0 : (double)income / appointmentsCount;

            Console.WriteLine($"Doctor {doctorIndex + 1}: {appointmentsCount} appointments, total={income} hrn, average={average:F2} hrn");
        }
        Console.WriteLine($"Largest income: Doctor {largestIncomeDoctor} ({largestIncome} hrn)");
    }
}