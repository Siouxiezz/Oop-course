using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new();
        PropertyInfo[] properties = typeof(T).GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo property = properties[i];
            if (property.GetSetMethod() is null)
            {
                continue;
            }

            RequiredAttribute? required = property.GetCustomAttribute<RequiredAttribute>();
            MaxLengthAttribute? maxLength = property.GetCustomAttribute<MaxLengthAttribute>();
            MinValueAttribute? minValue = property.GetCustomAttribute<MinValueAttribute>();

            while (true)
            {
                Console.Write($"{property.Name}{GetHint(required, maxLength, minValue)}: ");
                string? input = Console.ReadLine();
                if (input is null)
                {
                    throw new InvalidOperationException("Console input ended before the form was complete.");
                }

                object? value;
                try
                {
                    value = Convert.ChangeType(input, property.PropertyType);
                }
                catch (Exception exception) when (
                    exception is InvalidCastException
                    or FormatException
                    or OverflowException)
                {
                    Console.WriteLine("Некоректне значення, спробуйте ще раз.");
                    continue;
                }

                property.SetValue(obj, value);
                break;
            }
        }

        return obj;
    }

    private static string GetHint(
        RequiredAttribute? required,
        MaxLengthAttribute? maxLength,
        MinValueAttribute? minValue)
    {
        string hint = "";
        if (required is not null)
        {
            hint = "обов'язкове";
        }

        if (maxLength is not null)
        {
            if (hint.Length > 0)
            {
                hint += ", ";
            }

            hint += $"макс. {maxLength.Length}";
        }

        if (minValue is not null)
        {
            if (hint.Length > 0)
            {
                hint += ", ";
            }

            hint += $"мін. {minValue.Min}";
        }

        return hint.Length == 0 ? "" : $" ({hint})";
    }
}
