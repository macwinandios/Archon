using System;
using System.Collections.Generic;
using System.Text;

namespace Archon.Shared.Interfaces
{
    public interface IAdminAssignTaskModel
    {
        int Id { get; set; }
        string Username { get; set; }
        int NumberOfAssignedTasks { get; set; }
        DateTime DateOfAssignedTask { get; set; }
        bool TaskIsComplete { get; set; }
        string TaskDescription { get; set; }
        string TaskTitle { get; set; }
        string TaskCompletedNotes { get; set; }
        string TaskWasAssignedTo { get; set; }
    }
}
