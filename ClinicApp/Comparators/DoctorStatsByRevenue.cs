using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Comparators;

public class DoctorStatsByRevenue : IComparer<DoctorStats>
{
    public int Compare(DoctorStats? x, DoctorStats? y)
    {
        if (x is null)
        {
            return y is null ? 0 : -1;
        }

        if (y is null)
        {
            return 1;
        }

        return y.TotalRevenue.CompareTo(x.TotalRevenue);
    }
}
