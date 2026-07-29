using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Services;

namespace TalentBridgeBackEnd.Controllers.Admin
{
    [ApiController]
    [Route("api/v1/admin/moderation")]
    [Authorize(Roles = "SuperAdmin,OpsAdmin")]
    public class ModerationController : ControllerBase
    {
        private readonly ModerationService _moderationService;
        private readonly FullProjectionService _fullProjectionService;
        private readonly PreviewProjectionService _previewProjectionService;

        public ModerationController(
            ModerationService moderationService,
            FullProjectionService fullProjectionService,
            PreviewProjectionService previewProjectionService)
        {
            _moderationService = moderationService;
            _fullProjectionService = fullProjectionService;
            _previewProjectionService = previewProjectionService;
        }

        private int GetAdminUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : 0;
        }

        [HttpGet("queue")]
        public async Task<IActionResult> GetQueue()
        {
            var queue = await _moderationService.GetModerationQueueAsync();
            return Ok(queue);
        }

        [HttpGet("candidates/{id}")]
        public async Task<IActionResult> GetCandidate(int id)
        {
            var profile = await _fullProjectionService.GetFullProfile(id);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpGet("candidates/{id}/preview")]
        public async Task<IActionResult> GetCandidatePreview(int id)
        {
            var preview = await _previewProjectionService.GetPreviewProfile(id);
            if (preview == null) return NotFound();
            return Ok(preview);
        }

        public class ModerationActionDto
        {
            public string Notes { get; set; } = string.Empty;
            public string Reason { get; set; } = string.Empty;
        }

        [HttpPost("candidates/{id}/approve")]
        public async Task<IActionResult> ApproveCandidate(int id, [FromBody] ModerationActionDto request)
        {
            try
            {
                var adminId = GetAdminUserId();
                await _moderationService.ApproveProfileAsync(id, adminId, request.Notes);
                return Ok(new { message = "Candidate approved successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("candidates/{id}/reject")]
        public async Task<IActionResult> RejectCandidate(int id, [FromBody] ModerationActionDto request)
        {
            try
            {
                var adminId = GetAdminUserId();
                await _moderationService.RejectProfileAsync(id, adminId, request.Reason);
                return Ok(new { message = "Candidate rejected" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("candidates/{id}/publish")]
        public async Task<IActionResult> PublishCandidate(int id)
        {
            try
            {
                await _moderationService.PublishProfileAsync(id);
                return Ok(new { message = "Candidate published successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
