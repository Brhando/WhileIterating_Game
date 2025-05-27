using Godot;
using System;

public partial class HomeBase : Node2D
{
    private Chest _chest;
    private Label _playerUi;
    private Area2D _enterHouse;
    private SceneData _sceneData;
    private Node2D _spawnPoint;
    private MapOpen _mapOpen;
    private CanvasLayer _inventoryUi;
    private bool _playerInTravelArea = false;
    
    
    public override void _Ready()
    {
        _chest = GetNode<Chest>("Chest");
        _mapOpen = GetNode<MapOpen>("MapOpen");
        _playerUi = GetNode<Label>("PlayerUI/CanvasLayer/Panel/Label");
        _enterHouse = GetNode<Area2D>("EnterHouse");
        _inventoryUi = GetNode<CanvasLayer>("InventoryUI/CanvasLayer");
        _enterHouse.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
        _enterHouse.Connect("body_exited", new Callable(this, nameof(BodyExited)));
        
        _inventoryUi.Visible = false;
        
        // Find the spawn point specified in the global SceneData
        // Move the player to that spawn point
        var player = GetNode<Node2D>("Player");
        if (SceneData.DataInstance.GetSpawnPointName() != "default")
        {
            var spawnPoint = GetNode<Node2D>(SceneData.DataInstance.SpawnPointName);
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
    
    private void PrintMessage(string message)
    {
        _playerUi.Visible = true;
        _playerUi.Text = message;
    }

    public override void _Process(double delta)
    {
        var message = "";
    
        if (_playerInTravelArea)
        {
            message = "Press 'E' to enter.";
        
            if (Input.IsActionJustPressed("interact"))
            {
                GetTree().ChangeSceneToFile("res://Scenes/home_inside.tscn");
            }
        }
        else if (_chest.PlayerInRange)
        {
            message = "Press 'E' to open.";
        }
        
        else if (_mapOpen.PlayerInTravelArea)
        {
            message = "Press 'E' to open map.";
            if (Input.IsActionJustPressed("interact"))
            {
                SceneData.DataInstance.ChangeSpawnPointName("default");
                GetTree().ChangeSceneToFile("res://Scenes/map_interface.tscn");
            }
        }

        if (message != "")
        {
            PrintMessage(message);
        }
        else
        {
            _playerUi.Visible = false;
        }

        if (Input.IsActionJustPressed("inventory"))
        {
            _inventoryUi.Visible = !_inventoryUi.Visible;
        }
    }
}
