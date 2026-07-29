using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.DTOs.Candidate;
using TalentBridgeBackEnd.Models;

namespace TalentBridgeBackEnd.Services;

public class PreviewProjectionService
{
    private readonly AppDbContext _context;
    private readonly MaskingEngine _maskingEngine;

    public PreviewProjectionService(AppDbContext context, MaskingEngine maskingEngine)
    {
        _context = context;
        _maskingEngine = maskingEngine;
    }

    public async Task<PreviewProfileDto?> GetPreviewProfile(int profileId)
    {
        var profile = await _context.CandidateProfiles
            .Include(p => p.Experiences)
            .Include(p => p.Qualifications)
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == profileId);

        if (profile == null) return null;

        var dto = MapToPreviewDto(profile);
        return _maskingEngine.ApplyMasking(dto);
    }

    public async Task<List<PreviewProfileDto>> SearchPublishedCandidates(string? category, string? experienceBand, string? city, string? availability)
    {
        var query = _context.CandidateProfiles
            .Include(p => p.Experiences)
            .Include(p => p.Qualifications)
            .Include(p => p.Skills)
            .Where(p => p.Status == TalentBridgeBackEnd.Models.Enums.CandidateStatus.Published)
            .AsQueryable();

        if (!string.IsNullOrEmpty(experienceBand) && Enum.TryParse<TalentBridgeBackEnd.Models.Enums.ExperienceBand>(experienceBand, out var expBandEnum))
            query = query.Where(p => p.ExperienceBand == expBandEnum);

        if (!string.IsNullOrEmpty(city))
            query = query.Where(p => p.MainCity == city);

        if (!string.IsNullOrEmpty(availability) && Enum.TryParse<TalentBridgeBackEnd.Models.Enums.Availability>(availability, out var availEnum))
            query = query.Where(p => p.Availability == availEnum);

        var profiles = await query.ToListAsync();

        return profiles.Select(p => _maskingEngine.ApplyMasking(MapToPreviewDto(p))).ToList();
    }

    private static PreviewProfileDto MapToPreviewDto(CandidateProfile profile)
    {
        return new PreviewProfileDto
        {
            Id = profile.Id,
            ReferenceCode = profile.ReferenceCode,
            PositionSought = profile.PositionSought,
            YearsExperience = profile.YearsExperience,
            ExperienceBand = profile.ExperienceBand,
            HighestQualification = profile.HighestQualification,
            MainCity = profile.MainCity,
            Availability = profile.Availability,
            ExpectedSalaryMin = profile.ExpectedSalaryMin,
            ExpectedSalaryMax = profile.ExpectedSalaryMax,
            Status = profile.Status,
            CompletenessPct = profile.CompletenessPct,
            Experiences = profile.Experiences.Select(e => new ExperienceDto
            {
                Id = e.Id,
                EmployerName = e.EmployerName,
                EmployerDescriptor = e.EmployerDescriptor,
                JobTitle = e.JobTitle,
                Industry = e.Industry,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Responsibilities = e.Responsibilities
            }).ToList(),
            Qualifications = profile.Qualifications.Select(q => new QualificationDto
            {
                Id = q.Id,
                QualificationName = q.QualificationName,
                InstitutionName = q.InstitutionName,
                InstitutionDescriptor = q.InstitutionDescriptor,
                Level = q.Level,
                YearCompleted = q.YearCompleted
            }).ToList()
        };
    }
}
