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
        
        InitializeEnemies();
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
        
        //constructor
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
        
        //constructor
        public Enemy(string name, int health, int level = 1)
        {
            _name = name;
            _health = health;
            _level = level;
            _skills = new List<EnemySkill>();
        }
        //func used to add skills
        public void AddSkill(EnemySkill skill)
        {
            _skills.Add(skill);
        }
    }

    private EnemySkill _strike = new("Strike", 5, "Attack");
    private EnemySkill _block = new("Block", 0, "Defend", 5);

    private Enemy _goblin;
    private Enemy _slime;
    private Enemy _skeleton;

    public List<Enemy> Enemies = new();

    //Initialization
    private void InitializeEnemies()
    {
        _goblin = new Enemy("Goblin", 15);
        _goblin.AddSkill(_strike);
        _goblin.AddSkill(_block);

        _slime = new Enemy("Slime", 12);
        _slime.AddSkill(_strike);
        _slime.AddSkill(_block);

        _skeleton = new Enemy("Skeleton", 20);
        _skeleton.AddSkill(_strike);
        _skeleton.AddSkill(_block);

        Enemies.Add(_goblin);
        Enemies.Add(_slime);
        Enemies.Add(_skeleton);
    }
    
}