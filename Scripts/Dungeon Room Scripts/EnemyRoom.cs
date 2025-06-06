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
        var _position1 = GetNodeOrNull<Node2D>("EnemyContainer/Position1");
        var _position2 = GetNodeOrNull<Node2D>("EnemyContainer/Position2");
        var _position3 = GetNodeOrNull<Node2D>("EnemyContainer/Position3");
        var _position4 = GetNodeOrNull<Node2D>("EnemyContainer/Position4");
        
        int index = DungeonRoomManager.Instance.GetCurrentRoomIndex();
        int i = 0;
        
        var encounter = DungeonManager.Instance.ActiveDungeonEncounters[index];

        
        foreach (var enemyData in encounter)
        {
            var enemyScene = GD.Load<PackedScene>("res://Scenes/enemy_entity.tscn");
            var enemyNode = enemyScene.Instantiate<EnemyEntity>();
            enemyNode.Initialize(enemyData);
            
            enemyNode.Connect("EnemyClicked", new Callable(this, nameof(OnEnemyClicked)));

            enemyContainer.AddChild(enemyNode);
            switch (i)
            {
                case 0:
                    enemyNode.Position = _position1.Position;
                    break;
                case 1:
                    enemyNode.Position = _position2.Position;
                    break;
                case 2:
                    enemyNode.Position = _position3.Position;
                    break;
                case 3:
                    enemyNode.Position = _position4.Position;
                    break;
            }
            
            CombatManager.Instance.AddEnemyToBattle(enemyNode);
            i++;
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
