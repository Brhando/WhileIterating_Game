using System.Collections.Generic;
using Godot;

public partial class PlayerClassManager : Node
{
    public static PlayerClassManager Instance;
    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();
        
        CallDeferred(nameof(DeferredInit));
    }

    public PlayerClassData CurrentPlayerClass;
    
    public readonly Dictionary<string, PlayerClassData> PlayerClasses = new();

    private void LoadClasses()
    {
        AddClass(new PlayerClassData("Sword Swinger", PlayerClassType.SwordSwinger));
        AddClass(new PlayerClassData("Tome Reader", PlayerClassType.TomeReader));
        AddClass(new PlayerClassData("Berserker", PlayerClassType.Berserker));
        AddClass(new PlayerClassData("Warder", PlayerClassType.Warder));
        foreach (var playerClass in PlayerClasses.Values)
        {
            LoadSkills(playerClass);
        }
    }

    private void LoadSkills(PlayerClassData playerClass)
    {
        if (SkillData.Instance?.SkillLibrary == null) return;

        foreach (var skill in SkillData.Instance.SkillLibrary.Values)
        {
            if (skill.ForClassType == playerClass.Type || skill.ForClassType == PlayerClassType.All)
            {
                playerClass.AllSkills[skill.Name] = skill;
            }
        }
    }
    
    private void DeferredInit()
    {
        LoadClasses();
    }

    public void SetCurrentPlayerClass(string name)
    {
        CurrentPlayerClass = PlayerClasses[name];
        GD.Print($"Class set to {name}.");
    }

    public string GetClassName()
    {
        return CurrentPlayerClass.Name;
    } 
    
    private void AddClass(PlayerClassData playerClass)
    {
        if (!PlayerClasses.ContainsKey(playerClass.Name))
            PlayerClasses.Add(playerClass.Name, playerClass);
    }
    

    

}