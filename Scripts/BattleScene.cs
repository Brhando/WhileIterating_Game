using Godot;
using System;

public partial class BattleScene : Node
{
    private PlayerEntity _player;
    private EnemyEntity _enemy;
    private DungeonUi _ui;

    public override void _Ready()
    {
        _player = GetNodeOrNull<PlayerEntity>("PlayerEntity");
        _enemy = GetNodeOrNull<EnemyEntity>("EnemyEntity");
        _ui = GetNodeOrNull<DungeonUi>("DungeonUI");

        if (_player == null || _enemy == null)
        {
            GD.PrintErr("PlayerEntity or EnemyEntity not found in BattleScene.");
            return;
        }
        
        _ui.UpdateSkillTexts();

        CombatManager.Instance.StartBattle(_player, _enemy);
    }
}