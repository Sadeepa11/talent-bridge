import os
import re

models_dir = r"D:\TalentBridge\TalentBridgeBackEnd\Models"
dtos_dir = r"D:\TalentBridge\TalentBridgeBackEnd\DTOs"

if not os.path.exists(dtos_dir):
    os.makedirs(dtos_dir)

model_files = [f for f in os.listdir(models_dir) if f.endswith('.cs')]

for model_file in model_files:
    model_path = os.path.join(models_dir, model_file)
    with open(model_path, 'r', encoding='utf-8') as f:
        content = f.read()

    model_name = model_file.replace('.cs', '')
    
    # Extract properties
    # This regex looks for: public Type Name { get; set; }
    # Also handles virtual, ? for nullable, etc.
    # Note: this is a simple approximation
    props = re.findall(r'public\s+(?:virtual\s+)?([^\s]+)\s+([^\s]+)\s*\{\s*get;\s*set;\s*\}', content)
    
    dto_name = f"{model_name}Dto"
    dto_content = "using System;\nusing System.Collections.Generic;\n\n"
    dto_content += "namespace TalentBridgeBackEnd.DTOs\n{\n"
    dto_content += f"    public class {dto_name}\n    {{\n"
    
    for p_type, p_name in props:
        # Ignore navigation properties generically if they are ICollection
        if "ICollection" in p_type:
            continue
        dto_content += f"        public {p_type} {p_name} {{ get; set; }}\n"
        
    dto_content += "    }\n}\n"
    
    dto_path = os.path.join(dtos_dir, f"{dto_name}.cs")
    with open(dto_path, 'w', encoding='utf-8') as f:
        f.write(dto_content)
        
    print(f"Created {dto_path}")
