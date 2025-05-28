using Godot;
using System;

public partial class InventorySlot : Control
{
    public string ItemName;
    private Button _button;
    public override void _Ready()
    {
        _button = GetNode<Button>("Button");
        _button.Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        var chest = GameManager.Instance.ActiveChest;

        if (chest != null && chest.PlayerInRange)
            CallDeferred(nameof(TransferToChest));
        else
            GD.Print("You must be near the home base chest to store items.");
    }

    private void TransferToChest()
    {
        GameManager.Instance.AddChestItem(ItemName);
        GameManager.Instance.RemoveItem(ItemName);
    }
}
