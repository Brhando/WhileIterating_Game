using System;
public class Skill
{
    public string Name;
    public int StaminaCost;
    public int ActionCost;
    public SkillType SkillType;
    public PlayerClassType ForClassType;
    public int DevotionReq = 0;
    
    public Func<SkillResult> Execute;
}