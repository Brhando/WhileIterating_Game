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
        public void TakeDamage(int amount)
        {
            var initAmt = amount;
            _shield = Mathf.Max(0, _shield - amount);
            amount = Mathf.Max(0, initAmt - _shield);
            _health = Mathf.Max(0, _health - amount);
        }
        public bool IsDead()
        {
            return _health <= 0;
        }
    }

    private readonly EnemySkill _strike = new("Strike", 5, "Attack");
    private readonly EnemySkill _block = new("Block", 0, "Defend", 5);

    private Enemy _goblin;
    private Enemy _slime;
    private Enemy _skeleton;

    public List<Enemy> Enemies = new();

    //Initialization
    private void InitializeEnemies()
    {
        _goblin = new Enemy("Goblin", 15, 15);
        _goblin.AddSkill(_strike);
        _goblin.AddSkill(_block);

        _slime = new Enemy("Slime", 12, 12);
        _slime.AddSkill(_strike);
        _slime.AddSkill(_block);

        _skeleton = new Enemy("Skeleton", 20, 20);
        _skeleton.AddSkill(_strike);
        _skeleton.AddSkill(_block);

        Enemies.Add(_goblin);
        Enemies.Add(_slime);
        Enemies.Add(_skeleton);
    }
    
}