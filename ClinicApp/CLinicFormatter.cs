namespace ClinicApp;

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
            Speciality.Cardiology => "Cardiology",
            Speciality.Neurology => "Neurology",
            Speciality.Orthopedics => "Orthopedics",
            Speciality.Dermatology => "Dermatology",
            Speciality.Emergency => "Emergency",
            Speciality.General => "General Practice ",
            Speciality.Pediatrics => "Pediatrics",
            Speciality.Surgery => "Surgery",
            _ => "Unknown Speciality"
        };
    }

    public static string FormatAge(int age)
    {
        int mod100 = age % 100;
        int mod10 = age % 10;

        if (mod100 >= 11 && mod100 <= 19)
        {
            return $"{age} years";
        }

        return mod10 switch
        {
            1 => $"{age} year",
            2 or 3 or 4 => $"{age} years",
            _ => $"{age} years"
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