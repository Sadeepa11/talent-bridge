using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TalentBridgeBackEnd.DTOs.Admin;
using TalentBridgeBackEnd.Services;

namespace TalentBridgeBackEnd.Controllers.Admin
{
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize(Roles = "SuperAdmin,OpsAdmin")]
    public class CurationController : ControllerBase
    {
        private readonly BatchCurationService _batchCurationService;
        private readonly PreviewProjectionService _previewProjectionService;

        public CurationController(BatchCurationService batchCurationService, PreviewProjectionService previewProjectionService)
        {
            _batchCurationService = batchCurationService;
            _previewProjectionService = previewProjectionService;
        }

        private int GetAdminUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : 0;
        }

        [HttpGet("candidates/search")]
        public async Task<IActionResult> SearchAvailableCandidates([FromQuery] string? category, [FromQuery] string? experienceBand, [FromQuery] string? city, [FromQuery] string? availability)
        {
            var results = await _previewProjectionService.SearchPublishedCandidates(category, experienceBand, city, availability);
            return Ok(results);
        }

        [HttpPost("batches")]
        public async Task<IActionResult> CreateAndCommitBatch([FromBody] BatchCreateDto request)
        {
            try
            {
                var adminId = GetAdminUserId();
                var batch = await _batchCurationService.CreateAndCommitBatchAsync(request, adminId);
                return Ok(batch);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
