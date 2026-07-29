using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Batch
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FilterCriteriaJson { get; set; } = string.Empty;
        public DateTime DefaultValidFrom { get; set; }
        public DateTime DefaultValidUntil { get; set; }
        public int CreatedBy { get; set; }
        public BatchStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Company? Company { get; set; }
        public User? Creator { get; set; }
        public List<Grant> Grants { get; set; } = new();
    }
}
