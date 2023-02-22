using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Archon.Shared.Interfaces
{
    public interface IEmployeeTimeModel
    {
        int? Id { get; set; }
        DateTime CurrentTime { get; set; }
        DateTime ClockedInAt { get; set; }
        TimeSpan UpdatedClockInTime { get; set; }
        TimeSpan UpdatedClockOutTime { get; set; }

        DateTime ClockedOutAt { get; set; }
        DateTime DateClockedIn { get; set; }
        DateTime DateClockedOut { get; set; }
        float HourlyWage { get; set; }
        float TotalWagesEarnedThisWeek { get; set; }
        TimeSpan DurationOfClockIn { get; set; }
        TimeSpan TotalTimeClockedInToday { get; set; }
        TimeSpan TotalTimeClockedInThisWeek { get; set; }
        string Username { get; set; }
        ObservableCollection<IEmployeeTimeModel> HoursAndPayCollection { get; set; }
    }
}
