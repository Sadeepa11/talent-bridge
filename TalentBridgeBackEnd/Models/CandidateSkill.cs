namespace TalentBridgeBackEnd.Models
{
    public class CandidateSkill
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public int SkillId { get; set; }
        public string? ProficiencyLevel { get; set; }
    }
}
