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
    
    //signals to track  when the player's health/stamina/block changes
    [Signal] public delegate void HealthBlockStaminaChangedEventHandler();
    
    
    //player stats to be tracked globally
    private int _playerHealth = 25;
    private int _playerStamina = 3;
    public int PlayerBlock = 0;
    public int BlockBonus = 0;
    public int DamageBonus = 0;
    public int ZeroDamage = 0; //used to return zero from the func<int> in skill class (SkillData)
    public int PlayerDamageLight = 5;
    public int PlayerDamageMid = 12;
    public int PlayerDamageHeavy = 20;
    public int PlayerDotLeft = 0;
    public int DotDamageTotal = 0;
    public bool PrayerState = false;
    
    private int _healthCap = 25;
    private int _staminaCap = 3;
    public string StoredBuff = "None";
    //Todo: add a list or class to store debuffs
    
    //player loadout
    public Dictionary<string, Skill> PlayerSkills = new();
    
     private void SetSkills()
     {
         var i = 1;
         foreach (var skill in PlayerClassManager.Instance.CurrentPlayerClass.Skills.Values)
         {
             PlayerSkills["Skill" + i] = skill;
             i++;
         }

     }
    
    public void Heal(int amount)
    {
        var health = _playerHealth;
        
        health += amount;

        if (health < _healthCap)
        {
            _playerHealth = health;
        }
        else if (health >= _healthCap)
        {
            _playerHealth = _healthCap;
        }
        EmitSignalHealthBlockStaminaChanged();
    }

    public void IncreaseBlock(int amount)
    {
        PlayerBlock += amount;
        EmitSignalHealthBlockStaminaChanged();
    }
    public void TakeDamage(int amount)
    {
        var initAmt = amount;
        amount = Mathf.Max(0, initAmt - PlayerBlock);
        PlayerBlock = Mathf.Max(0, PlayerBlock - initAmt);
        _playerHealth = Mathf.Max(0, _playerHealth - amount);
        EmitSignalHealthBlockStaminaChanged();
    }

    public void ApplyDot(int counter, int damage)
    {
        PlayerDotLeft = Mathf.Min(3, PlayerDotLeft + counter);
        DotDamageTotal += Mathf.Min(7, DotDamageTotal + damage);
    }

    public void ApplyDotDamage(int amt)
    {
        _playerHealth = Mathf.Max(0, _playerHealth - amt);
    }
    public bool CheckDot()
    {
        if (PlayerDotLeft > 0)
        {
            PlayerDotLeft--;
            return true;
        }
        DotDamageTotal = 0;
        return false;
    }

    public void IncreaseStamina(int amount)
    {
        var stamina = _playerStamina;
        
        stamina += amount;

        if (stamina < _staminaCap)
        {
            _playerStamina = stamina;
        }
        else if (stamina >= _staminaCap)
        {
            _playerStamina = _staminaCap;
        } 
        EmitSignalHealthBlockStaminaChanged();
    }

    public bool DecreaseStamina(int amount)
    {
        var stamina = _playerStamina;
        stamina -= amount;

        if (stamina < 0)
        {
            GD.Print("Cannot have negative stamina.");
            return false;
        }
        
        
        _playerStamina = stamina;
        EmitSignalHealthBlockStaminaChanged();
        return true;
    }

    public int GetPlayerStamina()
    {
        return _playerStamina;
    }

    public int GetPlayerMaxStamina()
    {
        return _staminaCap;
    }

    public int GetPlayerHealth()
    {
        return _playerHealth;
    }

    public int GetPlayerMaxHealth()
    {
        return _healthCap;
    }

    public void ResetStamina()
    {
        _playerStamina = _staminaCap;
        EmitSignalHealthBlockStaminaChanged();
    }
    
    public void SetClass(string className) 
    {
        PlayerClassManager.Instance.SetCurrentPlayerClass(className); 
        SetSkills();
    }
    
    

}