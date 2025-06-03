using Godot;
using System;

public partial class PlayerEntity : CharacterBody2D
{
    public int PlayerHealth;
    public int PlayerMaxHealth;
    private AnimatedSprite2D _anim1;
    private AnimatedSprite2D _anim2;
    
    public override void _Ready()
    {
        _anim1 = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _anim2 = GetNode<AnimatedSprite2D>("AnimatedSprite2D2");

        PlayerHealth = PlayerData.Instance.GetPlayerHealth();
        PlayerMaxHealth = PlayerData.Instance.GetPlayerMaxHealth();
    }
    
    //used to decrement health when attacked
    public void DecreaseHealth(int amt)
    {
        PlayerData.Instance.TakeDamage(amt);
    }

    public bool CheckDot()
    {
        return PlayerData.Instance.CheckDot();
    }

    public void ApplyDotDamage()
    {
        PlayerData.Instance.ApplyDotDamage(PlayerData.Instance.DotDamageTotal);
    }

    public void ApplyDot(int counter, int damage)
    {
        PlayerData.Instance.ApplyDot(counter, damage);
    }
    
    //play certain animations depending on what is happening
    public void PlayAnimationHurt()
    {
        _anim1.Play("hurt");
        _anim2.Play("hurt_s");
    }

    public void PlayAnimationStand()
    {
        _anim1.Play("stand");
        _anim2.Play("stand_s");
    }

    public void PlayAnimationSlash()
    {
        _anim1.Play("slash");
        _anim2.Play("slash_s");
    }

    public bool IsDead()
    {
        return PlayerData.Instance.GetPlayerHealth() <= 0;
    }
    
}
