using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public void LogInfo(string message)
    {
        Write("INFO", message);
    }

    public void LogWarning(string message)
    {
        Write("WARN", message);
    }

    public void LogError(string message)
    {
        Write("ERROR", message);
    }

    public string[] GetLastLines(int n)
    {
        if (n <= 0 || !File.Exists(_logPath))
        {
            return Array.Empty<string>();
        }

        string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int skipCount = Math.Max(0, lines.Length - n);
        string[] result = new string[lines.Length - skipCount];
        Array.Copy(lines, skipCount, result, 0, result.Length);
        return result;
    }

    public void Clear()
    {
        using (StreamWriter writer = new(_logPath, false, Encoding.UTF8))
        {
        }
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }

    private void Write(string level, string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        string line = $"[{timestamp}] [{level.PadRight(5)}] {message}{Environment.NewLine}";
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }
}
