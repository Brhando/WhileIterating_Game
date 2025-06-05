using Godot;
using System.Collections.Generic;

public partial class DungeonManager : Node
{
    public static DungeonManager Instance;

    public int CurrentDungeonLevel { get; private set; } = 1;
    public List<EnemyManager.Enemy> ActiveDungeonEnemies = new();

    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();
    }

    public void GenerateDungeon(int roomCount)
    {
        ActiveDungeonEnemies.Clear();

        var eligible = EnemyManager.Instance.Enemies.FindAll(e => e.Level <= CurrentDungeonLevel);
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        for (int i = 0; i < roomCount; i++)
        {
            var randomEnemy = eligible[rng.RandiRange(0, eligible.Count - 1)].Clone();
            ActiveDungeonEnemies.Add(randomEnemy);
        }

        GD.Print($"Dungeon Level {CurrentDungeonLevel} generated with {ActiveDungeonEnemies.Count} enemies.");
    }

    public void IncreaseDungeonLevel()
    {
        CurrentDungeonLevel++;
        GD.Print($"Dungeon Level is now {CurrentDungeonLevel}");
    }

    public void IncreaseEnemiesLevel()
    {
        foreach (var enemy in ActiveDungeonEnemies)
        {
            enemy.LevelUp();
            EnemyManager.Instance.RefreshSkills(enemy);
        }
    }
}