namespace Lab02;

public static class Task8
{
    public static void Run()
    {
        Console.WriteLine("Enter the number of departments:");
        int departmentsCount = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter the number of weeks:");
        int weeksCount = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter the number of patients for each department, week, and shift (morning and evening):");
        int[,,] patients = new int[departmentsCount, weeksCount, 2];
        int[] departmentTotals = new int[departmentsCount];

        for (int departmentIndex = 0; departmentIndex < departmentsCount; departmentIndex++)
        {
            for (int weekIndex = 0; weekIndex < weeksCount; weekIndex++)
            {
                for (int shiftIndex = 0; shiftIndex < 2; shiftIndex++)
                {
                    Console.WriteLine($"Department {departmentIndex + 1}, Week {weekIndex + 1}, Shift {(shiftIndex == 0 ? "Morning" : "Evening")}:");
                    patients[departmentIndex, weekIndex, shiftIndex] = int.Parse(Console.ReadLine()!);
                    departmentTotals[departmentIndex] += patients[departmentIndex, weekIndex, shiftIndex];
                }
            }
        }

        int busiestDepartmentIndex = 0;
        for (int departmentIndex = 1; departmentIndex < departmentsCount; departmentIndex++)
        {
            if (departmentTotals[departmentIndex] > departmentTotals[busiestDepartmentIndex])
            {
                busiestDepartmentIndex = departmentIndex;
            }
        }

        for (int departmentIndex = 0; departmentIndex < departmentsCount; departmentIndex++)
        {
            Console.WriteLine($"Department {departmentIndex + 1}:");
            for (int weekIndex = 0; weekIndex < weeksCount; weekIndex++)
            {
                int morningPatients = patients[departmentIndex, weekIndex, 0];
                int eveningPatients = patients[departmentIndex, weekIndex, 1];
                Console.WriteLine($"  Week {weekIndex + 1}: Morning {morningPatients}, Evening {eveningPatients} → Total {morningPatients + eveningPatients}");
            }

            Console.WriteLine($"  Total: {departmentTotals[departmentIndex]} patients");
        }

        Console.WriteLine($"Busiest Department: Department {busiestDepartmentIndex + 1} ({departmentTotals[busiestDepartmentIndex]} patients)");
    }
}