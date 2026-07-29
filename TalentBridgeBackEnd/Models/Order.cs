using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string? PaymentReference { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? ConfirmedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Company? Company { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public List<Grant> Grants { get; set; } = new();
        public User? Confirmer { get; set; }
    }
}
