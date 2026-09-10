namespace Lab01;

public class Task6
{
    public static void Run()
    {
        Console.WriteLine("Enter number of your card (5-6 digits):");
        int cardNumber = int.Parse(Console.ReadLine()!);

        int result = cardNumber % 10;

        switch (result)
        {
           case 0 or 1:
                Console.WriteLine($"Department: general therapy, preferential: {(cardNumber % 2 == 0 ? "yes" : "no")}, rewiev: {(cardNumber % 3 == 0 ? "yes" : "no")}");
                break;
            case 2 or 3:
                Console.WriteLine($"Department: surgery, preferential: {(cardNumber % 2 == 0 ? "yes" : "no")}, rewiev: {(cardNumber % 3 == 0 ? "yes" : "no")}");
                break;
            case 4 or 5:
                Console.WriteLine($"Department: cardiology, preferential: {(cardNumber % 2 == 0 ? "yes" : "no")}, rewiev: {(cardNumber % 3 == 0 ? "yes" : "no")}");
                break;
            case 6 or 7:
                Console.WriteLine($"Department: neurology, preferential: {(cardNumber % 2 == 0 ? "yes" : "no")}, rewiev: {(cardNumber % 3 == 0 ? "yes" : "no")}");
                break;
            case 8 or 9:
                Console.WriteLine($"Department: ophthalmology, preferential: {(cardNumber % 2 == 0 ? "yes" : "no")}, rewiev: {(cardNumber % 3 == 0 ? "yes" : "no")}");
                break;
        }
    }
}