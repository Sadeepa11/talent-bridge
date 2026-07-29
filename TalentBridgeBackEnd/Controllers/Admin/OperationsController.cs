using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Services;

namespace TalentBridgeBackEnd.Controllers.Admin
{
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize(Roles = "SuperAdmin,OpsAdmin")]
    public class OperationsController : ControllerBase
    {
        private readonly DashboardService _dashboardService;
        private readonly FollowUpService _followUpService;

        public OperationsController(
            DashboardService dashboardService,
            FollowUpService followUpService)
        {
            _dashboardService = dashboardService;
            _followUpService = followUpService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetAdminStats()
        {
            var stats = await _dashboardService.GetAdminStats();
            return Ok(stats);
        }

        [HttpGet("follow-ups")]
        public async Task<IActionResult> GetOpenFollowUps()
        {
            var followUps = await _followUpService.GetOpenFollowUps();
            return Ok(followUps);
        }

        public class FollowUpUpdateDto
        {
            public TalentBridgeBackEnd.Models.Enums.TaskStatus Status { get; set; }
            public string? Notes { get; set; }
        }

        [HttpPatch("follow-ups/{id}")]
        public async Task<IActionResult> UpdateFollowUp(int id, [FromBody] FollowUpUpdateDto request)
        {
            await _followUpService.UpdateFollowUp(id, request.Status, request.Notes);
            return Ok(new { message = "Follow-up task updated" });
        }
    }
}
