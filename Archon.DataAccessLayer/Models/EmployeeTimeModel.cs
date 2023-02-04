using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Archon.DataAccessLayer.Models
{
    public class EmployeeTimeModel : IEmployeeTimeModel
    {
        public int Id { get; set; }
        public DateTime CurrentTime { get; set; }
        public DateTime ClockedInAt { get; set; }
        public DateTime ClockedOutAt { get; set; }
        public DateTime DateClockedIn { get; set; }
        public float HourlyWage { get; set; }
        public float TotalWagesEarnedThisWeek { get; set; }
        public DateTime DateClockedOut { get; set; }
        public TimeSpan DurationOfClockIn { get; set; }
        public TimeSpan TotalTimeClockedInToday { get; set; }
        public TimeSpan TotalTimeClockedInThisWeek { get; set; }
        public string Username { get; set; }

    }
}
