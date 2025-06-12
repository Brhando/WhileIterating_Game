using Godot;
using System.Collections.Generic;

public partial class ItemDatabase : Node
{
    public static ItemDatabase Instance;
    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();
        LoadItems();
    }

    public Dictionary<string, ItemData> Items = new();
    private void LoadItems()
    {
        //hardcoded for now; will load from JSON file later
        //-1 implies infinity; 0 implies N/A
        AddItem(new ItemData("wood_log", "Wood", ItemType.Material, "A sturdy log of wood.",
            "res://Assets/Icons/Wood.png", 60, 10, 0));
        AddItem(new ItemData("stone_chunk", "Stone", ItemType.Material, "A hefty chunk of stone.",
            "res://Assets/Icons/Stone.png", 60, 10, 0));
        AddItem(new ItemData("stone_plate", "Stone Plate", ItemType.Consumable, "Basic defensive material. Increases shields by 5 when used.",
            "res://Assets/Icons/Stone Plate.png", 5, 8, 1));
        AddItem(new ItemData("plank", "Plank", ItemType.Material, "Refined wood. Don't make it your best friend. It's better used in upgrades.",
            "res://Assets/Icons/Plank.png", 20, 10, 1)); //need to decide what this builds into and how it will help the player
        AddItem(new ItemData("bandage", "Bandage", ItemType.Consumable, "Heals a small amount",
            "res://Assets/Icons/Bandage.png", 5, 5, 1));
        AddItem(new ItemData("gold", "Gold", ItemType.Currency, "Almost always present and valued. Makes the world go round.",
            "res://Assets/Icons/Gold.png", -1, 0, 0));
    }
    
    private void AddItem(ItemData item)
    {
        if (!Items.ContainsKey(item.ID))
            Items.Add(item.ID, item);
    }
    public ItemData GetItem(string id)
    {
        if (Items.ContainsKey(id))
            return Items[id];
        GD.PrintErr("Item not found: " + id);
        return null;
    }
    
    
    
}