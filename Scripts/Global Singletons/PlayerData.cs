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
        
        SetSkills(_playerClass);
        
    }
    
    //signals to track  when the player's health/stamina/block changes
    [Signal] public delegate void HealthBlockStaminaChangedEventHandler();
    
    
    //player stats to be tracked globally
    private int _playerHealth = 25;
    private string _playerClass;
    private int _playerStamina = 3;
    public int PlayerBlock = 0;
    public int PlayerDamageLight = 5;
    public int PlayerDamageMid = 12;
    public int PlayerDamageHeavy = 20;
    public int PlayerDotLeft = 0;
    public int DotDamageTotal = 0;
    public bool PrayerState = false;
    
    private int _healthCap = 25;
    private int _staminaCap = 3;
    private int _totalHeavy = 0;
    private int _totalBlocks = 0;
    public string StoredBuff = "None";
    //Todo: add a list or class to store debuffs
    
    //player loadout
    public Dictionary<string, SkillData.Skill> PlayerSkills = new();
    
     private void SetSkills(string playerClass) {
         if (playerClass == null)
         {
             PlayerSkills["Skill1"] = null;
             PlayerSkills["Skill2"] = null;
             PlayerSkills["Skill3"] = null;
             PlayerSkills["Skill4"] = null;
             PlayerSkills["Skill5"] = null;
         }
         else if (playerClass == "Sword Swinger") {
            PlayerSkills["Skill1"] = SkillData.Instance?.SkillLibrary["Slash"];
            PlayerSkills["Skill2"] = SkillData.Instance?.SkillLibrary["Thrust"];
            PlayerSkills["Skill3"] = SkillData.Instance?.SkillLibrary["Light Block"];
            PlayerSkills["Skill4"] = SkillData.Instance?.SkillLibrary["Prayer"];
            PlayerSkills["Skill5"] = SkillData.Instance?.SkillLibrary["Whirlwind"];
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
        PlayerDotLeft += counter;
        DotDamageTotal += damage;
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
    

    public void IncrementBlocks()
    {
        _totalBlocks++;
        EmitSignalHealthBlockStaminaChanged();
    }

    public void IncrementHeavy()
    {
        _totalHeavy++;
    }

    public int GetBlocks()
    {
        return _totalBlocks;
    }

    public int GetHeavy()
    {
        return _totalHeavy;
    }

    public void SetClass(string className)
    {
        _playerClass = className;
        SetSkills(_playerClass);
    }
    

}