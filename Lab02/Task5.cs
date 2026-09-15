namespace Lab02;

public static class Task5
{
    public static void Run()
    {
        Console.WriteLine("Enter number for your matrix:");
        int n = int.Parse(Console.ReadLine()!);

        if(n <= 0)
        {
            Console.WriteLine("Matrix size must be a positive integer.");
            return;
        }

        int[,] matrix = new int[n, n];
        int[] mainDiagonalArray = new int[n];
        int[] secondaryDiagonalArray = new int[n];

        Console.WriteLine("Enter matrix values row by row (with spaces):");
        for (int i = 0; i < n; i++)
        {
            string[] row = Console.ReadLine()!.Split();
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = int.Parse(row[j]);
            }
        }

        int sumMainDiagonal = 0;
        int sumSecondaryDiagonal = 0;

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                if (i == j)
                {
                    mainDiagonalArray[i] = matrix[i, j];
                    sumMainDiagonal += matrix[i, j];
                }
                if (i + j == n - 1)
                {
                    secondaryDiagonalArray[i] = matrix[i, j];
                    sumSecondaryDiagonal += matrix[i, j];
                }
            }
        }

        Console.WriteLine($"Main diagonal: {string.Join(", ", mainDiagonalArray)} (sum = {sumMainDiagonal})");
        Console.WriteLine($"Secondary diagonal: {string.Join(", ", secondaryDiagonalArray)} (sum = {sumSecondaryDiagonal})");
    }
}