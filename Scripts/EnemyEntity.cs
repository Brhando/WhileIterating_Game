using Godot;
using System;

public partial class EnemyEntity : CharacterBody2D
{
    private AnimatedSprite2D _anim;
    private EnemyManager.Enemy _enemy;
    
    [Export] public SpriteFrames GoblinFrames { get; set; }
    [Export] public SpriteFrames SlimeFrames { get; set; }
    [Export] public SpriteFrames SkeletonFrames { get; set; }


    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        
        var randomIndex = rng.RandiRange(0, EnemyManager.Instance.Enemies.Count - 1);
        _enemy = EnemyManager.Instance.Enemies[randomIndex];
        
        switch (_enemy.Name)
        {
            case "Goblin": _anim.SpriteFrames = GoblinFrames; break;
            case "Slime": _anim.SpriteFrames = SlimeFrames; break;
            case "Skeleton": _anim.SpriteFrames = SkeletonFrames; break;
        }
        
        GD.Print("Enemy Selected: " + _enemy.Name);
        GD.Print("Assigned SpriteFrames: " + _anim.SpriteFrames);
        
        PlayAnimationStand();
    }
    
    //function to decrease enemy's health when attacked
    public void DecreaseHealth(int amt)
    {
        _enemy.TakeDamage(amt);
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
