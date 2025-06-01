using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyManager : Node
{
    //used to define enemy stats, skills, and behavior
    public static EnemyManager Instance;

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
    
    //class to define enemy skills
    public class EnemySkill
    {
        private string _name;
        private int _damage;
        private string _type;
        private int _shieldValue;
        private bool _isDamageOverTime;
        private bool _isBuff;

        public EnemySkill(string name, int damage, string type, int shield = 0, bool dot = false, bool buff = false)
        {
            _name = name;
            _damage = damage;
            _type = type;
            _shieldValue = shield;
            _isDamageOverTime = dot;
            _isBuff = buff;
        }
    }
    
    //class to define enemies
    public class Enemy
    {
        private string _name;
        private int _health;
        private int _level;
        private List<EnemySkill> _skills;

        public Enemy(string name, int health, int level = 1)
        {
            _name = name;
            _health = health;
            _level = level;
            _skills = new List<EnemySkill>();
        }

        public void AddSkill(EnemySkill skill)
        {
            _skills.Add(skill);
        }
    }

    public EnemySkill Strike = new EnemySkill("Strike", 5, "Attack");
    public EnemySkill Block = new EnemySkill("Block", 0, "Defend", 5);

    public Enemy Goblin;
    public Enemy Slime;
    public Enemy Skeleton;

    public List<Enemy> Enemies = new();

    //Initialization
    private void InitializeEnemies()
    {
        Goblin = new Enemy("Goblin", 15);
        Goblin.AddSkill(Strike);

        Slime = new Enemy("Slime", 12);
        Slime.AddSkill(Block);

        Skeleton = new Enemy("Skeleton", 20);
        Skeleton.AddSkill(Strike);
        Skeleton.AddSkill(Block);

        Enemies.Add(Goblin);
        Enemies.Add(Slime);
        Enemies.Add(Skeleton);
    }
    
}