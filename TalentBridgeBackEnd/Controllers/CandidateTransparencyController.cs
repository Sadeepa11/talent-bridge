using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Services;

namespace TalentBridgeBackEnd.Controllers
{
    [ApiController]
    [Route("api/v1/candidate")]
    [Authorize(Roles = "Candidate")]
    public class CandidateTransparencyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AccessEventService _accessEventService;

        public CandidateTransparencyController(AppDbContext context, AccessEventService accessEventService)
        {
            _context = context;
            _accessEventService = accessEventService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : 0;
        }

        [HttpGet("access-log")]
        public async Task<IActionResult> GetAccessLog()
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound();

            var logs = await _accessEventService.GetEventsForCandidateAsync(profile.Id);
            return Ok(logs);
        }

        [HttpGet("consents")]
        public async Task<IActionResult> GetConsents()
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound();

            var consents = await _context.Consents
                .Where(c => c.CandidateProfileId == profile.Id)
                .OrderByDescending(c => c.GrantedAt)
                .ToListAsync();

            return Ok(consents);
        }

        public class ConsentCreateDto
        {
            public string TermsVersion { get; set; } = "v1.0";
            public string PrivacyVersion { get; set; } = "v1.0";
            public string ConsentScope { get; set; } = "Full";
        }

        [HttpPost("consents")]
        public async Task<IActionResult> RecordConsent([FromBody] ConsentCreateDto request)
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound();

            var consent = new Consent
            {
                CandidateProfileId = profile.Id,
                TermsVersion = request.TermsVersion,
                PrivacyVersion = request.PrivacyVersion,
                ConsentScope = request.ConsentScope,
                GrantedAt = DateTime.UtcNow,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                CreatedAt = DateTime.UtcNow
            };

            _context.Consents.Add(consent);
            await _context.SaveChangesAsync();

            return Ok(consent);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw()
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound();

            profile.Status = Models.Enums.CandidateStatus.Withdrawn;
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Candidate profile consent withdrawn. Status set to Withdrawn." });
        }

        [HttpPost("data-export")]
        public async Task<IActionResult> ExportData()
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles
                .Include(p => p.Experiences)
                .Include(p => p.Qualifications)
                .Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return NotFound();

            var pii = await _context.CandidatePiis.FirstOrDefaultAsync(p => p.CandidateProfileId == profile.Id);

            return Ok(new { Profile = profile, Pii = pii, ExportedAt = DateTime.UtcNow });
        }

        [HttpPost("deletion-request")]
        public async Task<IActionResult> RequestDeletion()
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound();

            var task = new FollowUpTask
            {
                AssignedTo = 1,
                TaskType = Models.Enums.TaskType.Verification,
                DueDate = DateTime.UtcNow.AddDays(7),
                Status = Models.Enums.TaskStatus.Open,
                ResolutionNotes = $"Deletion request from candidate {profile.ReferenceCode}",
                CreatedAt = DateTime.UtcNow
            };

            _context.FollowUpTasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deletion request submitted to administrators" });
        }
    }
}
