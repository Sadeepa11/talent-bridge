using System;
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
}
