using Godot;
using System;
using System.Collections.Generic;

public partial class DevotionTree : Node
{
    //used to track passive skills and their unlock thresholds
    public static DevotionTree Instance;

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
    
    public Dictionary<string, int> Passives = new();
    
    public int DefenseDevotionPoints = 0;
    public int AttackDevotionPoints = 0;
    public int PrayerDevotionPoints = 0;

    public void ResetDevotionPoints()
    {
        DefenseDevotionPoints = 0;
        AttackDevotionPoints = 0;
        PrayerDevotionPoints = 0;
    }

    public void IncrementSelectedPoints(char sel)
    {
        switch (sel)
        {
            case 'd':
                DefenseDevotionPoints++;
                break;
            case 'a':
                AttackDevotionPoints++;
                break;
            case 'p':
                PrayerDevotionPoints++;
                break;
        }
    }
}