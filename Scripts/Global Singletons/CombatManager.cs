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

        _buffApplied = new() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}; //used to track if a buff was applied from prayer or chains
        _blessingEffects["Invigorated"] = () =>
        {
            PlayerData.Instance.DamageBonus += 3;
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
            PlayerData.Instance.DamageBonus += 6;
            _buffApplied[2] = 1;
        };

        _blessingEffects["Vampiric Feast"] = () =>
        {
            PlayerData.Instance.Heal(PlayerData.Instance.PlayerDamageLight + PlayerData.Instance.DamageBonus);
            _currentTarget.DecreaseHealth(PlayerData.Instance.PlayerDamageLight + PlayerData.Instance.DamageBonus);
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
            foreach (var count in PlayerData.Instance.Debuffs.Keys)
            {
                PlayerData.Instance.Debuffs[count] = 0;
            }
            _buffApplied[6] = 1;
        };

    }
    //combat entities
    private List<EnemyEntity> _enemies = new();
    private EnemyEntity _currentTarget;
    public PlayerEntity Player;
    
    //bool to track button access
    public bool ButtonLock = true;
    public bool VictoryButtonLock = true;
    
    //Combat variables to track
    private int ActionsLeft = 3;
    private int AttackCounter = 0;
    private int DefendCounter = 0;
    private int PrayCounter = 0;
    public int EnemiesDefeated = 0;
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
    
    public void AddEnemyToBattle(EnemyEntity enemy)
    {
        _enemies.Add(enemy);
        if (_currentTarget == null)
            _currentTarget = enemy; // Default first target
    }
    
    public void SetTarget(EnemyEntity enemy)
    {
        _currentTarget = enemy;

        foreach (var e in _enemies)
            e.Modulate = new Color(1, 1, 1); // reset to normal

        enemy.Modulate = new Color(1.5f, 1.2f, 1.2f); // highlight
        GD.Print($"Target set to: {enemy.GetName()}");
    }
    
    public void StartBattle(PlayerEntity player)
    {
        Player = player;
        PlayerData.Instance.PlayerBlock = 0;

        if (_enemies.Count > 0)
            SetTarget(_enemies[0]); // Always pick first enemy for new combat

        _currentBattleState = BattleState.PlayerTurn;
        StartPlayerTurn();
    }
    
    public async void StartPlayerTurn()
    {
        _currentBattleState = BattleState.PlayerTurn;

        foreach (var runningDebuff in PlayerData.Instance.Debuffs.Keys)
        {
            if (Player.CheckDot(runningDebuff))
            {
                if (DebuffData.Instance.DebuffLibrary[runningDebuff].Damage > 0)
                {
                    Player.PlayAnimationHurt();
                    await ToSignal(GetTree().CreateTimer(1), "timeout");
                    Player.PlayAnimationStand();
                    Player.ApplyDebuffDamage(DebuffData.Instance.DebuffLibrary[runningDebuff].Damage);
                }
            }
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
        EmitSignalActionUsed();
    }
    
    public void ExecutePlayerSkill(Skill skill)
    {
        GD.Print($"Trying to execute skill: {skill.Name}");

        var success = UseSkill(skill.Name);
        if (!success) return;

        PlaySkillAnimation(skill.Name); // animation helper
    }

    private void HandleEnemyDefeat(EnemyEntity enemy)
    {
        _enemies.Remove(enemy);
        enemy.QueueFree();
        EnemiesDefeated++;
    }

    private void EndCombatVictory()
    {
        GD.Print("All enemies defeated! Victory!");
        _currentBattleState = BattleState.Win;
        DungeonCompletionManager.Instance.DungeonDefeated = true;
        ButtonLock = true;
        VictoryButtonLock = false;
        EmitSignalBattleStateChanged();
    }

    public async void StartEnemyTurn()
    {
        _currentBattleState = BattleState.EnemyTurn;
        ButtonLock = true;
        EmitSignalBattleStateChanged();
        

        foreach (var e in _enemies)
        {
            if (Player.IsDead())
            {
                _currentBattleState = BattleState.Lose;
                EmitSignalBattleStateChanged();
                return;
            }

            var skills = e.GetSkills();
            var rng = new RandomNumberGenerator();
            rng.Randomize();
            var chosenSkill = skills[rng.RandiRange(0, skills.Count - 1)];

            GD.Print($"{e.Name} uses: {chosenSkill.Name}");

            if (chosenSkill.IsBuff)
            {
                GD.Print("Enemy casts a buff.");
            }

            if (chosenSkill.DebuffType == DebuffType.Bleed) //make this into a function that chooses the correct debuff
            {
                Player.ApplyDebuff(DebuffType.Bleed);
            }

            if (chosenSkill.Damage > 0)
            {
                e.PlayAnimationAttack();
                await ToSignal(GetTree().CreateTimer(1), "timeout");
                e.PlayAnimationStand();

                Player.PlayAnimationHurt();
                await ToSignal(GetTree().CreateTimer(1), "timeout");
                Player.PlayAnimationStand();

                Player.DecreaseHealth(chosenSkill.Damage);
            }

            if (chosenSkill.ShieldValue > 0)
            {
                GD.Print("Enemy gains shield.");
                e.AddShield(chosenSkill.ShieldValue);
            }

            await ToSignal(GetTree().CreateTimer(1), "timeout");
        }

        // After all enemies act, return to player's turn
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

    private void TurnReset()
    {
        Player.ResetGlow();
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

            if (i == 8 && _buffApplied[i] == 1)
            {
                PlayerData.Instance.DamageBonus = 0;
            }

            if (i == 9 && _buffApplied[i] == 1)
            {
                PlayerData.Instance.BlockBonus = 0;
            }
            
            _buffApplied[i] = 0;
        }
        ApplyChain();
        PlayerData.Instance.ResetStamina(); //reset stamina after buff falls off, so labels are properly updated
        
    }

    private void ApplyChain()
    {
        if (AttackChain)
        {
            PlayerData.Instance.DamageBonus = 5;
            _buffApplied[8] = 1;
            AttackCounter = 0;
            Player.SetGlowColor(new Color(1.0f, 0.4f, 0.4f)); // warm red glow
            GD.Print("Applied chain. Buff: Damage + 5");
        }

        else if (DefendChain)
        {
            PlayerData.Instance.BlockBonus = 5;
            _buffApplied[9] = 1;
            DefendCounter = 0;
            Player.SetGlowColor(new Color(0.4f, 0.4f, 1.0f)); // cool blue glow
            GD.Print("Applied chain. Buff: Defense + 5");
        }
    }
    public void IncrementAttackCounter()
    {
        AttackCounter++;
        DefendCounter = 0;
        DefendChain = false;
        AttackChain = AttackCounter >= 3;
    }

    public void IncrementDefendCounter()
    {
        DefendCounter++;
        AttackCounter = 0;
        AttackChain = false;
        DefendChain = DefendCounter >= 3;
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

        var result = skill.Execute?.Invoke();
        if (result != null)
            ApplySkillResult(result);

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
    
    private async void ApplySkillResult(SkillResult result)
    {
        if (result.Damage > 0)
        {
            var target = _currentTarget;

            if (target != null && !target.IsQueuedForDeletion())
            {
                target.PlayAnimationHurt();
                await ToSignal(GetTree().CreateTimer(1f), "timeout");

                if (target == null || target.IsQueuedForDeletion())
                    return;

                target.PlayAnimationStand();
                target.DecreaseHealth(result.Damage + PlayerData.Instance.DamageBonus);

                if (target.IsDead())
                {
                    HandleEnemyDefeat(target);

                    if (_enemies.Count > 0)
                        SetTarget(_enemies[0]);
                    else
                        EndCombatVictory();
                }
            }
        }

        if (result.Healing > 0)
            PlayerData.Instance.Heal(result.Healing);

        if (result.ShieldValue > 0)
            PlayerData.Instance.IncreaseBlock(result.ShieldValue + PlayerData.Instance.BlockBonus);

        if (result.Buff != null)
            GD.Print($"Buff applied: {result.Buff}");
        if (result.AppliedDebuff != DebuffType.None)
        {
            PlayerData.Instance.ApplyDebuff(result.AppliedDebuff);
        }
    }

    public async void PlaySkillAnimation(string name)
    {
        switch (name)
        {
            case "Slash":
                Player.PlayAnimationSlash();
                break;
            case "Thrust":
                Player.PlayAnimationThrust();
                break;
            case "Light Block":
                Player.PlayAnimationBlock();
                break;
            case "Prayer":
                Player.PlayAnimationPrayer();
                break;
            case "Whirlwind":
                Player.PlayAnimationWhirlwind();
                break;
        }
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        Player.PlayAnimationStand();
    }
}