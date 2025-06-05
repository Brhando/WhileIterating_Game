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
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _hpLabel = GetNode<Label>("AnimatedSprite2D/Panel/HPLabel");
        _blockLabel = GetNode<Label>("AnimatedSprite2D/Panel/BlockLabel");
        
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        
        var randomIndex = rng.RandiRange(0, EnemyManager.Instance.Enemies.Count - 1);
        var templateEnemy = EnemyManager.Instance.Enemies[randomIndex];
        _enemy = templateEnemy.Clone(); // gives a fresh instance of the enemy, so health doesn't persist between dungeon levels
        
        switch (_enemy.Name)
        {
            case "Goblin": _anim.SpriteFrames = GoblinFrames; break;
            case "Slime": _anim.SpriteFrames = SlimeFrames; break;
            case "Skeleton": _anim.SpriteFrames = SkeletonFrames; break;
        }
        
        GD.Print("Enemy Selected: " + _enemy.Name);
        GD.Print("Assigned SpriteFrames: " + _anim.SpriteFrames);
        
        PlayAnimationStand();
        UpdateLabels();
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
}
