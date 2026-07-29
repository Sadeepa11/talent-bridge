using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.DTOs.Candidate;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.Services;

namespace TalentBridgeBackEnd.Controllers
{
    [ApiController]
    [Route("api/v1/candidate")]
    [Authorize(Roles = "Candidate")]
    public class CandidateProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly FullProjectionService _fullProjectionService;

        public CandidateProfileController(AppDbContext context, FullProjectionService fullProjectionService)
        {
            _context = context;
            _fullProjectionService = fullProjectionService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            return int.TryParse(userIdClaim, out var id) ? id : 0;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound(new { message = "Profile not found" });

            var fullProfile = await _fullProjectionService.GetFullProfile(profile.Id);
            return Ok(fullProfile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto request)
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound(new { message = "Profile not found" });

            if (request.JobCategoryId.HasValue) profile.JobCategoryId = request.JobCategoryId.Value;
            if (!string.IsNullOrEmpty(request.PositionSought)) profile.PositionSought = request.PositionSought;
            profile.YearsExperience = request.YearsExperience;
            profile.ExperienceBand = request.ExperienceBand;
            if (!string.IsNullOrEmpty(request.HighestQualification)) profile.HighestQualification = request.HighestQualification;
            if (!string.IsNullOrEmpty(request.MainCity)) profile.MainCity = request.MainCity;
            profile.Availability = request.Availability;
            profile.ExpectedSalaryMin = request.ExpectedSalaryMin;
            profile.ExpectedSalaryMax = request.ExpectedSalaryMax;
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Profile updated successfully", profile.Id });
        }

        [HttpPost("profile/submit")]
        public async Task<IActionResult> SubmitProfile()
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound(new { message = "Profile not found" });

            profile.Status = CandidateStatus.Submitted;
            profile.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile submitted for moderation", profile.Id, status = profile.Status.ToString() });
        }

        [HttpPost("experiences")]
        public async Task<IActionResult> AddExperience([FromBody] ExperienceDto dto)
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound(new { message = "Profile not found" });

            var exp = new CandidateExperience
            {
                CandidateProfileId = profile.Id,
                EmployerName = dto.EmployerName,
                EmployerDescriptor = dto.EmployerDescriptor,
                JobTitle = dto.JobTitle,
                Industry = dto.Industry,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Responsibilities = dto.Responsibilities,
                CreatedAt = DateTime.UtcNow
            };

            _context.CandidateExperiences.Add(exp);
            await _context.SaveChangesAsync();

            return Ok(exp);
        }

        [HttpPut("experiences/{id}")]
        public async Task<IActionResult> UpdateExperience(int id, [FromBody] ExperienceDto dto)
        {
            var exp = await _context.CandidateExperiences.FindAsync(id);
            if (exp == null) return NotFound(new { message = "Experience entry not found" });

            exp.EmployerName = dto.EmployerName;
            exp.EmployerDescriptor = dto.EmployerDescriptor;
            exp.JobTitle = dto.JobTitle;
            exp.Industry = dto.Industry;
            exp.StartDate = dto.StartDate;
            exp.EndDate = dto.EndDate;
            exp.Responsibilities = dto.Responsibilities;
            exp.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(exp);
        }

        [HttpDelete("experiences/{id}")]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var exp = await _context.CandidateExperiences.FindAsync(id);
            if (exp == null) return NotFound();

            _context.CandidateExperiences.Remove(exp);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Experience deleted" });
        }

        [HttpPost("qualifications")]
        public async Task<IActionResult> AddQualification([FromBody] QualificationDto dto)
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound(new { message = "Profile not found" });

            var qual = new CandidateQualification
            {
                CandidateProfileId = profile.Id,
                QualificationName = dto.QualificationName,
                InstitutionName = dto.InstitutionName,
                InstitutionDescriptor = dto.InstitutionDescriptor,
                Level = dto.Level,
                YearCompleted = dto.YearCompleted,
                CreatedAt = DateTime.UtcNow
            };

            _context.CandidateQualifications.Add(qual);
            await _context.SaveChangesAsync();

            return Ok(qual);
        }

        [HttpPut("qualifications/{id}")]
        public async Task<IActionResult> UpdateQualification(int id, [FromBody] QualificationDto dto)
        {
            var qual = await _context.CandidateQualifications.FindAsync(id);
            if (qual == null) return NotFound();

            qual.QualificationName = dto.QualificationName;
            qual.InstitutionName = dto.InstitutionName;
            qual.InstitutionDescriptor = dto.InstitutionDescriptor;
            qual.Level = dto.Level;
            qual.YearCompleted = dto.YearCompleted;
            qual.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(qual);
        }

        [HttpDelete("qualifications/{id}")]
        public async Task<IActionResult> DeleteQualification(int id)
        {
            var qual = await _context.CandidateQualifications.FindAsync(id);
            if (qual == null) return NotFound();

            _context.CandidateQualifications.Remove(qual);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Qualification deleted" });
        }

        [HttpPost("documents")]
        public async Task<IActionResult> UploadDocument([FromBody] DocumentUploadDto dto)
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound(new { message = "Profile not found" });

            var doc = new CandidateDocument
            {
                CandidateProfileId = profile.Id,
                DocumentType = dto.DocumentType,
                FileContentBase64 = dto.Base64Content,
                OriginalFilename = dto.OriginalFilename,
                MimeType = dto.MimeType,
                FileSizeBytes = dto.FileSizeBytes,
                ScanStatus = ScanStatus.Clean,
                CreatedAt = DateTime.UtcNow
            };

            _context.CandidateDocuments.Add(doc);
            await _context.SaveChangesAsync();

            return Ok(new { id = doc.Id, message = "Document uploaded successfully" });
        }

        [HttpDelete("documents/{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var doc = await _context.CandidateDocuments.FindAsync(id);
            if (doc == null) return NotFound();

            _context.CandidateDocuments.Remove(doc);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Document deleted" });
        }

        [HttpPatch("availability")]
        public async Task<IActionResult> ToggleAvailability([FromBody] Availability availability)
        {
            var userId = GetUserId();
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return NotFound();

            profile.Availability = availability;
            profile.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { availability = profile.Availability.ToString() });
        }
    }
}
