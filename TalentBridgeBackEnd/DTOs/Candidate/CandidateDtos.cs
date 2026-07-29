using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.DTOs.Candidate
{
    public class PreviewProfileDto
    {
        public int Id { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public string JobCategory { get; set; } = string.Empty;
        public string PositionSought { get; set; } = string.Empty;
        public int YearsExperience { get; set; }
        public ExperienceBand ExperienceBand { get; set; }
        public string HighestQualification { get; set; } = string.Empty;
        public string MainCity { get; set; } = string.Empty;
        public Availability Availability { get; set; }
        public decimal? ExpectedSalaryMin { get; set; }
        public decimal? ExpectedSalaryMax { get; set; }
        public CandidateStatus Status { get; set; }
        public int CompletenessPct { get; set; }
        public List<string> Skills { get; set; } = new();
        public List<ExperienceDto> Experiences { get; set; } = new();
        public List<QualificationDto> Qualifications { get; set; } = new();
    }

    public class FullProfileDto : PreviewProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string NicNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string? PostalCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? ProfilePhotoBase64 { get; set; }
        public List<DocumentUploadDto> Documents { get; set; } = new();
        public List<ExperienceDto> UnmaskedExperiences { get; set; } = new();
        public List<QualificationDto> UnmaskedQualifications { get; set; } = new();
    }

    public class ProfileUpdateDto
    {
        public int? JobCategoryId { get; set; }
        public string PositionSought { get; set; } = string.Empty;
        public int YearsExperience { get; set; }
        public ExperienceBand ExperienceBand { get; set; }
        public string HighestQualification { get; set; } = string.Empty;
        public string MainCity { get; set; } = string.Empty;
        public Availability Availability { get; set; }
        public decimal? ExpectedSalaryMin { get; set; }
        public decimal? ExpectedSalaryMax { get; set; }
    }

    public class ExperienceDto
    {
        public int Id { get; set; }
        public string EmployerName { get; set; } = string.Empty;
        public string? EmployerDescriptor { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Responsibilities { get; set; } = string.Empty;
    }

    public class QualificationDto
    {
        public int Id { get; set; }
        public string QualificationName { get; set; } = string.Empty;
        public string InstitutionName { get; set; } = string.Empty;
        public string? InstitutionDescriptor { get; set; }
        public QualificationLevel Level { get; set; }
        public int YearCompleted { get; set; }
    }

    public class DocumentUploadDto
    {
        public int Id { get; set; }
        public DocumentType DocumentType { get; set; }
        public string OriginalFilename { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public string Base64Content { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public ScanStatus ScanStatus { get; set; }
    }
}
