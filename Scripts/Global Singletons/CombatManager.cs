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
            Enemy.DecreaseHealth(5);
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
            PlayerData.Instance.PlayerDotLeft = 0;
            PlayerData.Instance.DotDamageTotal = 0;
            _buffApplied[6] = 1;
        };

    }
    //combat entities
    public EnemyEntity Enemy;
    public PlayerEntity Player;
    
    //bool to track button access
    public bool ButtonLock = true;
    public bool VictoryButtonLock = true;
    
    //Combat variables to track
    private int ActionsLeft = 3;
    private int AttackCounter = 0;
    private int DefendCounter = 0;
    private int PrayCounter = 0;
    public bool AttackChain;
    public bool DefendChain;
    public bool PrayerChain;
    private List<int> _buffApplied;
    
    
    //turn tracker
    private enum BattleState { PlayerTurn, EnemyTurn, Win, Lose }
    private BattleState _currentBattleState;
    
    //signal to track turn changes 
    [Signal] public delegate void BattleStateChangedEventHandler();
    
    //signal to track action use
    [Signal] public delegate void ActionUsedEventHandler();
    
    //dictionary to hold buff applying logic
    private readonly Dictionary<string, Action> _blessingEffects = new();
    
    public void StartBattle(PlayerEntity player, EnemyEntity enemy)
    {
        Player = player;
        Enemy = enemy;

        _currentBattleState = BattleState.PlayerTurn;
        StartPlayerTurn();
    }
    
    public async void StartPlayerTurn()
    {
        _currentBattleState = BattleState.PlayerTurn;

        if (Player.CheckDot())
        {
            Player.PlayAnimationHurt();
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            Player.PlayAnimationStand();
            Player.ApplyDotDamage();
        }
        if (Player.IsDead())
        {
            _currentBattleState = BattleState.Lose;
            EmitSignalBattleStateChanged();
            return;
        }
        TurnReset();
        ButtonLock = false;
        EmitSignalBattleStateChanged();
        
    }
    
    public async void ExecutePlayerSkill(SkillData.Skill skill)
    {
        GD.Print($"Trying to execute skill: {skill.Name}");
        var success = UseSkill(skill.Name); // Already handles stamina & damage
        if (!success) return;

        // Play animations based on skill
        if (skill.Name == "Slash")
        {
            Player.PlayAnimationSlash();
        }
        else if (skill.Name == "Thrust")
        {
            //Player.PlayAnimationThrust();
        }
        else if (skill.Name == "Light Block")
        {
            //Player.PlayAnimationThrust();
        }
        else if (skill.Name == "Prayer")
        {
            //Player.PlayAnimationPrayer();
        }
        else if (skill.Name == "Whirlwind")
        {
            //Player.PlayAnimationWhirlwind()
        }

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        Player.PlayAnimationStand();

        if (skill.GetDamage != null && skill.GetDamage() > 0)
        {
            Enemy.PlayAnimationHurt();
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            Enemy.PlayAnimationStand();
            Enemy.DecreaseHealth(skill.GetDamage());
        }
        if (Enemy.IsDead())
        {
            _currentBattleState = BattleState.Win;
            VictoryButtonLock = false;
            ButtonLock = true;
            EmitSignalBattleStateChanged();
            return;
        }

        await ToSignal(GetTree().CreateTimer(1), "timeout");
    }
    
    public async void StartEnemyTurn()
    {
        _currentBattleState = BattleState.EnemyTurn;
        EmitSignalBattleStateChanged();

        // Get enemy skills
        var skills = Enemy.GetSkills();
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        // Choose a random skill
        var chosenSkill = skills[rng.RandiRange(0, skills.Count - 1)];

        GD.Print($"Enemy uses: {chosenSkill.Name}");
        

        // Handle skill effects
        if (chosenSkill.IsBuff)
        {
            GD.Print("Enemy casts a buff.");
            // Apply shield or other effects here
            // For now, let's just print that something happened
        }

        if (chosenSkill.IsDamageOverTime)
        {
            Player.ApplyDot(chosenSkill.DotCounter, chosenSkill.Damage);
        }
        
        if (chosenSkill.Damage > 0)
        {
            // Attack animation (generic for now)
            Enemy.PlayAnimationAttack();
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            Enemy.PlayAnimationStand();
            
            Player.PlayAnimationHurt();
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            Player.PlayAnimationStand();

            Player.DecreaseHealth(chosenSkill.Damage);
        }
        
        if (chosenSkill.ShieldValue > 0)
        {
            GD.Print("Enemy gains shield.");
            Enemy.AddShield(chosenSkill.ShieldValue);
        }

        if (Player.IsDead())
        {
            _currentBattleState = BattleState.Lose;
            return;
        }

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        StartPlayerTurn();
    }

    public void EndPlayerTurn()
    {
        ButtonLock = true;
        StartEnemyTurn();
    }

    public string GetBattleState()
    {
        return _currentBattleState.ToString();
    }

    public int GetActionsLeft()
    {
        return ActionsLeft;
    }

    public void TurnReset()
    {
        ActionsLeft = 3;

        for (var i = 0; i < _buffApplied.Count; i++)
        {
            if (i == 0 && _buffApplied[i] == 1)
            {
                PlayerData.Instance.PlayerDamageLight -= 3;
                PlayerData.Instance.PlayerDamageMid -= 3;
                PlayerData.Instance.PlayerDamageHeavy -= 3;
            }

            if (i == 1 && _buffApplied[i] == 1)
            {
                PlayerData.Instance.PlayerBlock -= 3;
            }

            if (i == 2 && _buffApplied[i] == 1)
            {
                PlayerData.Instance.PlayerDamageLight -= 6;
                PlayerData.Instance.PlayerDamageMid -= 6;
                PlayerData.Instance.PlayerDamageHeavy -= 6;
            }
            
            _buffApplied[i] = 0;
        }
        PlayerData.Instance.ResetStamina(); //reset stamina after buff falls off, so labels are properly updated
        
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
        GD.Print($"Stamina: {PlayerData.Instance.GetPlayerStamina()}, Action: {ActionsLeft}");
        if (!SkillData.Instance.SkillLibrary.TryGetValue(skillName, out var skill))
        {
            GD.PrintErr($"Skill '{skillName}' not found in SkillLibrary.");
            return false;
        }

        if (ActionsLeft < skill.ActionCost)
        {
            GD.PrintErr("Not enough actions to use skill.");
            return false;
        }

        if (PlayerData.Instance.GetPlayerStamina() < skill.StaminaCost)
        {
            GD.PrintErr("Not enough stamina to use skill.");
            return false;
        }

        ActionsLeft -= skill.ActionCost;
        EmitSignalActionUsed();
        skill.ExecuteEffect?.Invoke();
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