using System.Linq;
using TalentBridgeBackEnd.DTOs.Candidate;

namespace TalentBridgeBackEnd.Services;

public class MaskingEngine
{
    public PreviewProfileDto ApplyMasking(PreviewProfileDto profile)
    {
        if (profile == null) return null!;

        // Masking logic: Replace actual names with descriptors for preview
        if (profile.Experiences != null)
        {
            foreach (var exp in profile.Experiences)
            {
                exp.EmployerName = exp.EmployerDescriptor ?? "Confidential Employer";
            }
        }

        if (profile.Qualifications != null)
        {
            foreach (var qual in profile.Qualifications)
            {
                qual.InstitutionName = qual.InstitutionDescriptor ?? "Confidential Institution";
            }
        }

        return profile;
    }
}
