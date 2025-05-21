using Godot;
using System;

public partial class HomeInside : Node2D
{
    private Bed _bed;
    private Label _playerUi;
    private Area2D _exitHouse;
    private SceneData _sceneData;
    
    private bool _hasShownMessage = false;
    private bool _playerInTravelArea = false;
    
    

    public override void _Ready()
    {
        _bed = GetNode<Bed>("Bed");
        _playerUi = GetNode<Label>("PlayerUI/CanvasLayer/Panel/Label");
        _exitHouse = GetNode<Area2D>("ExitHouse");
        _exitHouse.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
        _exitHouse.Connect("body_exited", new Callable(this, nameof(BodyExited)));
        _sceneData = (SceneData)GetNode("/root/SceneData");
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
        if (_bed.PlayerInRange && !_hasShownMessage)
        {
            const string message = "Press 'E' to sleep.";
            _hasShownMessage = true;
            PrintMessage(message);
        }

        if (!_bed.PlayerInRange && _hasShownMessage)
        {
            _hasShownMessage = false;
        }

        if (_playerInTravelArea && !_hasShownMessage)
        {
            const string message = "Press 'E' to exit.";
            _hasShownMessage = true;
            PrintMessage(message);
        }
        if (_playerInTravelArea && Input.IsActionJustPressed("interact"))
        {
            _sceneData.ChangeSpawnPointName("SpawnExterior");
            GetTree().ChangeSceneToFile("res://Scenes/home_base.tscn");
        }
    }

}
