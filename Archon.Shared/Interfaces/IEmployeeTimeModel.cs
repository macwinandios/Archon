using System;
using System.Collections.Generic;
using System.Text;

namespace Archon.Shared.Interfaces
{
    public interface IEmployeeTimeModel
    {
         DateTime CurrentTime { get; set; }
         DateTime ClockedInAt { get; set; }
         DateTime ClockedOutAt { get; set; }
         DayOfWeek DateClockedIn { get; set; }
         float HourlyWage { get; set; }
         float TotalWagesEarnedThisWeek { get; set; }
         DayOfWeek DateClockedOut { get; set; }
         TimeSpan DurationOfClockIn { get; set; }
         TimeSpan TotalTimeClockedInToday { get; set; }
         TimeSpan TotalTimeClockedInThisWeek { get; set; }
         string Username { get; set; } 
    }
}
