using Godot;
using System.Collections.Generic;

public partial class EnemyEntity : CharacterBody2D
{
    private AnimatedSprite2D _anim;
    private EnemyManager.Enemy _enemy;
    private Label _hpLabel;
    private Label _blockLabel;
    
    [Export] public SpriteFrames GoblinFrames { get; set; }
    [Export] public SpriteFrames SlimeFrames { get; set; }
    [Export] public SpriteFrames SkeletonFrames { get; set; }


    public override void _Ready()
    {
        //nothing here for now; handled by Initialize() func
    }
    [Signal] public delegate void EnemyClickedEventHandler(EnemyEntity self);
    
    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            EmitSignal(nameof(EnemyClicked), this);
        }
    }
    public void Initialize(EnemyManager.Enemy enemy)
    {
        _enemy = enemy;

        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _hpLabel = GetNode<Label>("AnimatedSprite2D/Panel/HPLabel");
        _blockLabel = GetNode<Label>("AnimatedSprite2D/Panel/BlockLabel");

        switch (_enemy.Name)
        {
            case "Goblin": _anim.SpriteFrames = GoblinFrames; break;
            case "Slime": _anim.SpriteFrames = SlimeFrames; break;
            case "Skeleton": _anim.SpriteFrames = SkeletonFrames; break;
        }

        PlayAnimationStand();
        UpdateLabels();
    }

    public bool CheckForFatigue()
    {
        if (_enemy.Debuffs.ContainsKey(DebuffType.BloodFatigue))
        {
            _enemy.DecrementDebuff(DebuffType.BloodFatigue);
            return true;
        }
        return false;
    }
    public void AddOrIncrementDebuff(DebuffType appliedDebuff)
    {
        _enemy.AddOrIncrementDebuff(appliedDebuff);
    }

    public void TickAllDebuffsDown()
    {
        _enemy.TickAllDebuffsDown();
    }

    public void CheckDebuffs()
    {
        //check debuffs and apply visual cues
        //until I have icons we will just add a glow for blood fatigue and disarm
        if (_enemy.Debuffs.ContainsKey(DebuffType.BloodFatigue))
            SetGlowColor(Colors.OrangeRed);
        //else if (_enemy.Debuffs.ContainsKey(DebuffType.Disarm)) {SetGlowColor(Colors.Purple)}
        else
        {
            ResetGlow();
        }
    }

    //function to decrease enemy's health when attacked
    public void DecreaseHealth(int amt)
    {
        _enemy.TakeDamage(amt);
        UpdateLabels();
    }

    public void AddShield(int amount)
    {
        _enemy.AddShield(amount);
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        _hpLabel.Text = _enemy.Health.ToString();
        _blockLabel.Text = _enemy.Shield.ToString();
    }

    public bool IsDead()
    {
        return _enemy.IsDead();
    }
    
    public int GetHealth()
    {
        return _enemy.Health;
    }

    public int GetMaxHealth()
    {
        return _enemy.MaxHealth;
    }
    
    public List<EnemyManager.EnemySkill> GetSkills()
    {
        return _enemy.GetSkills(); //linked to EnemyManager
    }
    
    public void PlayAnimationAttack()
    {
        _anim.Play("Attack");
    }

    public void PlayAnimationStand()
    {
        _anim.Play("Stand");
    }

    public void PlayAnimationHurt()
    {
        _anim.Play("Hurt");
    }
    
    public void SetGlowColor(Color color)
    {
        _anim.Modulate = color;
    }

    public void ResetGlow()
    {
        _anim.Modulate = Colors.White;
    }
}
