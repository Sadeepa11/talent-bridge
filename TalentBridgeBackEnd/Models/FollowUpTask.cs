using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class FollowUpTask
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public int AssignedTo { get; set; }
        public TaskType TaskType { get; set; }
        public DateTime DueDate { get; set; }
        public TalentBridgeBackEnd.Models.Enums.TaskStatus Status { get; set; }
        public string? ResolutionNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
