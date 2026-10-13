using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;
    private readonly MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count;

    public int Count => _count;

    public MedicalRecord? this[int index] =>
        index >= 0 && index < _count ? _records[index] : null;

    public void Add(MedicalRecord record)
    {
        if (record is null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        if (_count >= MaxRecords)
        {
            Console.WriteLine($"Не вдалося додати запис: досягнуто ліміт ({MaxRecords}).");
            return;
        }

        _records[_count] = record;
        _count++;

        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
            {
                return _records[i];
            }
        }

        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                matches++;
            }
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                result[index] = _records[i];
                index++;
            }
        }

        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                matches++;
            }
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                result[index] = _records[i];
                index++;
            }
        }

        return result;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список медичних записів порожній.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i]);
        }
    }

    public void DisplayList(MedicalRecord[] records)
    {
        if (records is null)
        {
            throw new ArgumentNullException(nameof(records));
        }

        if (records.Length == 0)
        {
            Console.WriteLine("Список медичних записів порожній.");
            return;
        }

        for (int i = 0; i < records.Length; i++)
        {
            Console.WriteLine(records[i]);
        }
    }
}