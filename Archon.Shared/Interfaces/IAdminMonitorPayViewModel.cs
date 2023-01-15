using System;
using System.Collections.Generic;
using System.Text;

namespace Archon.Shared.Interfaces
{
    public interface IAdminMonitorPayViewModel
    {
        DateTime CurrentTime { get; set; }
        DateTime ClockedInAt { get; set; }
        DateTime ClockedOutAt { get; set; }
        DateTime DateClockedIn { get; set; }
        DateTime DateClockedOut { get; set; }
        float HourlyWage { get; set; }
        float WagesEarned { get; set; }
        TimeSpan DurationOfClockIn { get; set; }
        TimeSpan TotalTimeClockedInToday { get; set; }
        TimeSpan TotalTimeClockedInThisWeek { get; set; }
        string Username { get; set; }

    }


}
