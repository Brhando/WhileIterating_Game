using Godot;
using System.Collections.Generic;

public partial class DungeonManager : Node
{
    public static DungeonManager Instance;

    public int CurrentDungeonLevel { get; private set; } = 1;
    public List<List<EnemyManager.Enemy>> ActiveDungeonEncounters = new();
    private HashSet<string> _usedEnemyNames = new(); // avoid duplicates, used to track which enemies were included in the dungeon for proper leveling

    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();
    }

    public void GenerateDungeon(int roomCount)
    {
        ActiveDungeonEncounters.Clear();
        _usedEnemyNames.Clear();

        // Get eligible base enemies for this dungeon level
        var eligible = EnemyManager.Instance.Enemies.FindAll(e => e.Level <= CurrentDungeonLevel);
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        for (var i = 0; i < roomCount; i++)
        {
            var groupSize = rng.RandiRange(1, Mathf.Min(4, CurrentDungeonLevel)); // 1–4 enemies per encounter; based on CurrentDungeonLevel
            var encounter = new List<EnemyManager.Enemy>();

            for (var j = 0; j < groupSize; j++)
            {
                var selectedTemplate = eligible[rng.RandiRange(0, eligible.Count - 1)];
                var clonedEnemy = selectedTemplate.Clone();
                encounter.Add(clonedEnemy);

                // Track the name of the original template used
                _usedEnemyNames.Add(selectedTemplate.Name);
            }

            ActiveDungeonEncounters.Add(encounter);
        }

        GD.Print($"Dungeon Level {CurrentDungeonLevel} generated with {ActiveDungeonEncounters.Count} encounters.");
    }

    public void IncreaseCurrentDungeonLevel()
    {
        CurrentDungeonLevel++;
    }
    public void IncreaseEnemiesLevel()
    {
        foreach (var enemy in EnemyManager.Instance.Enemies)
        {
            if (_usedEnemyNames.Contains(enemy.Name))
            {
                enemy.LevelUp();
                EnemyManager.Instance.RefreshSkills(enemy);
            }
        }
    }
}