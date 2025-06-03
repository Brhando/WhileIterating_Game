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
        private int _dotCounter;
        
        public int Damage => _damage;
        public string Type => _type;
        public int ShieldValue => _shieldValue;
        public string Name => _name;
        public bool IsDamageOverTime => _isDamageOverTime;
        public bool IsBuff => _isBuff;
        public int DotCounter => _dotCounter;
        
        //constructor
        public EnemySkill(string name, int damage, string type, int shield = 0, bool dot = false, bool buff = false, int dotCounter = 0)
        {
            _name = name;
            _damage = damage;
            _type = type;
            _shieldValue = shield;
            _isDamageOverTime = dot;
            _isBuff = buff;
            _dotCounter = dotCounter;
        }
        
    }
    
    //class to define enemies
    public class Enemy
    {
        private readonly string _name;
        private int _health;
        private readonly int _maxHealth;
        private int _level;
        private int _shield;
        private List<EnemySkill> _skills;
        public int Health => _health;
        public string Name => _name;
        public int MaxHealth => _maxHealth;
        public int Shield => _shield;
        
        //constructor
        public Enemy(string name, int health, int maxHealth, int level = 1, int shield = 0)
        {
            _name = name;
            _health = health;
            _maxHealth = maxHealth;
            _level = level;
            _shield = shield;
            _skills = new List<EnemySkill>();
            
        }
        
        //func used to add skills
        public void AddSkill(EnemySkill skill)
        {
            _skills.Add(skill);
        }

        public List<EnemySkill> GetSkills()
        {
            return _skills;
        }
        public void TakeDamage(int amount)
        {
            var initAmt = amount;
            amount = Mathf.Max(0, initAmt - _shield);
            _shield = Mathf.Max(0, _shield - initAmt);
            _health = Mathf.Max(0, _health - amount);
        }

        public void AddShield(int amount)
        {
            _shield += amount;
        }
        public bool IsDead()
        {
            return _health <= 0;
        }
    }
    //Enemy Skills
    private readonly EnemySkill _strike = new("Strike", 5, "Attack");
    private readonly EnemySkill _block = new("Block", 0, "Defend", 5);
    private readonly EnemySkill _skeletonStab = new("SkeletonStab", 5, "DOT", 0, true, false, 3);

    private Enemy _goblin;
    private Enemy _slime;
    private Enemy _skeleton;

    public List<Enemy> Enemies = new();

    //Initialization
    private void InitializeEnemies()
    {
        _goblin = new Enemy("Goblin", 45, 45);
        _goblin.AddSkill(_strike);
        _goblin.AddSkill(_block);

        _slime = new Enemy("Slime", 36, 36);
        _slime.AddSkill(_strike);
        _slime.AddSkill(_block);

        _skeleton = new Enemy("Skeleton", 60, 60);
        _skeleton.AddSkill(_strike);
        _skeleton.AddSkill(_block);
        _skeleton.AddSkill(_skeletonStab);

        Enemies.Add(_goblin);
        Enemies.Add(_slime);
        Enemies.Add(_skeleton);
    }
    
}