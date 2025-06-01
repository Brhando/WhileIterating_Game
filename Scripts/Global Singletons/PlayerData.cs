using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerData: Node
{
    //used to track the player's stats
    public static PlayerData Instance;

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
    }
    
    //player stats to be tracked globally
    public int PlayerHealth = 25;
    public string PlayerClass;
    public int PlayerStamina = 3;
    public int PlayerDamageLight = 5;
    public int PlayerDamageMid = 12;
    public int PlayerDamageHeavy = 20;
    public bool PrayerState = false;
    
    private int _healthCap = 25;
    private int _staminaCap = 3;
    private int _totalHeavy = 0;
    private int _totalBlocks = 0;
    
    //player loadout
    public Dictionary<string, int> PlayerSkills = new();
    
    //Todo: create function to set player skills
    public void Heal(int amount)
    {
        var health = PlayerHealth;
        
        health += amount;

        if (health < _healthCap)
        {
            PlayerHealth = health;
        }
        else if (health >= _healthCap)
        {
            PlayerHealth = _healthCap;
        }
        
    }
    public void TakeDamage(int amount)
    {
        PlayerHealth = Mathf.Max(0, PlayerHealth - amount);
    }

    public void IncreaseStamina(int amount)
    {
        var stamina = PlayerStamina;
        
        stamina += amount;

        if (stamina < _staminaCap)
        {
            PlayerStamina = stamina;
        }
        else if (stamina >= _staminaCap)
        {
            PlayerStamina = _staminaCap;
        } 
    }

    public bool DecreaseStamina(int amount)
    {
        var stamina = PlayerStamina;
        stamina -= amount;

        if (stamina < 0)
        {
            GD.Print("Cannot have negative stamina.");
            return false;
        }
        else
        {
            PlayerStamina = stamina;
        }
        return true;
    }

}