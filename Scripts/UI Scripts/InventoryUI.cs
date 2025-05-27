using Godot;

public partial class InventoryUI : Control
{
    [Export] private GridContainer _grid;
    [Export] private PackedScene _slotScene;

    public override void _Ready()
    {
        GameManager.Instance.Connect("InventoryChanged", new Callable(this, nameof(UpdateInventory)));
        UpdateInventory(); // initial load
    }

    private void UpdateInventory()
    {
        //clear each item
        foreach (var child in _grid.GetChildren())
            child.QueueFree();
        
        //readd updated items and amounts
        foreach (var entry in GameManager.Instance.Inventory)
        {
            var slot = _slotScene.Instantiate<Control>();
            slot.GetNode<TextureRect>("TextureRect").Texture = GetIconForItem(entry.Key);
            slot.GetNode<Label>("TextureRect/Label").Text = entry.Value.ToString();
            _grid.AddChild(slot);
        }
    }

    private static Texture2D GetIconForItem(string itemName)
    {
        // Replace with actual item icon lookup
        return GD.Load<Texture2D>($"res://Assets/Icons/{itemName}.png");
    }
}
