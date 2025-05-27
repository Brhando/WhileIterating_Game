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
    //used to add items to the inventory
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
        EmitSignalTimeOfDayChanged(CurrentTimeOfDay);
        GD.Print("New day started. Day: " + DayCount);
    }
    
}