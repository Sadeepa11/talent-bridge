import os

def create_file(path, content):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, 'w', encoding='utf-8') as f:
        f.write(content)

base_dir = r"D:\TalentBridge\TalentBridgeBackEnd"

enums_content = """namespace TalentBridgeBackEnd.Models.Enums
{
    public enum UserRole { SuperAdmin, OpsAdmin, CompanyUser, Candidate }
    public enum UserStatus { Pending, Active, Suspended }
    public enum CandidateStatus { Draft, Submitted, UnderReview, Approved, Published, Reserved, Placed, Withdrawn, Rejected, Expired }
    public enum ExperienceBand { ZeroToOne, OneToThree, ThreeToFive, FiveToTen, TenPlus }
    public enum Availability { Immediate, OneMonth, TwoMonths, ThreeMonthsPlus }
    public enum QualificationLevel { Certificate, Diploma, Bachelors, Masters, Doctorate, Professional }
    public enum DocumentType { Cv, Certificate, Reference, Other }
    public enum ScanStatus { Pending, Clean, Infected }
    public enum CompanyStatus { Active, Suspended, Terminated }
    public enum BatchStatus { Draft, Issued, Closed }
    public enum GrantScope { Preview, Full }
    public enum GrantStatus { Draft, Active, Superseded, Lapsed, Revoked, Closed }
    public enum OrderStatus { Draft, Quoted, AwaitingPayment, PaymentReceived, Cancelled }
    public enum PaymentMethod { BankTransfer, Cheque, Cash }
    public enum AccessEventType { PreviewView, FullView, FieldReveal, DocumentDownload, SignedUrlIssued }
    public enum OutcomeValue { Hired, InterviewPending, Rejected, CandidateDeclined, NoResponse, NotContacted }
    public enum OutcomeSource { CompanyPortal, AdminManual }
    public enum TaskType { OutcomeChase, PaymentChase, Verification, Escalation }
    public enum TaskStatus { Open, InProgress, Done, Escalated }
    public enum MaskingRuleType { EmployerName, InstitutionName, SelfReference, LocationPrecision, CustomRegex }
    public enum ReplacementStrategy { Descriptor, Redact, Generalise }
    public enum AccessRequestStatus { Requested, Quoted, AwaitingPayment, Granted, Declined }
}
"""
create_file(os.path.join(base_dir, "Models", "Enums", "Enums.cs"), enums_content)

print("Enums generated.")
