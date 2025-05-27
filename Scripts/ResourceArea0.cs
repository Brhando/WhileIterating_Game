using Godot;
using System;

public partial class ResourceArea0 : Node2D
{
    private CanvasLayer _inventoryUi;
    private Label _playerUi;
    private MapOpen _mapOpen;

    public override void _Ready()
    {
        _inventoryUi = GetNode<CanvasLayer>("InventoryUI/CanvasLayer");
        _playerUi = GetNode<Label>("PlayerUI/CanvasLayer/Panel/Label");
        _mapOpen = GetNode<MapOpen>("MapOpen");
        _inventoryUi.Visible = false;
    }
    
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventory"))
        {
            _inventoryUi.Visible = !_inventoryUi.Visible;
        }

        if (_mapOpen.PlayerInTravelArea)
        {
            _playerUi.Visible = true;
            _playerUi.Text = "Press 'E' to open map.";

            if (Input.IsActionJustPressed("interact"))
            {
                GetTree().ChangeSceneToFile("res://Scenes/map_interface.tscn");
            }
        }
        else
        {
            _playerUi.Visible = false;
        }
        
    }
    
}

