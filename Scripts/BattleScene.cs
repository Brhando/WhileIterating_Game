using Godot;
using System;

public partial class BattleScene : Node
{
    // Enum to track who’s turn it is
    private enum BattleState { PlayerTurn, EnemyTurn, Win, Lose }

    private BattleState _state = BattleState.PlayerTurn;

    private Button _attackButton;
    private Label _attackButtonLabel;
    private Label _turnLabel;
    private Label _enemyHp;
    private Label _playerHp;

    private PlayerEntity _player;
    private EnemyEntity _enemy;

    public override void _Ready()
    {
        // Get references to your button/label/player/enemy
        _attackButton = GetNode<Button>("DungeonUI/CanvasLayer/AttackButton");
        _attackButtonLabel = GetNode<Label>("DungeonUI/CanvasLayer/AttackButton/Label");
        _turnLabel = GetNode<Label>("DungeonUI/CanvasLayer/Panel_center/Turn");
        _enemyHp = GetNode<Label>("DungeonUI/CanvasLayer/Panel_right/Enemy_hp");
        _playerHp = GetNode<Label>("DungeonUI/CanvasLayer/Panel_left/Player_hp");
        _player = GetNode<PlayerEntity>("PlayerEntity");
        _enemy = GetNode<EnemyEntity>("EnemyEntity");

        // Disable button until it's player's turn
        _attackButton.Disabled = true;
        _attackButton.Pressed += OnPlayerAttackPressed;
        
        //set initial text values
        _enemyHp.Text = _enemy.GetHealth() + " / " + _enemy.GetMaxHealth();
        _playerHp.Text = PlayerData.Instance.GetPlayerHealth() + " / " + PlayerData.Instance.GetPlayerMaxHealth();
        _attackButtonLabel.Text = "SLASH!";
        

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        _state = BattleState.PlayerTurn;
        _turnLabel.Text = "Player's Turn";
        _attackButton.Disabled = false;
    }

    private async void OnPlayerAttackPressed()
    {
        _attackButton.Disabled = true;
        _player.PlayAnimationSlash();
        
        //wait for animation to play
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        _player.PlayAnimationStand();
        
        _enemy.PlayAnimationHurt();
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        _enemy.PlayAnimationStand();
        
        //decrease health and update the label's text
        _enemy.DecreaseHealth(3);
        _enemyHp.Text = _enemy.GetHealth() + " / " + _enemy.GetMaxHealth();

        if (_enemy.IsDead())
        {
            _state = BattleState.Win;
            _turnLabel.Text = "You Win!";
            return;
        }

        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        StartEnemyTurn();
    }

    private async void StartEnemyTurn()
    {
        _state = BattleState.EnemyTurn;
        _turnLabel.Text = "Enemy's Turn";
        
        _enemy.PlayAnimationAttack();
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        _enemy.PlayAnimationStand();
        
        _player.PlayAnimationHurt(); //play damage animation
        
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout"); //wait for animation
        _player.PlayAnimationStand();
        
        //decrease health and update text
        _player.DecreaseHealth(2);
        _playerHp.Text = PlayerData.Instance.GetPlayerHealth() + " / " + PlayerData.Instance.GetPlayerMaxHealth();

        if (_player.IsDead())
        {
            _state = BattleState.Lose;
            _turnLabel.Text = "You Lose!";
            return;
        }

        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        StartPlayerTurn();
    }
}