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
        _blessingEffects["Invigorated"] = () =>
        {
            PlayerData.Instance.PlayerDamageHeavy += 3;
            PlayerData.Instance.PlayerDamageMid += 3;
            PlayerData.Instance.PlayerDamageLight += 3;
            _buffApplied[0] = 1;
        };

        _blessingEffects["Stalwart"] = () =>
        {
            PlayerData.Instance.PlayerBlock += 3;
            _buffApplied[1] = 1;
        };

        _blessingEffects["Mars's Pulse"] = () =>
        {
            PlayerData.Instance.IncreaseStamina(1);
        };

        _blessingEffects["Raven's Claws"] = () =>
        {
            PlayerData.Instance.PlayerDamageHeavy += 6;
            PlayerData.Instance.PlayerDamageMid += 6;
            PlayerData.Instance.PlayerDamageLight += 6;
            _buffApplied[2] = 1;
        };

        _blessingEffects["Vampiric Feast"] = () =>
        {
            PlayerData.Instance.Heal(5);
            // TODO: deal 5 damage to enemy
            _buffApplied[3] = 1;
        };

        _blessingEffects["Strategic Knowledge"] = () =>
        {
            PlayerData.Instance.PlayerBlock += 6;
            _buffApplied[4] = 1;
        };

        _blessingEffects["Eir's Blessing"] = () =>
        {
            PlayerData.Instance.Heal(8);
            _buffApplied[5] = 1;
        };

        _blessingEffects["Quick Reflexes"] = () =>
        {
            // TODO: clear debuffs
            _buffApplied[6] = 1;
        };

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
    
    //dictionary to hold buff applying logic
    private readonly Dictionary<string, Action> _blessingEffects = new();

    public void TurnReset()
    {
        ActionsLeft = 3;
        PlayerData.Instance.ResetStamina();

        for (var i = 0; i < _buffApplied.Count; i++)
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

    //function to apply the stored player blessing
    public void ApplyBlessing()
    {
        var blessingName = PlayerData.Instance.StoredBuff;
        if (_blessingEffects.TryGetValue(blessingName, out var applyEffect))
        {
            PlayerData.Instance.StoredBuff = "None";
            applyEffect.Invoke();
        }
    }

}