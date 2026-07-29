using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class MaskingRule
    {
        public int Id { get; set; }
        public MaskingRuleType RuleType { get; set; }
        public string? Pattern { get; set; }
        public ReplacementStrategy ReplacementStrategy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
