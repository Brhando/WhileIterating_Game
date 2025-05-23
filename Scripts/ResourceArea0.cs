using Godot;
using System;

public partial class ResourceArea0 : Node2D
{
    private CanvasLayer _inventoryUi;

    public override void _Ready()
    {
        _inventoryUi = GetNode<CanvasLayer>("InventoryUI/CanvasLayer");
        _inventoryUi.Visible = false;
    }
    
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventory"))
        {
            _inventoryUi.Visible = !_inventoryUi.Visible;
            GD.Print("Inventory Toggled: " + _inventoryUi.Visible);
        }
        
    }
    
}

