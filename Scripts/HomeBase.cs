using Godot;
using System;

public partial class HomeBase : Node2D
{
    private Chest _chest;
    private Label _playerUi;
    private Area2D _enterHouse;
    private SceneData _sceneData;
    private Node2D _spawnPoint;
    
    private bool _hasShownMessage = false;
    private bool _playerInTravelArea = false;
    
    
    public override void _Ready()
    {
        _chest = GetNode<Chest>("Chest");
        
        _playerUi = GetNode<Label>("PlayerUI/CanvasLayer/Panel/Label");
        _enterHouse = GetNode<Area2D>("EnterHouse");
        _enterHouse.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
        _enterHouse.Connect("body_exited", new Callable(this, nameof(BodyExited)));
        
        // Find the spawn point specified in the global SceneData
        _sceneData = (SceneData)GetNode("/root/SceneData");
        
        // Move the player to that spawn point
        var player = GetNode<Node2D>("Player");
        if (_sceneData.GetSpawnPointName() != "default")
        {
            var spawnPoint = GetNode<Node2D>(_sceneData.SpawnPointName);
            player.GlobalPosition = spawnPoint.GlobalPosition;
        }
    }
    
    private void BodyEntered(Node body)
    {
        if (body.Name == "Player")
            _playerInTravelArea = true; //player is in range, and can interact with the item
    }

    private void BodyExited(Node body)
    {
        if (body.Name == "Player")
            _playerInTravelArea = false; //player no longer in range
    }
    
    private async void PrintMessage(string message)
    {
        _playerUi.Visible = true;
        _playerUi.Text = message;
        await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
        _playerUi.Visible = false;
    }

    public override void _Process(double delta)
    {
        
        if (_chest.PlayerInRange && !_hasShownMessage)
        {
            const string message = "Press 'E' to open.";
            _hasShownMessage = true;
            PrintMessage(message);
        }

        if (!_chest.PlayerInRange && _hasShownMessage)
        {
            _hasShownMessage = false;
        }

        if (_playerInTravelArea && !_hasShownMessage)
        {
            const string message = "Press 'E' to enter.";
            _hasShownMessage = true;
            PrintMessage(message);
        }
        if (_playerInTravelArea && Input.IsActionJustPressed("interact"))
        {
            GetTree().ChangeSceneToFile("res://Scenes/home_inside.tscn");
        }
    }
}
