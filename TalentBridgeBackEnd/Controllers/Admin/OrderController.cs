using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TalentBridgeBackEnd.DTOs.Admin;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.Services;

namespace TalentBridgeBackEnd.Controllers.Admin
{
    [ApiController]
    [Route("api/v1/admin/orders")]
    [Authorize(Roles = "SuperAdmin,OpsAdmin")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetAdminUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<IActionResult> ListOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto request)
        {
            var adminId = GetAdminUserId();
            var order = await _orderService.CreateOrderAsync(request, adminId);
            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        public class OrderStatusUpdateDto
        {
            public OrderStatus Status { get; set; }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatusUpdateDto request)
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, request.Status);
            return Ok(order);
        }

        public class PaymentConfirmDto
        {
            public string PaymentReference { get; set; } = string.Empty;
            public PaymentMethod PaymentMethod { get; set; }
            public System.DateTime PaymentDate { get; set; }
        }

        [HttpPost("{id}/confirm-payment")]
        public async Task<IActionResult> RecordPayment(int id, [FromBody] PaymentConfirmDto request)
        {
            var adminId = GetAdminUserId();
            var order = await _orderService.ConfirmPaymentAsync(id, request.PaymentReference, request.PaymentMethod, request.PaymentDate, adminId);
            return Ok(order);
        }
    }
}
