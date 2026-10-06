using System;
using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{

    private static readonly Regex PhoneRegex =
    new(@"^[0-9]{10}\z");

    private static readonly Regex InternationalPhoneRegex =
        new(@"^\+38[0-9]{10}\z");

    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z");
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} не може бути порожнім.", fieldName);

        if (value.Length > 50)
            throw new ArgumentException($"{fieldName} не може бути довшим за 50 символів.", fieldName);
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrEmpty(phone) ||
        (!PhoneRegex.IsMatch(phone) && !InternationalPhoneRegex.IsMatch(phone)))
        {
            throw new ArgumentException(
                "Телефон має містити 10 цифр або починатися з +38 і містити 10 цифр.",
                nameof(phone));
        }
    }

    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || !EmailRegex.IsMatch(email))
        {
            throw new ArgumentException("Некоректний email.", nameof(email));
        }
    }
    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today)
            throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути в майбутньому.");

        if (value.Year < 1900)
            throw new ArgumentOutOfRangeException(fieldName, "Рік не може бути раніше 1900.");
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(fieldName, "Значення має бути більшим за нуль.");
    }
}