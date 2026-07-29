using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.Services;

namespace TalentBridgeBackEnd.Controllers.Company
{
    [ApiController]
    [Route("api/v1/company")]
    [Authorize(Roles = "CompanyUser")]
    public class CompanyPortalController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly GrantResolverService _grantResolverService;
        private readonly PreviewProjectionService _previewProjectionService;
        private readonly FullProjectionService _fullProjectionService;
        private readonly OutcomeService _outcomeService;

        public CompanyPortalController(
            AppDbContext context,
            GrantResolverService grantResolverService,
            PreviewProjectionService previewProjectionService,
            FullProjectionService fullProjectionService,
            OutcomeService outcomeService)
        {
            _context = context;
            _grantResolverService = grantResolverService;
            _previewProjectionService = previewProjectionService;
            _fullProjectionService = fullProjectionService;
            _outcomeService = outcomeService;
        }

        private int GetCompanyId()
        {
            var companyIdClaim = User.FindFirst("companyId")?.Value;
            return int.TryParse(companyIdClaim, out var id) ? id : 0;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : 0;
        }

        [HttpGet("batches")]
        public async Task<IActionResult> GetCompanyBatches()
        {
            var companyId = GetCompanyId();
            var batches = await _context.Batches
                .Where(b => b.CompanyId == companyId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
            return Ok(batches);
        }

        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidates()
        {
            var companyId = GetCompanyId();
            var activeGrants = await _grantResolverService.ResolveGrantsForCompany(companyId);

            var result = new System.Collections.Generic.List<object>();
            foreach (var grant in activeGrants)
            {
                if (grant.Scope == GrantScope.Full)
                {
                    var full = await _fullProjectionService.GetFullProfile(grant.CandidateProfileId);
                    if (full != null) result.Add(new { Scope = "Full", Profile = full, grant.ValidUntil });
                }
                else
                {
                    var preview = await _previewProjectionService.GetPreviewProfile(grant.CandidateProfileId);
                    if (preview != null) result.Add(new { Scope = "Preview", Profile = preview, grant.ValidUntil });
                }
            }

            return Ok(result);
        }

        [HttpGet("candidates/{referenceCode}")]
        public async Task<IActionResult> GetCandidateProfile(string referenceCode)
        {
            var companyId = GetCompanyId();
            var candidate = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.ReferenceCode == referenceCode);
            if (candidate == null) return NotFound();

            var grant = await _grantResolverService.ResolveGrant(companyId, candidate.Id);
            if (grant == null) return Forbid("No active grant for this candidate");

            if (grant.Scope == GrantScope.Full)
            {
                var full = await _fullProjectionService.GetFullProfile(candidate.Id);
                return Ok(new { Scope = "Full", Profile = full, grant.ValidUntil });
            }
            else
            {
                var preview = await _previewProjectionService.GetPreviewProfile(candidate.Id);
                return Ok(new { Scope = "Preview", Profile = preview, grant.ValidUntil });
            }
        }

        public class AccessRequestCreateDto
        {
            public int CandidateProfileId { get; set; }
            public string? Notes { get; set; }
        }

        [HttpPost("access-requests")]
        public async Task<IActionResult> RequestFullAccess([FromBody] AccessRequestCreateDto request)
        {
            var companyId = GetCompanyId();
            var accessReq = new AccessRequest
            {
                CompanyId = companyId,
                CandidateProfileId = request.CandidateProfileId,
                Status = AccessRequestStatus.Requested,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.AccessRequests.Add(accessReq);
            await _context.SaveChangesAsync();

            return Ok(accessReq);
        }

        [HttpGet("access-requests")]
        public async Task<IActionResult> GetAccessRequests()
        {
            var companyId = GetCompanyId();
            var requests = await _context.AccessRequests
                .Where(r => r.CompanyId == companyId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return Ok(requests);
        }

        public class ReportOutcomeDto
        {
            public OutcomeValue OutcomeValue { get; set; }
            public string? Notes { get; set; }
        }

        [HttpPost("grants/{grantId}/outcome")]
        public async Task<IActionResult> ReportOutcome(int grantId, [FromBody] ReportOutcomeDto request)
        {
            var userId = GetUserId();
            await _outcomeService.ReportOutcome(grantId, request.OutcomeValue, userId, OutcomeSource.CompanyPortal, request.Notes);
            return Ok(new { message = "Outcome reported successfully" });
        }
    }
}
