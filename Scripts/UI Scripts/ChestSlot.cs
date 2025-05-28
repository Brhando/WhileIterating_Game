using Godot;
using System;

public partial class ChestSlot : Control
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
        CallDeferred(nameof(TransferToInventory));
    }

    private void TransferToInventory()
    {
        GameManager.Instance.AddItem(ItemName);
        GameManager.Instance.RemoveChestItem(ItemName);
    }
}
