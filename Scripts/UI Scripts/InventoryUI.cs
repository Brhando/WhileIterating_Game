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
    
    public override void _ExitTree()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.InventoryChanged -= UpdateInventory;
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
        //add in the newly updated children
        foreach (var entry in GameManager.Instance.Inventory)
        {
            var slot = _slotScene.Instantiate<InventorySlot>();
            slot.ItemName = entry.Key;
            slot.GetNode<TextureRect>("TextureRect").Texture = GetIconForItem(entry.Key);
            slot.GetNode<Label>("TextureRect/Label").Text = entry.Value.ToString();
            _grid.AddChild(slot);
        }
    }

    private static Texture2D GetIconForItem(string itemId)
    {
        // Replace with actual item icon lookup
        return GD.Load<Texture2D>(ItemDatabase.Instance.Items[itemId].IconPath);
    }
}
