using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.DTOs.Admin;
using TalentBridgeBackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace TalentBridgeBackEnd.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDto orderDto, int adminUserId)
        {
            var orderCode = $"ORD-{DateTime.UtcNow.Year}-{await _context.Orders.CountAsync() + 1:D4}";

            var order = new Order
            {
                CompanyId = orderDto.CompanyId,
                OrderCode = orderCode,
                Subtotal = 0,
                Total = 0,
                Status = OrderStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> ConfirmPaymentAsync(int orderId, string paymentRef, PaymentMethod paymentMethod, DateTime paymentDate, int adminUserId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            if (order.Status == OrderStatus.PaymentReceived) throw new Exception("Order is already paid");

            order.Status = OrderStatus.PaymentReceived;
            order.PaymentReference = paymentRef;
            order.PaymentMethod = paymentMethod;
            order.PaymentDate = paymentDate;
            order.ConfirmedBy = adminUserId;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return order;
        }
    }
}
