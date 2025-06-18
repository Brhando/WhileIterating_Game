using Godot;
using System;
using System.Collections.Generic;

public partial class DevotionTree : Node
{
    //used to track devotion earned in game
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
    
    private int _defenseDevotionPoints = 0;
    private int _attackDevotionPoints = 0;
    private int _prayerDevotionPoints = 0;
    public int RunningDefensePoints = 0;
    public int RunningAttackPoints = 0;
    public int RunningPrayerPoints = 0;

    public void NormalizeDevotionPoints()
    {
        RunningDefensePoints += _defenseDevotionPoints / 3;
        RunningAttackPoints += _attackDevotionPoints / 3;
        RunningPrayerPoints += _prayerDevotionPoints;
        ResetDevotionPoints();
    }
    public void ResetDevotionPoints()
    {
        _defenseDevotionPoints = 0;
        _attackDevotionPoints = 0;
        _prayerDevotionPoints = 0;
    }

    public void IncrementSelectedPoints(char sel)
    {
        switch (sel)
        {
            case 'd':
                _defenseDevotionPoints++;
                break;
            case 'a':
                _attackDevotionPoints++;
                break;
            case 'p':
                _prayerDevotionPoints++;
                break;
        }
    }
}