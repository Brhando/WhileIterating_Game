using Godot;
using System;

public partial class HomeInside : Node2D
{
    private Bed _bed;
    private Label _playerUi;
    private Area2D _exitHouse;
    private SceneData _sceneData;

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

    private void PrintMessage(string message)
    {
        _playerUi.Visible = true;
        _playerUi.Text = message;

    }

    public override void _Process(double delta)
    {
        string message = "";

        if (_playerInTravelArea)
        {
            message = "Press 'E' to exit.";

            if (Input.IsActionJustPressed("interact"))
            {
                _sceneData.ChangeSpawnPointName("SpawnExterior");
                GetTree().ChangeSceneToFile("res://Scenes/home_base.tscn");
            }
        }
        else if (_bed.PlayerInRange)
        {
            message = "Press 'E' to sleep.";
            // TODO: create sleep logic (heal to full health/reset daytime resource
        }

        if (message != "")
        {
            PrintMessage(message);
        }
        else
        {
            _playerUi.Visible = false;
        }
    }
}
