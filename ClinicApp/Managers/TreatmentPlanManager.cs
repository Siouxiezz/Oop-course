using ClinicApp.Enums;
using ClinicApp.Events;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp.Managers;

public class TreatmentPlanManager
{
    private readonly List<TreatmentPlan> _plans = new();

    public event EventHandler<TreatmentPlanEventArgs>? PlanCompleted;

    public bool Add(TreatmentPlan plan)
    {
        if (plan is null)
        {
            throw new ArgumentNullException(nameof(plan));
        }

        ValidationResult validationResult = ModelValidator.Validate(plan);
        if (!validationResult.IsValid)
        {
            validationResult.Print();
            return false;
        }

        _plans.Add(plan);
        return true;
    }

    public TreatmentPlan? GetById(int id)
    {
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].Id == id)
            {
                return _plans[i];
            }
        }

        return null;
    }

    public TreatmentPlan[] GetByPatient(int patientId)
    {
        List<TreatmentPlan> matchingPlans = new();
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].PatientId == patientId)
            {
                matchingPlans.Add(_plans[i]);
            }
        }

        return matchingPlans.ToArray();
    }

    public TreatmentPlan[] GetByStatus(TreatmentStatus status)
    {
        List<TreatmentPlan> matchingPlans = new();
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].Status == status)
            {
                matchingPlans.Add(_plans[i]);
            }
        }

        return matchingPlans.ToArray();
    }

    public TreatmentPlan[] GetAll()
    {
        return _plans.ToArray();
    }

    public bool Complete(int planId)
    {
        TreatmentPlan? plan = GetById(planId);
        if (plan is null)
        {
            return false;
        }

        bool changed = plan.Complete();
        if (changed)
        {
            PlanCompleted?.Invoke(this, new TreatmentPlanEventArgs(
                plan.Id,
                plan.PatientId,
                plan.Diagnosis));
        }

        return changed;
    }
}
