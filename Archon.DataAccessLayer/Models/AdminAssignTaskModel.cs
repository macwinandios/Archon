using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archon.DataAccessLayer.Models
{
    public class AdminAssignTaskModel : IAdminAssignTaskModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int? NumberOfAssignedTasks { get; set; }
        public DateTime DateOfAssignedTask { get; set; }
        public bool TaskIsComplete { get; set; }
        public string TaskDescription { get; set; }
        public string TaskTitle { get; set; }
        public string TaskCompletedNotes { get; set; }
        public string TaskWasAssignedTo { get; set; }


    }
}
