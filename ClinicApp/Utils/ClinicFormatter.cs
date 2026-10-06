using ClinicApp.Enums;

namespace ClinicApp.Utils;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bt)
    {
        return bt switch
        {
            BloodType.APositive => "A+",
            BloodType.ANegative => "A-",
            BloodType.BPositive => "B+",
            BloodType.BNegative => "B-",
            BloodType.ABPositive => "AB+",
            BloodType.ABNegative => "AB-",
            BloodType.OPositive => "O+",
            BloodType.ONegative => "O-",
            _ => "Unknown Blood Type"
        };
    }

    public static string FormatSpeciality(Speciality s)
    {
        return s switch
        {
            Speciality.Cardiology => "Кардіологія",
            Speciality.Neurology => "Неврологія",
            Speciality.Orthopedics => "Ортопедія",
            Speciality.Dermatology => "Дерматологія",
            Speciality.Emergency => "Невідкладна допомога",
            Speciality.General => "Загальна практика",
            Speciality.Pediatrics => "Педіатрія",
            Speciality.Surgery => "Хірургія",
            _ => "Невідома спеціальність"
        };
    }

    public static string FormatAge(int age)
    {
        int mod100 = age % 100;
        int mod10 = age % 10;

        if (mod100 >= 11 && mod100 <= 19)
        {
            return $"{age} років";
        }

        return mod10 switch
        {
            1 => $"{age} рік",
            2 or 3 or 4 => $"{age} роки",
            _ => $"{age} років"
        };
    }

    public static string FormatPhone(string phone)
    {
        if (phone.Length != 10)
        {
            return phone;
        }

        for (int i = 0; i < phone.Length; i++)
        {
            if (!char.IsDigit(phone[i]))
            {
                return phone;
            }
        }

        return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 4)}";
    }
}
