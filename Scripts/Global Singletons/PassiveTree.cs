using Godot;
using System;
using System.Collections.Generic;

public partial class PassiveTree : Node
{
    //used to track passive skills and their unlock thresholds
    public static PassiveTree Instance;

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
    
    public Dictionary<string, int> PassiveTreeSkills = new();
}