using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.DTOs.Candidate;

namespace TalentBridgeBackEnd.DTOs.Admin
{
    public class ModerationQueueItemDto
    {
        public int CandidateProfileId { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public CandidateStatus Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

    public class CompanyCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string BusinessRegNo { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
    }

    public class CompanyDto : CompanyCreateDto
    {
        public int Id { get; set; }
        public CompanyStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CompanyUserCreateDto
    {
        public int CompanyId { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class BatchCreateDto
    {
        public int CompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<int> CandidateIds { get; set; } = new();
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public GrantScope Scope { get; set; }
    }

    public class BatchDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public BatchStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GrantDto
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int CompanyId { get; set; }
        public int CandidateProfileId { get; set; }
        public GrantScope Scope { get; set; }
        public GrantStatus Status { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
    }

    public class OrderCreateDto
    {
        public int CompanyId { get; set; }
        public List<int> CandidateIds { get; set; } = new();
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
    }

    public class DashboardStatsDto
    {
        public int PublishedCount { get; set; }
        public int AvailableCount { get; set; }
        public int ReservedCount { get; set; }
        public int ActiveGrantsCount { get; set; }
        public int ExpiringCount { get; set; }
        public decimal AwaitingPaymentTotal { get; set; }
    }

    public class CandidateSearchFilterDto
    {
        public string? Category { get; set; }
        public string? ExperienceBand { get; set; }
        public string? City { get; set; }
        public string? Availability { get; set; }
        public string? Skill { get; set; }
    }

    public class FollowUpDto
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public TaskType TaskType { get; set; }
        public DateTime DueDate { get; set; }
        public TalentBridgeBackEnd.Models.Enums.TaskStatus Status { get; set; }
        public string? ResolutionNotes { get; set; }
    }

    public class ReportDto
    {
        public string ReportType { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
