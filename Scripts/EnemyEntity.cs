using Godot;
using System;

public partial class EnemyEntity : CharacterBody2D
{
    public int EnemyHealth = 10;
    private AnimatedSprite2D _anim;

    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
    }
    
    //function to decrease enemy's health when attacked
    public void DecreaseHealth(int amt)
    {
        EnemyHealth -= amt;
    }

    public bool IsDead()
    {
        return EnemyHealth <= 0;
    }
    
}
