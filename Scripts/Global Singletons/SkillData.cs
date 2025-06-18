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
        SkillLibrary["Slash"] = new Skill //slot 1
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

        SkillLibrary["Thrust"] = new Skill //slot 2
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

        SkillLibrary["Light Block"] = new Skill //slot 3
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

        SkillLibrary["Prayer"] = new Skill //slot 4
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

        SkillLibrary["Whirlwind"] = new Skill //slot 5
        {
            Name = "Whirlwind",
            StaminaCost = 3,
            ActionCost = 1,
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
        SkillLibrary["Vampiric Strike"] = new Skill //replaces light block (slot 3)
        {
            Name = "Vampiric Strike",
            StaminaCost = 1,
            ActionCost = 1,
            SkillType = SkillType.Damage,
            ForClassType = PlayerClassType.Berserker,

            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Vampiric Strike"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('a');
                return new SkillResult()
                {
                    Damage = PlayerData.Instance.PlayerDamageMid,
                    Healing = PlayerData.Instance.PlayerDamageMid
                    //apply 'blood fatigue' debuff to enemy if active attack chain
                };
            }
        };
        SkillLibrary["Heavy Block"] = new Skill //replaces thrust (slot 2)
        {
            Name = "Heavy Block",
            StaminaCost = 2,
            ActionCost = 1,
            SkillType = SkillType.Shield,
            ForClassType = PlayerClassType.Warder,

            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Heavy Block"].StaminaCost);
                CombatManager.Instance?.IncrementDefendCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('d');
                return new SkillResult()
                {
                    ShieldValue = 12
                    //apply the 'disarm' debuff to enemy if active defend chain
                };
            }
        };
        SkillLibrary["Rage Slash"] = new Skill // replaces thrust (slot 2)
        {
            Name = "Rage Slash",
            StaminaCost = 2,
            ActionCost = 1,
            SkillType = SkillType.Aoe,
            ForClassType = PlayerClassType.Berserker,

            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Rage Slash"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('a');
                return new SkillResult()
                {
                    Damage = PlayerData.Instance.PlayerDamageLight
                };
            }
        };
        SkillLibrary["Skull Splitter"] = new Skill //replaces whirlwind (slot 5)
        {
            Name = "Skull Splitter",
            StaminaCost = 3,
            ActionCost = 1,
            SkillType = SkillType.Ultimate,
            ForClassType = PlayerClassType.Berserker,

            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Skull Splitter"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('a');
                return new SkillResult()
                {
                    Damage = PlayerData.Instance.PlayerDamageHeavy,
                    Healing = PlayerData.Instance.PlayerDamageHeavy
                    //apply extra damage and healing if enemy has bloodfatigue
                };
            }
        };
        SkillLibrary["Deflect Slash"] = new Skill //replaces slash (slot 1)
        {
            Name = "Deflect Slash",
            StaminaCost = 1,
            ActionCost = 1,
            SkillType = SkillType.Debuff,
            ForClassType = PlayerClassType.Warder,
            
            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Deflect Slash"].StaminaCost);
                CombatManager.Instance?.IncrementDefendCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('d');
                return new SkillResult()
                {
                    ShieldValue = PlayerData.Instance.PlayerDamageLight / 3
                    //need to write logic to reflect half of the damage onto the enemy
                    //this can be done by simply applying a debuff to the enemy that detonates upon it attacking
                    //maybe include a Debuff string value in SkillResult?
                };
            }
        };
        SkillLibrary["Immovable Judgement"] = new Skill //replaces whirlwind (slot 5)
        {
            Name = "Immovable Judgement",
            StaminaCost = 3,
            ActionCost = 1,
            SkillType = SkillType.Ultimate,
            ForClassType = PlayerClassType.Warder,

            Execute = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Immovable Judgement"].StaminaCost);
                CombatManager.Instance.IncrementDefendCounter();
                DevotionTree.Instance?.IncrementSelectedPoints('d');
                return new SkillResult()
                {
                    Damage = PlayerData.Instance.PlayerDamageHeavy
                    //stun enemies that have disarm debuff
                    //gain massive (50+) one turn shield
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