using System;

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
}
