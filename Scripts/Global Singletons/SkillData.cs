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

        if (PlayerData.Instance != null)
            InitializeSkills();
            
    }
    
    public PrayerType CurrentPrayer { get; set; } = PrayerType.UnnamedAttackGod;

    private bool _prayerChanged = false;
    
    public Dictionary<string, Skill> SkillLibrary = new();
    
    //initialize and hold skills in SkillLibrary dictionary
    private void InitializeSkills()
    {
        SkillLibrary["Slash"] = new Skill
        {
            Name = "Slash",
            StaminaCost = 1,
            ActionCost = 1,
            SkillType = SkillType.Damage,
            Execute = () => {
                PlayerData.Instance.DecreaseStamina(1);
                CombatManager.Instance?.IncrementAttackCounter();
                return new SkillResult { Damage = PlayerData.Instance.PlayerDamageLight };
            }
        };

        SkillLibrary["Thrust"] = new Skill
        {
            Name = "Thrust",
            StaminaCost = 2,
            ActionCost = 1,
            SkillType = SkillType.Damage,
            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Thrust"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
                return new SkillResult { Damage = PlayerData.Instance.PlayerDamageMid };
            }
        };

        SkillLibrary["Light Block"] = new Skill
        {
            Name = "Light Block",
            StaminaCost = 1,
            ActionCost = 1,
            SkillType = SkillType.Shield,
            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Light Block"].StaminaCost);
                CombatManager.Instance?.IncrementDefendCounter();
                return new SkillResult
                {
                    Damage = 0,
                    ShieldValue = 5,
                    Healing = 0
                };
            }
        };

        SkillLibrary["Prayer"] = new Skill
        {
            Name = "Prayer",
            ActionCost = 2,
            StaminaCost = 0,
            SkillType = SkillType.Buff,
            Execute = () =>
            {
                CombatManager.Instance?.IncrementPrayerCounter();
                if (PlayerData.Instance != null)
                {
                    GiveGodsBuff();
                }
                return new SkillResult
                {
                    Damage = 0,
                    ShieldValue = 0,
                    Healing = 0
                };
            }
        };

        SkillLibrary["Whirlwind"] = new Skill
        {
            Name = "Whirlwind",
            StaminaCost = 3,
            ActionCost = 2,
            SkillType = SkillType.Aoe,
            
            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Whirlwind"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
                return new SkillResult()
                {
                    Damage = PlayerData.Instance.PlayerDamageHeavy,
                    ShieldValue = 0,
                    Healing = 0
                };
            }
        };
    }

    private void GiveGodsBuff()
    {  
        if (PlayerData.Instance.GetPlayerClass() == "Berserker" && !_prayerChanged)
        {
            CurrentPrayer = PrayerType.Mars;
            _prayerChanged = true;
        }
        else if (PlayerData.Instance.GetPlayerClass() == "Warder" && !_prayerChanged)
        {
            CurrentPrayer = PrayerType.Anicetus;
            _prayerChanged = true;
        }
        
        if (PrayerData.PrayerCycle.TryGetValue(CurrentPrayer, out var next))
        {
            CurrentPrayer = next.Next;
            PlayerData.Instance.StoredBuff = next.Buff;
        }
    }


}