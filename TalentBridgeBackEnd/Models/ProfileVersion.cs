using System;

namespace TalentBridgeBackEnd.Models
{
    public class ProfileVersion
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public int VersionNumber { get; set; }
        public string SnapshotJson { get; set; } = string.Empty;
        public int ApprovedBy { get; set; }
        public DateTime ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
