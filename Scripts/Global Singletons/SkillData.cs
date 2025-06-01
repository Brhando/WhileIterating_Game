using Godot;
using System;
using System.Collections.Generic;

public partial class SkillData : Node
{
    //used to track different skill information
    public static SkillData Instance;
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
        }
    }
    
    public class Skill
    {
        public string Name;
        public int StaminaCost;
        public int ActionCost;
        public Action ExecuteEffect;
    }
    
    private List<string> _warGods;
    private List<string> _protectionGods;
}