using System;

namespace TalentBridgeBackEnd.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int ActorUserId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? BeforeJson { get; set; }
        public string? AfterJson { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }
}
