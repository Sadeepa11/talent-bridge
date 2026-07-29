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
    public class AccessEventService
    {
        private readonly AppDbContext _context;

        public AccessEventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogEventAsync(int grantId, int companyId, int userId, int candidateProfileId, int? profileVersionId, AccessEventType type, int? documentId, string ip, string userAgent)
        {
            var accessEvent = new AccessEvent
            {
                GrantId = grantId,
                CompanyId = companyId,
                UserId = userId,
                CandidateProfileId = candidateProfileId,
                ProfileVersionId = profileVersionId,
                EventType = type,
                DocumentId = documentId,
                IpAddress = ip,
                UserAgent = userAgent,
                OccurredAt = DateTime.UtcNow
            };

            _context.AccessEvents.Add(accessEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AccessEvent>> GetEventsForCandidateAsync(int profileId)
        {
            return await _context.AccessEvents
                .Where(e => e.CandidateProfileId == profileId)
                .OrderByDescending(e => e.OccurredAt)
                .ToListAsync();
        }
    }
}
