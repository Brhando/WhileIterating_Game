using System.Collections.Generic;

public class PlayerClassData
{
    public string Name { get; set; }
    public PlayerClassType Type { get; set; }
    public Dictionary<string, Skill> AllSkills { get; set; }
    public Dictionary<int, string> UnlockedSkillBySlot { get; set; } // SLOT → SkillName
    public int DevotionPoints { get; set; }

    public PlayerClassData(string name = "", PlayerClassType type = PlayerClassType.None)
    {
        Name = name;
        Type = type;
        AllSkills = new Dictionary<string, Skill>();
        UnlockedSkillBySlot = new Dictionary<int, string>();
        DevotionPoints = 0;
    }

    public void UnlockSkillForSlot(int slotIndex, string skillName)
    {
        if (AllSkills.ContainsKey(skillName))
        {
            UnlockedSkillBySlot[slotIndex] = skillName;
        }
    }

    public Skill GetSkillForSlot(int slotIndex)
    {
        if (UnlockedSkillBySlot.TryGetValue(slotIndex, out var skillName) &&
            AllSkills.ContainsKey(skillName))
        {
            return AllSkills[skillName];
        }

        return null;
    }
    
    public void EnsureFallbackSkills()
    {
        var fallback = new Dictionary<int, string>
        {
            { 1, "Slash" },
            { 2, "Thrust" },
            { 3, "Light Block" },
            { 4, "Prayer" },
            { 5, "Whirlwind" }
        };

        foreach (var kvp in fallback)
        {
            if (!UnlockedSkillBySlot.ContainsKey(kvp.Key))
            {
                UnlockSkillForSlot(kvp.Key, kvp.Value);
            }
        }
    }
}

