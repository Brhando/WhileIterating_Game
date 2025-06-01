using Godot;
using System;
using System.Collections.Generic;

public partial class CombatManager: Node
{
    //used to track combat
    public static CombatManager Instance;

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
    
    //Combat variables to track
    public int ActionsLeft = 3;
    public int AttackCounter;
    public int DefendCounter;
    public int PrayCounter;
    public bool AttackChain;
    public bool DefendChain;
    public bool PrayerChain;
    
    
    //turn tracker
    public enum BattleState { PlayerTurn, EnemyTurn, Win, Lose }
    public BattleState CurrentBattleState { get; private set; } = BattleState.PlayerTurn;
    
    //TODO: create a function to reset actions left and counters 
    public void IncrementAttackCounter()
    {
        AttackCounter++;
        AttackChain = (AttackCounter != 0 && AttackCounter % 3 == 0);
    }

    public void IncrementDefendCounter()
    {
        DefendCounter++;
        DefendChain = (DefendCounter != 0 && DefendCounter % 3 == 0);
    }

    public void IncrementPrayCounter()
    {
        PrayCounter++;
        PrayerChain = (PrayCounter != 0 && PrayCounter % 3 == 0);
    }

}