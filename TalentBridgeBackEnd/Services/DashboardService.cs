using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.DTOs.Admin;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetAdminStats()
        {
            var publishedCount = await _context.CandidateProfiles.CountAsync(p => p.Status == CandidateStatus.Published);
            var availableCount = await _context.CandidateProfiles.CountAsync(p => p.Status == CandidateStatus.Approved || p.Status == CandidateStatus.Published);
            var reservedCount = await _context.CandidateProfiles.CountAsync(p => p.Status == CandidateStatus.Reserved);
            
            var now = DateTime.UtcNow;
            var activeGrantsCount = await _context.Grants.CountAsync(g => g.Status == GrantStatus.Active && g.ValidFrom <= now && g.ValidUntil >= now);
            var expiringCount = await _context.Grants.CountAsync(g => g.Status == GrantStatus.Active && g.ValidUntil <= now.AddDays(7));
            var awaitingPaymentTotal = await _context.Orders.Where(o => o.Status == OrderStatus.AwaitingPayment).SumAsync(o => o.Total);

            return new DashboardStatsDto
            {
                PublishedCount = publishedCount,
                AvailableCount = availableCount,
                ReservedCount = reservedCount,
                ActiveGrantsCount = activeGrantsCount,
                ExpiringCount = expiringCount,
                AwaitingPaymentTotal = awaitingPaymentTotal
            };
        }

        public async Task<object> GetCategoryBreakdown()
        {
            var breakdown = await _context.CandidateProfiles
                .GroupBy(p => p.PositionSought)
                .Select(g => new
                {
                    Category = g.Key,
                    Available = g.Count(p => p.Status == CandidateStatus.Published),
                    Reserved = g.Count(p => p.Status == CandidateStatus.Reserved)
                })
                .ToListAsync();

            return breakdown;
        }

        public async Task<IEnumerable<object>> GetExpiringGrants(int days)
        {
            var threshold = DateTime.UtcNow.AddDays(days);
            var now = DateTime.UtcNow;

            var grants = await _context.Grants
                .Include(g => g.Company)
                .Include(g => g.CandidateProfile)
                .Where(g => g.Status == GrantStatus.Active && g.ValidUntil >= now && g.ValidUntil <= threshold)
                .Select(g => new
                {
                    g.Id,
                    CompanyName = g.Company.Name,
                    CandidateRef = g.CandidateProfile.ReferenceCode,
                    g.Scope,
                    g.ValidUntil,
                    DaysRemaining = (g.ValidUntil - now).Days
                })
                .ToListAsync();

            return grants;
        }
    }
}
