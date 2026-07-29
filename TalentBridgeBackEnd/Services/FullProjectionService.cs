using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.DTOs.Candidate;
using TalentBridgeBackEnd.Models;

namespace TalentBridgeBackEnd.Services;

public class FullProjectionService
{
    private readonly AppDbContext _context;

    public FullProjectionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FullProfileDto?> GetFullProfile(int profileId)
    {
        var profile = await _context.CandidateProfiles
            .Include(p => p.Experiences)
            .Include(p => p.Qualifications)
            .Include(p => p.Skills)
            .Include(p => p.Documents)
            .FirstOrDefaultAsync(p => p.Id == profileId);

        if (profile == null) return null;

        var pii = await _context.CandidatePiis.FirstOrDefaultAsync(p => p.CandidateProfileId == profileId);

        return new FullProfileDto
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
            FullName = pii?.FullName ?? string.Empty,
            NicNumber = pii?.NicNumber ?? string.Empty,
            Email = pii?.Email ?? string.Empty,
            Mobile = pii?.Mobile ?? string.Empty,
            AddressLine1 = pii?.AddressLine1 ?? string.Empty,
            AddressLine2 = pii?.AddressLine2,
            PostalCode = pii?.PostalCode,
            DateOfBirth = pii?.DateOfBirth ?? DateTime.MinValue,
            ProfilePhotoBase64 = profile.ProfilePhotoBase64,
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
            }).ToList(),
            Documents = profile.Documents.Select(d => new DocumentUploadDto
            {
                DocumentType = d.DocumentType,
                Base64Content = d.FileContentBase64,
                OriginalFilename = d.OriginalFilename,
                MimeType = d.MimeType
            }).ToList()
        };
    }
}
