using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BusinessRegNo { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public DateTime? OnboardingMeetingDate { get; set; }
        public int? OnboardedBy { get; set; }
        public string? AgreementReference { get; set; }
        public CompanyStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<User> Users { get; set; } = new();
        public List<Batch> Batches { get; set; } = new();
        public List<Grant> Grants { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}
