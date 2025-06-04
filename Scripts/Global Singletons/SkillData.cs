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
    
    public class Skill
    {
        public string Name;
        public int StaminaCost;
        public int ActionCost;
        public int ShieldValue;
        public bool IsBuff = false;
        public Func<int>? GetDamage;
        public Action? ExecuteEffect;
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

    public Prayers CurrentPrayer { get; set; }

    private bool _prayerChanged = false;
    
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
    private void InitializeSkills()
    {
        SkillLibrary["Slash"] = new Skill
        {
            Name = "Slash",
            StaminaCost = 1,
            ActionCost = 1,
            ShieldValue = 0,
            GetDamage = () => PlayerData.Instance.PlayerDamageLight,
            ExecuteEffect = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Slash"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
            }
        };

        SkillLibrary["Thrust"] = new Skill
        {
            Name = "Thrust",
            StaminaCost = 2,
            ActionCost = 1,
            GetDamage = () => PlayerData.Instance.PlayerDamageMid,
            ShieldValue = 0,
            ExecuteEffect = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Thrust"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
            }
        };

        SkillLibrary["Light Block"] = new Skill
        {
            Name = "Light Block",
            StaminaCost = 1,
            ActionCost = 1,
            GetDamage = () => PlayerData.Instance.ZeroDamage,
            ShieldValue = 5,
            ExecuteEffect = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Light Block"].StaminaCost);
                PlayerData.Instance.IncreaseBlock(SkillLibrary["Light Block"].ShieldValue);
                CombatManager.Instance?.IncrementDefendCounter();
            }
        };

        SkillLibrary["Prayer"] = new Skill
        {
            Name = "Prayer",
            ActionCost = 2,
            StaminaCost = 0,
            GetDamage = () => PlayerData.Instance.ZeroDamage,
            ShieldValue = 0,
            IsBuff = true,
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
            GetDamage = () => PlayerData.Instance.PlayerDamageHeavy,
            ShieldValue = 0,
            ExecuteEffect = () =>
            {
                PlayerData.Instance.DecreaseStamina(SkillLibrary["Whirlwind"].StaminaCost);
                CombatManager.Instance?.IncrementAttackCounter();
            }
        };
    }

    private void GiveGodsBuff()
    {  
        if (PlayerData.Instance.GetPlayerClass() == "Berserker" && !_prayerChanged)
        {
            CurrentPrayer = Prayers.Mars;
            _prayerChanged = true;
        }
        else if (PlayerData.Instance.GetPlayerClass() == "Warder" && !_prayerChanged)
        {
            CurrentPrayer = Prayers.Anicetus;
            _prayerChanged = true;
        }
        else if (!_prayerChanged)
        {
            CurrentPrayer = Prayers.UnnamedAttackGod;
        }
        if (_prayerCycle.TryGetValue(CurrentPrayer, out var next))
        {
            CurrentPrayer = next.Next;
            PlayerData.Instance.StoredBuff = next.Buff;
        }
    }


}