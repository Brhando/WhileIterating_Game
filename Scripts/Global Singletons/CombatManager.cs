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

        _buffApplied = new() {0, 0, 0, 0, 0, 0, 0, 0}; //used to track bools for if a buff was applied from prayer
    }
    
    //Combat variables to track
    public int ActionsLeft = 3;
    public int AttackCounter;
    public int DefendCounter;
    public int PrayCounter;
    public bool AttackChain;
    public bool DefendChain;
    public bool PrayerChain;
    private List<int> _buffApplied;
    
    
    //turn tracker
    public enum BattleState { PlayerTurn, EnemyTurn, Win, Lose }
    public BattleState CurrentBattleState { get; private set; } = BattleState.PlayerTurn;

    public void TurnReset()
    {
        ActionsLeft = 3;
        PlayerData.Instance.ResetStamina();

        for (int i = 0; i < 7; i++)
        {
            if (i == 0 && _buffApplied[i] == 1)
            {
                PlayerData.Instance.PlayerBlock -= 3;
            }

            if (i == 1 && _buffApplied[i] == 1)
            {
                PlayerData.Instance.PlayerDamageLight -= 3;
                PlayerData.Instance.PlayerDamageMid -= 3;
                PlayerData.Instance.PlayerDamageHeavy -= 3;
            }

            if (i == 2 && _buffApplied[i] == 1)
            {
                PlayerData.Instance.PlayerDamageLight -= 6;
                PlayerData.Instance.PlayerDamageMid -= 6;
                PlayerData.Instance.PlayerDamageHeavy -= 6;
            }
            
            _buffApplied[i] = 0;
        }
        
    } 
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

    public void IncrementPrayerCounter()
    {
        PrayCounter++;
        PrayerChain = (PrayCounter != 0 && PrayCounter % 3 == 0);
    }

    public bool UseSkill(string skillName)
    {
        if (!SkillData.Instance.SkillLibrary.ContainsKey(skillName)) return false;
        
        var skill = SkillData.Instance.SkillLibrary[skillName];

        if (ActionsLeft < skill.ActionCost || PlayerData.Instance.GetPlayerStamina() < skill.ActionCost)
        {
            GD.Print("Not enough actions or stamina left");
            return false;
        }
        
        PlayerData.Instance.DecreaseStamina(skill.StaminaCost);
        ActionsLeft -= skill.ActionCost;
        skill.ExecuteEffect.Invoke();
        return true;
    }

    public void ApplyBlessing()
    {
        var blessingName = PlayerData.Instance.StoredBuff;

        switch (blessingName)
        {
            case "Invigorated":
                PlayerData.Instance.StoredBuff = "None";
                PlayerData.Instance.PlayerDamageHeavy += 3;
                PlayerData.Instance.PlayerDamageMid += 3;
                PlayerData.Instance.PlayerDamageLight += 3;
                _buffApplied[0] = 1;
                break;
            case "Stalwart":
                PlayerData.Instance.StoredBuff = "None";
                PlayerData.Instance.PlayerBlock += 3;
                _buffApplied[1] = 1;
                break;
            case "Mars's Pulse":
                PlayerData.Instance.StoredBuff = "None";
                PlayerData.Instance.IncreaseStamina(1);
                break;
            case "Raven's Claws":
                PlayerData.Instance.StoredBuff = "None";
                PlayerData.Instance.PlayerDamageHeavy += 6;
                PlayerData.Instance.PlayerDamageMid += 6;
                PlayerData.Instance.PlayerDamageLight += 6;
                _buffApplied[2] = 1;
                break;
            case "Vampiric Feast":
                PlayerData.Instance.StoredBuff = "None";
                PlayerData.Instance.Heal(5);
                //add logic to deal damage to the enemy == 5
                _buffApplied[3] = 1;
                break;
            case "Strategic Knowledge":
                PlayerData.Instance.StoredBuff = "None";
                PlayerData.Instance.PlayerBlock += 6;
                _buffApplied[4] = 1;
                break;
            case "Eir's Blessing":
                PlayerData.Instance.StoredBuff = "None";
                PlayerData.Instance.Heal(8);
                _buffApplied[5] = 1;
                break;
            case "Quick Reflexes":
                PlayerData.Instance.StoredBuff = "None";
                //Todo: set the debuff status of the player to none
                _buffApplied[6] = 1;
                break;
        }
        
    }
}