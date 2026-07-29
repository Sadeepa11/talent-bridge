using System;

namespace TalentBridgeBackEnd.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CandidateProfileId { get; set; }
        public decimal UnitPrice { get; set; }
        public int AccessDays { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
