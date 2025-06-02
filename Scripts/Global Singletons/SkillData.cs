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
        
        InitializeSkills();
    }
    
    public class Skill
    {
        public string Name;
        public int StaminaCost;
        public int ActionCost;
        public int Damage;
        public int ShieldValue;
        public Action ExecuteEffect;
    }

    public enum Prayers
    {
        UnnamedAttackGod,
        UnnamedDefenseGod,
        Mars,
        Odin,
        Montu,
        Anicetus,
        Eir,
        Bastet
    }

    public Prayers CurrentPrayer { get; set; } = Prayers.UnnamedAttackGod;
    public Dictionary<string, Skill> SkillLibrary = new();

    public void InitializeSkills()
    {
        SkillLibrary["Slash"] = new Skill
        {
            Name = "Slash",
            StaminaCost = 1,
            ActionCost = 1,
            Damage = 5,
            ShieldValue = 0,
            ExecuteEffect = () =>
            {
                PlayerData.Instance?.DecreaseStamina(1);
                CombatManager.Instance?.IncrementAttackCounter();
            }
        };

        SkillLibrary["Thrust"] = new Skill
        {
            Name = "Thrust",
            StaminaCost = 2,
            ActionCost = 1,
            Damage = 10,
            ShieldValue = 0,
            ExecuteEffect = () =>
            {
                PlayerData.Instance?.DecreaseStamina(2);
                CombatManager.Instance?.IncrementAttackCounter();
            }
        };

        SkillLibrary["Light Block"] = new Skill
        {
            Name = "Light Block",
            StaminaCost = 1,
            ActionCost = 1,
            Damage = 0,
            ShieldValue = 5,
            ExecuteEffect = () =>
            {
                PlayerData.Instance?.DecreaseStamina(1);
                CombatManager.Instance?.IncrementDefendCounter();
            }
        };

        SkillLibrary["Prayer"] = new Skill
        {
            ActionCost = 2,
            StaminaCost = 0,
            Damage = 0,
            ShieldValue = 0,
            ExecuteEffect = () =>
            {
                CombatManager.Instance?.IncrementPrayerCounter();
                if (PlayerData.Instance != null)
                {
                    GiveGodsBuff();
                }
            }
        };

        SkillLibrary["Whirlwind"] = new Skill
        {
            Name = "Whirlwind",
            StaminaCost = 3,
            ActionCost = 3,
            Damage = 20,
            ShieldValue = 0,
            ExecuteEffect = () =>
            {
                PlayerData.Instance?.DecreaseStamina(3);
                CombatManager.Instance?.IncrementDefendCounter();
            }
        };
    }

    private void GiveGodsBuff()
    {
        if (CurrentPrayer == Prayers.UnnamedAttackGod)
        {
            CurrentPrayer = Prayers.UnnamedDefenseGod;
            PlayerData.Instance.StoredBuff = "Invigorated"; //small attack boost
        }

        if (CurrentPrayer == Prayers.UnnamedDefenseGod)
        {
            CurrentPrayer = Prayers.UnnamedAttackGod;
            PlayerData.Instance.StoredBuff = "Stalwart"; // small defense boost
        }

        if (CurrentPrayer == Prayers.Mars)
        {
            CurrentPrayer = Prayers.Odin;
            PlayerData.Instance.StoredBuff = "Mars's Pulse"; // stamina boost
        }

        if (CurrentPrayer == Prayers.Odin)
        {
            CurrentPrayer = Prayers.Montu;
            PlayerData.Instance.StoredBuff = "Raven's Claws"; //moderate attack boost
        }

        if (CurrentPrayer == Prayers.Montu)
        {
            CurrentPrayer = Prayers.Mars;
            PlayerData.Instance.StoredBuff = "Vampiric Feast"; //health steal
        }

        if (CurrentPrayer == Prayers.Anicetus)
        {
            CurrentPrayer = Prayers.Eir;
            PlayerData.Instance.StoredBuff = "Strategic Knowledge"; //moderate defense boost
        }

        if (CurrentPrayer == Prayers.Eir)
        {
            CurrentPrayer = Prayers.Bastet;
            PlayerData.Instance.StoredBuff = "Eir's Blessing"; //medium heal
        }

        if (CurrentPrayer == Prayers.Bastet)
        {
            CurrentPrayer = Prayers.Anicetus;
            PlayerData.Instance.StoredBuff = "Quick Reflexes"; //debuff cleanse
        }
    }
    
    
}