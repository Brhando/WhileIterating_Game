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
        InitializeSkills();
    }
    
    //class to define enemy skills
    public class EnemySkill
    {
        private string _name;
        private int _level;
        private string _enemyType;
        private int _damage;
        private string _type;
        private int _shieldValue;
        private bool _isDamageOverTime;
        private bool _isBuff;
        private int _dotCounter;
        
        public int Damage { get => _damage; set => _damage = value; }
        public string Type { get => _type; set => _type = value; }
        public int ShieldValue { get => _shieldValue; set => _shieldValue = value; }
        public string Name {get => _name; set => _name = value;}
        public int Level => _level;
        public string EnemyType => _enemyType;
        public bool IsDamageOverTime => _isDamageOverTime;
        public bool IsBuff => _isBuff;
        public int DotCounter => _dotCounter;
        
        //constructor
        public EnemySkill(string name, string etype, int level, int damage, string type, int shield = 0, bool dot = false, bool buff = false, int dotCounter = 0)
        {
            _name = name;
            _level = level;
            _enemyType = etype;
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
        public int Level => _level;
        
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
        
        public Enemy Clone()
        {
            var clone = new Enemy(_name, _health, _maxHealth, _level, _shield);

            //Generate skills based on level
            Instance.RefreshSkills(clone);

            GD.Print($"Cloned enemy: {_name} with {clone.GetSkills().Count} skills at level {clone.Level}");
            return clone;
        }
        public void LevelUp()
        {
            _level++;
            ScaleStats();
            Instance.RefreshSkills(this);
        }

        private void ScaleStats()
        {
            _health += Mathf.RoundToInt(_maxHealth * 0.2f);
            _skills.ForEach(skill =>
            {
                //scale the damage of each skill in the list
                var newDamage = skill.Damage + (_level * 2);
                skill.Damage = newDamage;
            });
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
    private readonly EnemySkill _strike = new("Strike", "All", 1, 5, "Attack");
    private readonly EnemySkill _block = new("Block","All", 1, 0, "Defend", 5);
    private readonly EnemySkill _skeletonStab = new("SkeletonStab","Skeleton", 2, 5, "DOT", 0, true, false, 3);
    private Dictionary<string, EnemySkill> _enemySkillLibrary = new();
    private Enemy _goblin;
    private Enemy _slime;
    private Enemy _skeleton;

    public List<Enemy> Enemies = new();

    //Initialization
    private void InitializeSkills()
    {
        _enemySkillLibrary["Strike"] = _strike;
        _enemySkillLibrary["Block"] = _block;
        _enemySkillLibrary["SkeletonStab"] = _skeletonStab;
    }
    private void InitializeEnemies()
    {
        _goblin = new Enemy("Goblin", 45, 45);
        AddSkills(_goblin);

        _slime = new Enemy("Slime", 36, 36);
        AddSkills(_slime);

        _skeleton = new Enemy("Skeleton", 60, 60);
        AddSkills(_skeleton);

        Enemies.Add(_goblin);
        Enemies.Add(_slime);
        Enemies.Add(_skeleton);
    }

    private void AddSkills(Enemy enemy)
    {
        foreach (var skill in _enemySkillLibrary.Values)
        {
            if ((skill.EnemyType == "All" || skill.EnemyType == enemy.Name) && enemy.Level >= skill.Level)
            {
                enemy.AddSkill(skill);
            }
        }
    }
    
    public void RefreshSkills(Enemy enemy)
    {
        enemy.GetSkills().Clear(); // clear current skill list
        AddSkills(enemy);          // re-check eligibility based on new level
    }
    
}