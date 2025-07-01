using Godot;
using System;

public partial class PlayerEntity : CharacterBody2D
{
    public int PlayerHealth;
    public int PlayerMaxHealth;
    private AnimatedSprite2D _anim1;
    
    
    public override void _Ready()
    {
        _anim1 = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        

        PlayerHealth = PlayerData.Instance.GetPlayerHealth();
        PlayerMaxHealth = PlayerData.Instance.GetPlayerMaxHealth();
    }
    
    //used to decrement health when attacked
    public void DecreaseHealth(int amt)
    {
        PlayerData.Instance.TakeDamage(amt);
    }

    public bool CheckDot(DebuffType appliedDebuff)
    {
        return PlayerData.Instance.CheckDot(appliedDebuff);
    }

    public void ApplyDebuffDamage(int dmg)
    {
        PlayerData.Instance.TakeDamage(dmg);
    }

    public void ApplyDebuff(DebuffType appliedDebuff)
    {
        PlayerData.Instance.ApplyDebuff(appliedDebuff);
    }
    
    //play certain animations depending on what is happening
    public void PlayAnimationHurt()
    {
        _anim1.Play("hurt");
        
    }

    public void PlayAnimationStand()
    {
        _anim1.Play("stand");
        
    }

    public void PlayAnimationSlash()
    {
        _anim1.Play("slash");
        
    }

    public void PlayAnimationThrust()
    {
        _anim1.Play("thrust");
    }

    public void PlayAnimationWhirlwind()
    {
        _anim1.Play("whirlwind");
    }

    public void PlayAnimationBlock()
    {
        _anim1.Play("block");
    }

    public void PlayAnimationPrayer()
    {
        _anim1.Play("prayer");
    }
    
    public void SetGlowColor(Color color)
    {
        _anim1.Modulate = color;
    }

    public void ResetGlow()
    {
        _anim1.Modulate = Colors.White;
    }

    public bool IsDead()
    {
        return PlayerData.Instance.GetPlayerHealth() <= 0;
    }
    
}
