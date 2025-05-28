using Godot;
using System;

public partial class ChestUI : CanvasLayer
{
    private Button _button1;
    private GridContainer _grid;
    [Export] private Chest _chest;
    [Export] private PackedScene _slotScene;
    public override void _Ready()
    {
        Visible = false;
        _button1 = GetNode<Button>("Panel/CloseButton");
        _grid = GetNode<GridContainer>("Panel/GridContainer");
        
        GameManager.Instance.ChestInventoryChanged += UpdateInventory;
        UpdateInventory(); //initial load
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("interact"))
        {
            Visible = true;
        }

        if ((Visible && _button1.IsPressed()) || !_chest.PlayerInRange)
        {
            Visible = false;
        }
    }
    //disconnect upon tree exit
    public override void _ExitTree()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ChestInventoryChanged -= UpdateInventory;
    }
    
    private void UpdateInventory()
    {
        //guard against null instances
        if (_grid == null || !IsInstanceValid(_grid))
        {
            GD.PrintErr("Inventory grid is null or invalid. Skipping update.");
            return;
        }
        //remove the old children
        while (_grid.GetChildCount() > 0)
        {
            var child = _grid.GetChild(0);
            _grid.RemoveChild(child);
            child.QueueFree();
        }
        //rebuild with the new children
        foreach (var entry in GameManager.Instance.ChestInventory)
        {
            var slot = _slotScene.Instantiate<ChestSlot>();
            slot.ItemName = entry.Key;
            slot.GetNode<TextureRect>("TextureRect").Texture = GetIconForItem(entry.Key);
            slot.GetNode<Label>("TextureRect/Label").Text = entry.Value.ToString();
            _grid.AddChild(slot);
        }
    }
    
    private static Texture2D GetIconForItem(string itemName)
    {
        //item icon lookup
        return GD.Load<Texture2D>($"res://Assets/Icons/{itemName}.png");
    }
    
    
    
}
