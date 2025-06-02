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
    
    //dictionary to hold cycling logic
    private readonly Dictionary<Prayers, (Prayers Next, string Buff)> _prayerCycle = new()
    {
        { Prayers.UnnamedAttackGod, (Prayers.UnnamedDefenseGod, "Invigorated") },
        { Prayers.UnnamedDefenseGod, (Prayers.UnnamedAttackGod, "Stalwart") },
        { Prayers.Mars, (Prayers.Odin, "Mars's Pulse") },
        { Prayers.Odin, (Prayers.Montu, "Raven's Claws") },
        { Prayers.Montu, (Prayers.Mars, "Vampiric Feast") },
        { Prayers.Anicetus, (Prayers.Eir, "Strategic Knowledge") },
        { Prayers.Eir, (Prayers.Bastet, "Eir's Blessing") },
        { Prayers.Bastet, (Prayers.Anicetus, "Quick Reflexes") }
    };
    
    public Dictionary<string, Skill> SkillLibrary = new();
    
    //initialize and hold skills in SkillLibrary dictionary
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
            Name = "Prayer",
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
            ActionCost = 2,
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
        if (_prayerCycle.TryGetValue(CurrentPrayer, out var next))
        {
            CurrentPrayer = next.Next;
            PlayerData.Instance.StoredBuff = next.Buff;
        }
    }


}