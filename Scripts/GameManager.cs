using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class GameManager : Node
{
    public static GameManager Instance;
    [Signal] public delegate void InventoryChangedEventHandler();

    // Dictionary to store item names and their quantities
    public Dictionary<string, int> Inventory = new();
    
    //inventory space capacity
    private int _inventoryCap = 20;

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
        }
    }

    public bool AddItem(string itemName, int amount = 1)
    {
        int currentTotal = Inventory.Values.Sum();

        if (currentTotal + amount > _inventoryCap)
        {
            GD.Print("Inventory full!");
            return false;
        }

        if (Inventory.ContainsKey(itemName))
            Inventory[itemName] += amount;
        else
            Inventory[itemName] = amount;

        GD.Print($"Added {amount} {itemName}. Total: {Inventory[itemName]}");
        EmitSignal(nameof(InventoryChanged));
        return true;
    }

    public int GetItemCount(string itemName)
    {
        return Inventory.GetValueOrDefault(itemName, 0);
    }

    
}