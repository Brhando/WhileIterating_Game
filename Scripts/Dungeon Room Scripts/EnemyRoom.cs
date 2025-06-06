using Godot;
using System;

public partial class EnemyRoom : Node2D
{
    private PlayerEntity _player;
    private EnemyEntity _enemy;
    private DungeonUi _ui;

    public override void _Ready()
    {
        _player = GetNodeOrNull<PlayerEntity>("PlayerEntity");
        _ui = GetNodeOrNull<DungeonUi>("DungeonUI");

        var enemyContainer = GetNode<Node>("EnemyContainer");
        int index = DungeonRoomManager.Instance.GetCurrentRoomIndex();
        var encounter = DungeonManager.Instance.ActiveDungeonEncounters[index];

        foreach (var enemyData in encounter)
        {
            var enemyScene = GD.Load<PackedScene>("res://Scenes/enemy_entity.tscn");
            var enemyNode = enemyScene.Instantiate<EnemyEntity>();
            enemyNode.Initialize(enemyData);
            
            enemyNode.Connect("EnemyClicked", new Callable(this, nameof(OnEnemyClicked)));

            enemyContainer.AddChild(enemyNode);
            CombatManager.Instance.AddEnemyToBattle(enemyNode);
        }

        if (_player == null)
        {
            GD.PrintErr("PlayerEntity not found in EnemyRoom.");
            return;
        }

        _ui.UpdateSkillTexts();

        CombatManager.Instance.StartBattle(_player);
    }
    
    private void OnEnemyClicked(EnemyEntity clickedEnemy)
    {
        CombatManager.Instance.SetTarget(clickedEnemy);
    }
}
