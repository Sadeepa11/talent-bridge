using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.DTOs.Candidate;

namespace TalentBridgeBackEnd.DTOs.Company
{
    public class CompanyBatchDto
    {
        public int Id { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public BatchStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CompanyCandidateDto
    {
        public int CandidateProfileId { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public GrantScope Scope { get; set; }
        public PreviewProfileDto? PreviewProfile { get; set; }
        public FullProfileDto? FullProfile { get; set; }
    }

    public class AccessRequestCreateDto
    {
        public int CandidateProfileId { get; set; }
        public string? Notes { get; set; }
    }

    public class AccessRequestDto
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public AccessRequestStatus Status { get; set; }
        public string? Notes { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class OutcomeCreateDto
    {
        public OutcomeValue OutcomeValue { get; set; }
        public string? ContactMethod { get; set; }
        public string? Notes { get; set; }
    }

    public class OutcomeDto
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public OutcomeValue OutcomeValue { get; set; }
        public bool ConfirmedByAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CompanyDashboardDto
    {
        public int ActiveGrantsCount { get; set; }
        public int PendingAccessRequestsCount { get; set; }
        public int OutstandingInvoicesCount { get; set; }
    }
}
