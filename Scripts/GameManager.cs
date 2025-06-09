using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class GameManager : Node
{
    public static GameManager Instance;
    
    public override void _Ready()
    {
        //if there isn't an instance yet-> create a new one
        if (Instance == null)
        {
            Instance = this;
        }
        //if there is, delete it, so that you don't have duplicates
        else
        {
            QueueFree();
        }
    }
/////////////////////////////////////--Inventory System--////////////////////////////////////////////
    // Dictionary to store item names and their quantities
    public Dictionary<string, int> Inventory = new();
    public Dictionary<string, int> ChestInventory = new();
    [Signal] public delegate void InventoryChangedEventHandler();
    [Signal] public delegate void ChestInventoryChangedEventHandler();
    public Chest ActiveChest { get; set; }
    //inventory space capacity
    private int _inventoryCap = 10;
    private int _chestCap = 20;
    //used to add items to the player inventory
    public bool AddItem(string itemName, int amount = 1)
    {
        var uniqueItemCount = Inventory.Count;
        
        if (!Inventory.ContainsKey(itemName) && uniqueItemCount >= _inventoryCap)
        {
            GD.Print("Inventory full!");
            return false;
        }

        if (Inventory.ContainsKey(itemName))
            Inventory[itemName] += amount;
        else
            Inventory[itemName] = amount;

        GD.Print($"Added {amount} {itemName}. Total: {Inventory[itemName]}");
        CallDeferred(nameof(EmitInventoryChanged));
        return true;
    }
    private void EmitInventoryChanged()
    {
        EmitSignal(nameof(InventoryChanged));
    }

    //used to remove from player inventory
    public bool RemoveItem(string itemName, int amount = 1)
    {
        if (Inventory.ContainsKey(itemName))
        {
            var currentTotal = Inventory[itemName];
            currentTotal -= amount;
            
            if (currentTotal < 0)
            {
                GD.Print("You can't remove more than you have!");
                return false;
            }
            
            if (currentTotal == 0)
                Inventory.Remove(itemName);
            else
            {
                Inventory[itemName] = currentTotal;
            }
            EmitSignalInventoryChanged();
            return true;
        }
        return false;
    }
    
    //used to add items to chest inventory
    public bool AddChestItem(string itemName, int amount = 1)
    {
        var uniqueItemCount = ChestInventory.Count;

        if (!ChestInventory.ContainsKey(itemName) && uniqueItemCount >= _chestCap)
        {
            GD.Print("Chest full!");
            return false;
        }

        if (ChestInventory.ContainsKey(itemName))
        {
            ChestInventory[itemName] += amount;
        }
        else
        {
            ChestInventory[itemName] = amount;
        }
        
        GD.Print($"Added {amount} {itemName} to Chest");
        CallDeferred(nameof(EmitChestInventoryChanged));
        return true;
    }
    
    private void EmitChestInventoryChanged()
    {
        EmitSignal(nameof(ChestInventoryChanged));
    }
    
    //used to remove from chest inventory
    public bool RemoveChestItem(string itemName, int amount = 1)
    {
        if (ChestInventory.ContainsKey(itemName))
        {
            var currentTotal = ChestInventory[itemName];
            currentTotal -= amount;
            
            if (currentTotal < 0)
            {
                GD.Print("You can't remove more than you have!");
                return false;
            }
            
            if (currentTotal == 0)
                ChestInventory.Remove(itemName);
            else
            {
                ChestInventory[itemName] = currentTotal;
            }
            EmitSignalChestInventoryChanged();
            return true;
        }
        return false;
    }
    
    public int GetItemCount(string itemName)
    {
        return Inventory.GetValueOrDefault(itemName, 0);
    }

///////////////////////////////////--Day/Night System--//////////////////////////////////////////////
    public enum TimeOfDay
    {
        Morning,
        MidMorning,
        Noon,
        Afternoon,
        Night
    };
    
    public TimeOfDay CurrentTimeOfDay { get; private set; } = TimeOfDay.Morning;
    
    //For systems that need to know when the time changes we create a signal
    [Signal] public delegate void TimeOfDayChangedEventHandler(TimeOfDay newTime);
    
    //integer used to track number of days that have passed
    public int DayCount { get; private set; } = 1;
    
    //function to advance time
    public void AdvanceTime()
    {
        if (CurrentTimeOfDay != TimeOfDay.Night)
        {
            CurrentTimeOfDay += 1;
            EmitSignalTimeOfDayChanged(CurrentTimeOfDay);
        }
        else
        {
            GD.Print("It is already night. You need to sleep.");
        }
    }
    
    //function to call AdvanceTime if a player travels to a new area
    public void Travel(string areaName)
    {
        if (areaName == "Dungeon")
        {
            CurrentTimeOfDay = TimeOfDay.Night;
        }
        else
        {
            AdvanceTime();
        }
        EmitSignalTimeOfDayChanged(CurrentTimeOfDay);
    }
    
    //function to reset the time of day if the player sleeps
    public void Sleep()
    {
        CurrentTimeOfDay = TimeOfDay.Morning;
        DayCount++;
        DreamManager.Instance.RefreshDreamTextList2();
        PlayerData.Instance.Heal(PlayerData.Instance.GetPlayerMaxHealth()); //make this less effective for future runs
        EmitSignalTimeOfDayChanged(CurrentTimeOfDay);
        GD.Print("New day started. Day: " + DayCount);
    }
    
}