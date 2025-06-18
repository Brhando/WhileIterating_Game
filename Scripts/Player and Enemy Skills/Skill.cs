using System;
public class Skill
{
    public string Name;
    public int StaminaCost;
    public int ActionCost;
    public SkillType SkillType;
    public PlayerClassType ForClassType;
    
    public Func<SkillResult> Execute;
}