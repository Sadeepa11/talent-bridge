import os

base_dir = r"D:\TalentBridge\TalentBridgeBackEnd\Services"
interfaces_dir = os.path.join(base_dir, "Interfaces")

os.makedirs(interfaces_dir, exist_ok=True)
os.makedirs(base_dir, exist_ok=True)

models = [
    "AccessEvent", "AccessRequest", "AuditLog", "Batch", "CandidateCategory", 
    "CandidateDocument", "CandidateExperience", "CandidatePii", "CandidateProfile", 
    "CandidateQualification", "CandidateSkill", "Company", "CompanyNote", "Consent", 
    "FollowUpTask", "Grant", "JobCategory", "MaskingRule", "Order", "OrderItem", 
    "Outcome", "ProfileVersion", "Skill", "User"
]

for model in models:
    # Interface
    interface_content = f"""using System.Collections.Generic;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Models;

namespace TalentBridgeBackEnd.Services.Interfaces
{{
    public interface I{model}Service
    {{
        Task<{model}> GetByIdAsync(int id);
        Task<IEnumerable<{model}>> GetAllAsync();
        Task<{model}> CreateAsync({model} entity);
        Task UpdateAsync({model} entity);
        Task DeleteAsync(int id);
    }}
}}
"""
    interface_path = os.path.join(interfaces_dir, f"I{model}Service.cs")
    with open(interface_path, 'w', encoding='utf-8') as f:
        f.write(interface_content)

    # Implementation
    implementation_content = f"""using System.Collections.Generic;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Services.Interfaces;

namespace TalentBridgeBackEnd.Services
{{
    public class {model}Service : I{model}Service
    {{
        public async Task<{model}> GetByIdAsync(int id)
        {{
            // TODO: Implement
            return await Task.FromResult(new {model}());
        }}

        public async Task<IEnumerable<{model}>> GetAllAsync()
        {{
            // TODO: Implement
            return await Task.FromResult(new List<{model}>());
        }}

        public async Task<{model}> CreateAsync({model} entity)
        {{
            // TODO: Implement
            return await Task.FromResult(entity);
        }}

        public async Task UpdateAsync({model} entity)
        {{
            // TODO: Implement
            await Task.CompletedTask;
        }}

        public async Task DeleteAsync(int id)
        {{
            // TODO: Implement
            await Task.CompletedTask;
        }}
    }}
}}
"""
    implementation_path = os.path.join(base_dir, f"{model}Service.cs")
    with open(implementation_path, 'w', encoding='utf-8') as f:
        f.write(implementation_content)

print(f"Generated Services and Interfaces for {len(models)} models.")
