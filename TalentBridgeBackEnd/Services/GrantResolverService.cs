using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Services;

public class GrantResolverService
{
    private readonly AppDbContext _context;

    public GrantResolverService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GrantResolution?> ResolveGrant(int companyId, int candidateProfileId)
    {
        var now = DateTime.UtcNow;
        var grant = await _context.Grants
            .Where(g => g.CompanyId == companyId 
                     && g.CandidateProfileId == candidateProfileId 
                     && g.Status == GrantStatus.Active
                     && g.ValidFrom <= now 
                     && g.ValidUntil >= now)
            .OrderByDescending(g => g.ValidUntil)
            .FirstOrDefaultAsync();

        return grant != null ? new GrantResolution
        {
            GrantId = grant.Id,
            Scope = grant.Scope,
            ValidUntil = grant.ValidUntil,
            CandidateProfileId = grant.CandidateProfileId
        } : null;
    }

    public async Task<List<GrantResolution>> ResolveGrantsForCompany(int companyId)
    {
        var now = DateTime.UtcNow;
        var grants = await _context.Grants
            .Where(g => g.CompanyId == companyId 
                     && g.Status == GrantStatus.Active
                     && g.ValidFrom <= now 
                     && g.ValidUntil >= now)
            .ToListAsync();

        return grants.Select(g => new GrantResolution
        {
            GrantId = g.Id,
            Scope = g.Scope,
            ValidUntil = g.ValidUntil,
            CandidateProfileId = g.CandidateProfileId
        }).ToList();
    }
}

public class GrantResolution
{
    public int GrantId { get; set; }
    public GrantScope Scope { get; set; }
    public DateTime ValidUntil { get; set; }
    public int CandidateProfileId { get; set; }
}
