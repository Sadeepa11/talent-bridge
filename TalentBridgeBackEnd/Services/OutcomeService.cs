using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Services
{
    public class OutcomeService
    {
        private readonly AppDbContext _context;

        public OutcomeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ReportOutcome(int grantId, OutcomeValue outcomeValue, int reportedBy, OutcomeSource via, string? notes)
        {
            var existing = await _context.Outcomes.FirstOrDefaultAsync(o => o.GrantId == grantId);
            if (existing != null)
            {
                existing.OutcomeValue = outcomeValue;
                existing.ReportedBy = reportedBy;
                existing.ReportedVia = via;
                existing.Notes = notes;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var outcome = new Outcome
                {
                    GrantId = grantId,
                    OutcomeValue = outcomeValue,
                    ReportedBy = reportedBy,
                    ReportedVia = via,
                    Notes = notes,
                    ConfirmedByAdmin = false,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Outcomes.Add(outcome);
            }

            await _context.SaveChangesAsync();
        }

        public async Task ConfirmOutcome(int outcomeId, int adminUserId)
        {
            var outcome = await _context.Outcomes.FindAsync(outcomeId);
            if (outcome == null) throw new Exception("Outcome not found");

            outcome.ConfirmedByAdmin = true;
            outcome.UpdatedAt = DateTime.UtcNow;

            if (outcome.OutcomeValue == OutcomeValue.Hired)
            {
                var grant = await _context.Grants.FindAsync(outcome.GrantId);
                if (grant != null)
                {
                    var profile = await _context.CandidateProfiles.FindAsync(grant.CandidateProfileId);
                    if (profile != null)
                    {
                        profile.Status = CandidateStatus.Placed;
                        profile.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
