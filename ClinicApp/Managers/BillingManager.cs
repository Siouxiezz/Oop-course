using System;
using ClinicApp.Interfaces;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments)
    {
        _appointments = appointments ?? throw new ArgumentNullException(nameof(appointments));
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointments.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(_appointments.GetByPatient(patientId));
    }

    public decimal GetTotalDebt()
    {
        return CalculateDebt(GetAllUnpaid());
    }

    public decimal GetPatientDebt(int patientId)
    {
        return CalculateDebt(GetUnpaidByPatient(patientId));
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] appointments = _appointments.GetAll();

        for (int i = 0; i < appointments.Length; i++)
        {
            Appointment appointment = appointments[i];
            if (appointment.Id == appointmentId)
            {
                if (appointment.IsPaid || appointment.IsCancelled)
                {
                    return false;
                }

                appointment.MarkPaid();
                return true;
            }
        }

        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] is Appointment appointment)
            {
                Console.WriteLine($"{appointment} | Сума: {items[i].GetCost().ToString("F2")} грн");
            }
            else
            {
                Console.WriteLine($"[{i + 1}] | Сума: {items[i].GetCost().ToString("F2")} грн");
            }
        }
    }

    private static IPayable[] FilterUnpaid(Appointment[] appointments)
    {
        int unpaidCount = 0;

        for (int i = 0; i < appointments.Length; i++)
        {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
            {
                unpaidCount++;
            }
        }

        IPayable[] unpaid = new IPayable[unpaidCount];
        int index = 0;

        for (int i = 0; i < appointments.Length; i++)
        {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
            {
                unpaid[index++] = appointments[i];
            }
        }

        return unpaid;
    }

    private static decimal CalculateDebt(IPayable[] items)
    {
        decimal total = 0m;

        for (int i = 0; i < items.Length; i++)
        {
            total += items[i].GetCost();
        }

        return total;
    }
}
