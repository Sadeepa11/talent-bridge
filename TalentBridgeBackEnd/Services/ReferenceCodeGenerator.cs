using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Data;

namespace TalentBridgeBackEnd.Services;

public class ReferenceCodeGenerator
{
    private readonly AppDbContext _context;

    public ReferenceCodeGenerator(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateCandidateCode()
    {
        int year = DateTime.UtcNow.Year;
        // Ideally we would use a sequence, but count is used here for simplicity.
        int count = await _context.CandidateProfiles.CountAsync();
        return $"CND-{year}-{(count + 1):D4}";
    }

    public async Task<string> GenerateBatchCode()
    {
        int year = DateTime.UtcNow.Year;
        int count = await _context.Batches.CountAsync();
        return $"BAT-{year}-{(count + 1):D4}";
    }

    public async Task<string> GenerateOrderCode()
    {
        int year = DateTime.UtcNow.Year;
        int count = await _context.Orders.CountAsync();
        return $"ORD-{year}-{(count + 1):D4}";
    }
}
