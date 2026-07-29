using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.DTOs.Admin;
using TalentBridgeBackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace TalentBridgeBackEnd.Services
{
    public class BatchCurationService
    {
        private readonly AppDbContext _context;

        public BatchCurationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CandidateProfile>> SearchAvailableCandidatesAsync(CandidateSearchFilterDto filters)
        {
            var query = _context.CandidateProfiles
                .Where(p => p.Status == CandidateStatus.Published);

            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.City))
                {
                    query = query.Where(p => p.MainCity == filters.City);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Batch> CreateAndCommitBatchAsync(BatchCreateDto batchDto, int adminUserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var batchCode = $"BAT-{DateTime.UtcNow.Year}-{await _context.Batches.CountAsync() + 1:D4}";

                var batch = new Batch
                {
                    BatchCode = batchCode,
                    Title = batchDto.Title,
                    CompanyId = batchDto.CompanyId,
                    DefaultValidFrom = batchDto.ValidFrom,
                    DefaultValidUntil = batchDto.ValidUntil,
                    FilterCriteriaJson = "{}",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = adminUserId,
                    Status = BatchStatus.Issued
                };

                _context.Batches.Add(batch);
                await _context.SaveChangesAsync();

                foreach (var candidateId in batchDto.CandidateIds)
                {
                    var existingGrant = await _context.Grants
                        .AnyAsync(g => g.CandidateProfileId == candidateId && g.Status == GrantStatus.Active);

                    if (existingGrant)
                    {
                        throw new Exception($"Conflict: Candidate {candidateId} already has an active grant.");
                    }

                    var grant = new Grant
                    {
                        BatchId = batch.Id,
                        CompanyId = batch.CompanyId,
                        CandidateProfileId = candidateId,
                        Scope = batchDto.Scope,
                        ValidFrom = batchDto.ValidFrom,
                        ValidUntil = batchDto.ValidUntil,
                        IssuedBy = adminUserId,
                        Status = GrantStatus.Active,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Grants.Add(grant);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return batch;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
