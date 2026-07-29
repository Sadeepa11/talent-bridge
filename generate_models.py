import os

base_dir = r"D:\TalentBridge\TalentBridgeBackEnd"

def create_file(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, 'w', encoding='utf-8') as f:
        f.write(content)

models = {
    "User.cs": """using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int? CompanyId { get; set; }
        public UserStatus Status { get; set; }
        public string? TwoFactorSecret { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockedUntil { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        public Company? Company { get; set; }
    }
}""",

    "CandidateProfile.cs": """using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class CandidateProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public int? JobCategoryId { get; set; }
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
        public DateTime LastActivityAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? ProfilePhotoBase64 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User? User { get; set; }
        public JobCategory? JobCategory { get; set; }
        public List<CandidateExperience> Experiences { get; set; } = new();
        public List<CandidateQualification> Qualifications { get; set; } = new();
        public List<CandidateSkill> Skills { get; set; } = new();
        public List<CandidateDocument> Documents { get; set; } = new();
        public List<Grant> Grants { get; set; } = new();
        public List<AccessEvent> AccessEvents { get; set; } = new();
        public List<ProfileVersion> ProfileVersions { get; set; } = new();
        public List<Consent> Consents { get; set; } = new();
        public List<CandidateCategory> Categories { get; set; } = new();
    }
}""",

    "CandidatePii.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class CandidatePii
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string NicNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string? PostalCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public CandidateProfile? CandidateProfile { get; set; }
    }
}""",

    "CandidateExperience.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class CandidateExperience
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public string EmployerName { get; set; } = string.Empty;
        public string EmployerDescriptor { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Responsibilities { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}""",

    "CandidateQualification.cs": """using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class CandidateQualification
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public string QualificationName { get; set; } = string.Empty;
        public string InstitutionName { get; set; } = string.Empty;
        public string InstitutionDescriptor { get; set; } = string.Empty;
        public QualificationLevel Level { get; set; }
        public int YearCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}""",

    "CandidateDocument.cs": """using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class CandidateDocument
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string FileContentBase64 { get; set; } = string.Empty;
        public string OriginalFilename { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public ScanStatus ScanStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}""",

    "ProfileVersion.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class ProfileVersion
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public int VersionNumber { get; set; }
        public string SnapshotJson { get; set; } = string.Empty;
        public int ApprovedBy { get; set; }
        public DateTime ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}""",

    "Company.cs": """using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BusinessRegNo { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public DateTime? OnboardingMeetingDate { get; set; }
        public int? OnboardedBy { get; set; }
        public string? AgreementReference { get; set; }
        public CompanyStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<User> Users { get; set; } = new();
        public List<Batch> Batches { get; set; } = new();
        public List<Grant> Grants { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}""",

    "Batch.cs": """using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Batch
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FilterCriteriaJson { get; set; } = string.Empty;
        public DateTime DefaultValidFrom { get; set; }
        public DateTime DefaultValidUntil { get; set; }
        public int CreatedBy { get; set; }
        public BatchStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Company? Company { get; set; }
        public User? Creator { get; set; }
        public List<Grant> Grants { get; set; } = new();
    }
}""",

    "Grant.cs": """using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Grant
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int CompanyId { get; set; }
        public int CandidateProfileId { get; set; }
        public GrantScope Scope { get; set; }
        public GrantStatus Status { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public int? OrderId { get; set; }
        public int? SupersedesGrantId { get; set; }
        public int IssuedBy { get; set; }
        public int? RevokedBy { get; set; }
        public string? RevocationReason { get; set; }
        public int ExtensionCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Batch? Batch { get; set; }
        public Company? Company { get; set; }
        public CandidateProfile? CandidateProfile { get; set; }
        public Order? Order { get; set; }
        public Grant? SupersededGrant { get; set; }
        public User? Issuer { get; set; }
        public List<AccessEvent> AccessEvents { get; set; } = new();
        public Outcome? Outcome { get; set; }
    }
}""",

    "Order.cs": """using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string? PaymentReference { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? ConfirmedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Company? Company { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public List<Grant> Grants { get; set; } = new();
        public User? Confirmer { get; set; }
    }
}""",

    "OrderItem.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CandidateProfileId { get; set; }
        public decimal UnitPrice { get; set; }
        public int AccessDays { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}""",

    "AccessEvent.cs": """using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class AccessEvent
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int CandidateProfileId { get; set; }
        public int? ProfileVersionId { get; set; }
        public AccessEventType EventType { get; set; }
        public int? DocumentId { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }
}""",

    "Consent.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class Consent
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public string TermsVersion { get; set; } = string.Empty;
        public string PrivacyVersion { get; set; } = string.Empty;
        public string ConsentScope { get; set; } = string.Empty;
        public DateTime GrantedAt { get; set; }
        public DateTime? WithdrawnAt { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}""",

    "Outcome.cs": """using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Outcome
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public OutcomeValue OutcomeValue { get; set; }
        public int ReportedBy { get; set; }
        public OutcomeSource ReportedVia { get; set; }
        public string? ContactMethod { get; set; }
        public string? Notes { get; set; }
        public bool ConfirmedByAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public Grant? Grant { get; set; }
    }
}""",

    "FollowUpTask.cs": """using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class FollowUpTask
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public int AssignedTo { get; set; }
        public TaskType TaskType { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public string? ResolutionNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}""",

    "MaskingRule.cs": """using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class MaskingRule
    {
        public int Id { get; set; }
        public MaskingRuleType RuleType { get; set; }
        public string? Pattern { get; set; }
        public ReplacementStrategy ReplacementStrategy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}""",

    "AuditLog.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int ActorUserId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? BeforeJson { get; set; }
        public string? AfterJson { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }
}""",

    "JobCategory.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class JobCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}""",

    "Skill.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}""",

    "CandidateSkill.cs": """namespace TalentBridgeBackEnd.Models
{
    public class CandidateSkill
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public int SkillId { get; set; }
        public string? ProficiencyLevel { get; set; }
    }
}""",

    "CandidateCategory.cs": """namespace TalentBridgeBackEnd.Models
{
    public class CandidateCategory
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public int JobCategoryId { get; set; }
    }
}""",

    "CompanyNote.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class CompanyNote
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CandidateProfileId { get; set; }
        public string NoteText { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}""",

    "AccessRequest.cs": """using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class AccessRequest
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CandidateProfileId { get; set; }
        public int? GrantId { get; set; }
        public AccessRequestStatus Status { get; set; }
        public string? Notes { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}""",

    "Notification.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string? ReferenceType { get; set; }
        public long? ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}""",

    "Setting.cs": """using System;

namespace TalentBridgeBackEnd.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}"""
}

for name, content in models.items():
    create_file(os.path.join(base_dir, "Models", name), content)

print("Models generated.")
