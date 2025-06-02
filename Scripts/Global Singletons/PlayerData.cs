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
    private int _playerHealth = 25;
    private string _playerClass;
    private int _playerStamina = 3;
    public int PlayerBlock = 0;
    public int PlayerDamageLight = 5;
    public int PlayerDamageMid = 12;
    public int PlayerDamageHeavy = 20;
    public bool PrayerState = false;
    
    private int _healthCap = 25;
    private int _staminaCap = 3;
    private int _totalHeavy = 0;
    private int _totalBlocks = 0;
    public string StoredBuff = "None";
    //Todo: add a list or class to store debuffs
    
    //player loadout
    public Dictionary<string, SkillData.Skill> PlayerSkills = new();
    
    /* public SetSkills(string playerClass) {
        if (playerClass == "Sword Swinger") {
            PlayerSkills["Slash"] = SkillData.Instance.SkillLibrary["Slash"];
            PlayerSkills["Thrust"] = SkillData.Instance.SkillLibrary["Thrust"];
            PlayerSkills["Light Block"] = SkillData.Instance.SkillLibrary["Light Block"];
            PlayerSkills["Prayer"] = SkillData.Instance.SkillLibrary["Prayer"];
            PlayerSkills["Whirlwind"] = SkillData.Instance.SkillLibrary["Whirlwind"];
        }
     
     } */
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
        
    }
    public void TakeDamage(int amount)
    {
        _playerHealth = Mathf.Max(0, _playerHealth - amount);
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
        else
        {
            _playerStamina = stamina;
        }
        return true;
    }

    public int GetPlayerStamina()
    {
        return _playerStamina;
    }

    public void ResetStamina()
    {
        _playerStamina = _staminaCap;
    }

    public void IncrementBlocks()
    {
        _totalBlocks++;
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
    

}