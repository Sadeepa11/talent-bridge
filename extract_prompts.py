import json
transcript_path = r"C:\Users\stf\.gemini\antigravity-cli\brain\f705fe24-6a7b-4c3b-a6a9-266d60574c23\.system_generated\logs\transcript_full.jsonl"
out_path = r"D:\TalentBridge\prompts.txt"
with open(transcript_path, 'r', encoding='utf-8') as f, open(out_path, 'w', encoding='utf-8') as out:
    for line in f:
        try:
            data = json.loads(line)
            if data.get("type") == "USER_INPUT":
                out.write(data.get("content", ""))
                out.write("\n---\n")
        except:
            pass
