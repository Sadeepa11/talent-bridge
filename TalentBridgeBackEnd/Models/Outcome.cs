using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Outcome
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public OutcomeValue OutcomeValue { get; set; }
        public int ReportedBy { get; set; }
        public OutcomeSource ReportedVia { get; set; }
        public string? ContactMethod { get; set; }
        public string? Notes { get; set; }
        public bool ConfirmedByAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public Grant? Grant { get; set; }
    }
}
