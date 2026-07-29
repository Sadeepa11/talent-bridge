using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace TalentBridgeBackEnd.Services
{
    public class ModerationService
    {
        private readonly AppDbContext _context;

        public ModerationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CandidateProfile>> GetModerationQueueAsync()
        {
            return await _context.CandidateProfiles
                .Where(p => p.Status == CandidateStatus.Submitted || p.Status == CandidateStatus.UnderReview)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task ApproveProfileAsync(int profileId, int adminUserId, string notes)
        {
            var profile = await _context.CandidateProfiles.FindAsync(profileId);
            if (profile == null) throw new Exception("Profile not found");

            if (profile.Status != CandidateStatus.Submitted && profile.Status != CandidateStatus.UnderReview)
                throw new Exception("Invalid state transition");

            profile.Status = CandidateStatus.Approved;
            profile.UpdatedAt = DateTime.UtcNow;

            var snapshot = new ProfileVersion
            {
                CandidateProfileId = profile.Id,
                VersionNumber = 1,
                SnapshotJson = System.Text.Json.JsonSerializer.Serialize(profile),
                ApprovedBy = adminUserId,
                ApprovedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            _context.ProfileVersions.Add(snapshot);

            await _context.SaveChangesAsync();
        }

        public async Task RejectProfileAsync(int profileId, int adminUserId, string reason)
        {
            var profile = await _context.CandidateProfiles.FindAsync(profileId);
            if (profile == null) throw new Exception("Profile not found");

            if (profile.Status != CandidateStatus.Submitted && profile.Status != CandidateStatus.UnderReview)
                throw new Exception("Invalid state transition");

            profile.Status = CandidateStatus.Rejected;
            profile.UpdatedAt = DateTime.UtcNow;

            var notification = new Notification
            {
                UserId = profile.UserId,
                Title = "Profile Rejected",
                Message = $"Your profile has been rejected. Reason: {reason}",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                NotificationType = "System"
            };
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        public async Task PublishProfileAsync(int profileId)
        {
            var profile = await _context.CandidateProfiles.FindAsync(profileId);
            if (profile == null) throw new Exception("Profile not found");

            if (profile.Status != CandidateStatus.Approved)
                throw new Exception("Profile must be approved before publishing");

            profile.Status = CandidateStatus.Published;
            profile.PublishedAt = DateTime.UtcNow;
            profile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
