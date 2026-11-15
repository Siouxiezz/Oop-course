using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        ValidationResult result = new();

        foreach (PropertyInfo property in obj.GetType().GetProperties())
        {
            RequiredAttribute? required = property.GetCustomAttribute<RequiredAttribute>();
            MaxLengthAttribute? maxLength = property.GetCustomAttribute<MaxLengthAttribute>();
            MinValueAttribute? minValue = property.GetCustomAttribute<MinValueAttribute>();

            if (required is null && maxLength is null && minValue is null)
            {
                continue;
            }

            object? value = property.GetValue(obj);

            if (required is not null &&
                (value is null || value is string text && string.IsNullOrWhiteSpace(text)))
            {
                result.AddError($"{property.Name}: {required.ErrorMessage}");
            }

            if (maxLength is not null && value is string stringValue && stringValue.Length > maxLength.Length)
            {
                result.AddError($"{property.Name}: {maxLength.ErrorMessage}");
            }

            if (minValue is not null && value is not null && Convert.ToDouble(value) < minValue.Min)
            {
                result.AddError($"{property.Name}: {minValue.ErrorMessage}");
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        Console.WriteLine($"Type: {type.Name}");

        foreach (PropertyInfo property in type.GetProperties())
        {
            Console.WriteLine($"  {property.Name} ({property.PropertyType.Name})");

            foreach (Attribute attribute in property.GetCustomAttributes())
            {
                Console.WriteLine($"    [{attribute.GetType().Name}]");
            }
        }
    }
}
