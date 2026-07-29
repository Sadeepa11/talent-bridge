using System;

namespace TalentBridgeBackEnd.Models
{
    public class CompanyNote
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CandidateProfileId { get; set; }
        public string NoteText { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
