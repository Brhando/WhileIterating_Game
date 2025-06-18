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
            ForClassType = PlayerClassType.SwordSwinger,
            Execute = () => {
                PlayerData.Instance.DecreaseStamina(1);
                CombatManager.Instance?.IncrementAttackCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('a');
                return new SkillResult { Damage = PlayerData.Instance.PlayerDamageLight };
            }
        };

        SkillLibrary["Thrust"] = new Skill
        {
            Name = "Thrust",
            StaminaCost = 2,
            ActionCost = 1,
            SkillType = SkillType.Damage,
            ForClassType = PlayerClassType.SwordSwinger,
            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Thrust"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('a');
                return new SkillResult { Damage = PlayerData.Instance.PlayerDamageMid };
            }
        };

        SkillLibrary["Light Block"] = new Skill
        {
            Name = "Light Block",
            StaminaCost = 1,
            ActionCost = 1,
            SkillType = SkillType.Shield,
            ForClassType = PlayerClassType.SwordSwinger,
            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Light Block"].StaminaCost);
                CombatManager.Instance?.IncrementDefendCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('d');
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
            ForClassType = PlayerClassType.All,
            Execute = () =>
            {
                CombatManager.Instance?.IncrementPrayerCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('p');
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
            ForClassType = PlayerClassType.SwordSwinger,
            
            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Whirlwind"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('a');
                return new SkillResult()
                {
                    Damage = PlayerData.Instance.PlayerDamageHeavy,
                    ShieldValue = 0,
                    Healing = 0
                };
            }
        };
        SkillLibrary["Vampiric Strike"] = new Skill
        {
            Name = "Vampiric Strike",
            StaminaCost = 2,
            ActionCost = 1,
            SkillType = SkillType.Damage,
            ForClassType = PlayerClassType.Berserker,
            DevotionReq = 15,

            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Vampiric Strike"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('a');
                return new SkillResult()
                {
                    Damage = PlayerData.Instance.PlayerDamageMid,
                    Healing = PlayerData.Instance.PlayerDamageMid
                };
            }
        };
        SkillLibrary["Heavy Block"] = new Skill
        {
            Name = "Heavy Block",
            StaminaCost = 2,
            ActionCost = 1,
            SkillType = SkillType.Shield,
            ForClassType = PlayerClassType.Warder,
            DevotionReq = 15,

            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Heavy Block"].StaminaCost);
                CombatManager.Instance?.IncrementDefendCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('d');
                return new SkillResult()
                {
                    ShieldValue = 12
                };
            }
        };
    }

    private void GiveGodsBuff()
    {  
        if (PlayerClassManager.Instance.GetClassName() == "Berserker" && !_prayerChanged)
        {
            CurrentPrayer = PrayerType.Mars;
            _prayerChanged = true;
        }
        else if (PlayerClassManager.Instance.GetClassName() == "Warder" && !_prayerChanged)
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