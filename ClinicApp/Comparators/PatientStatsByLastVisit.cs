using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Comparators;

public class PatientStatsByLastVisit : IComparer<PatientStats>
{
    public int Compare(PatientStats? x, PatientStats? y)
    {
        if (x is null)
        {
            return y is null ? 0 : -1;
        }

        if (y is null)
        {
            return 1;
        }

        return y.LastVisitDate.CompareTo(x.LastVisitDate);
    }
}
