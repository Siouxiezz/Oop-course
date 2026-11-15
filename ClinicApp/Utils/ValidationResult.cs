using System.Collections.ObjectModel;

namespace ClinicApp.Utils;

public sealed class ValidationResult
{
    private readonly List<string> _errors = new();

    public bool IsValid => _errors.Count == 0;

    public IReadOnlyList<string> Errors { get; }

    public ValidationResult()
    {
        Errors = new ReadOnlyCollection<string>(_errors);
    }

    public void AddError(string error)
    {
        _errors.Add(error);
    }

    public void Print()
    {
        foreach (string error in _errors)
        {
            Console.WriteLine($"[!] {error}");
        }
    }
}
