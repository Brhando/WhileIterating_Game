using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonRoomManager : Node
{
    public static DungeonRoomManager Instance;

    private int _currentRoomIndex = 0;
    private List<string> _roomTypes = new(); // Example: "Enemy", "Rest", "Enemy", ...

    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();
    }

    public void StartDungeonRun()
    {
        _currentRoomIndex = 0;
        _roomTypes = GenerateRoomTypes(); // Decide layout
        LoadCurrentRoom();
    }

    private List<string> GenerateRoomTypes()
    {
        var layout = new List<string>();

        // For example: alternating rest/enemy with boss at the end
        for (int i = 0; i < DungeonManager.Instance.ActiveDungeonEncounters.Count; i++)
        {
            if (i != 0 && i % 3 == 2) //spawn a 'rest' room every 3rd room
                layout.Add("Rest");
            else
                layout.Add("Enemy");
        }

        layout.Add("Boss"); // optional for now
        return layout;
    }

    public void LoadCurrentRoom()
    {
        var roomType = _roomTypes[_currentRoomIndex];

        switch (roomType)
        {
            case "Enemy":
                GetTree().ChangeSceneToFile("res://Scenes/Dungeon Rooms/enemy_room.tscn");
                break;
            case "Rest":
                GetTree().ChangeSceneToFile("res://Scenes/Dungeon Rooms/rest_room.tscn");
                break;
            case "Boss":
                GetTree().ChangeSceneToFile("res://Scenes/Dungeon Rooms/boss_room.tscn");
                break;
        }
    }

    public void NextRoom()
    {
        _currentRoomIndex++;

        if (_currentRoomIndex >= _roomTypes.Count)
        {
            GD.Print("Dungeon complete!");
            DungeonManager.Instance.IncreaseCurrentDungeonLevel();
            DungeonManager.Instance.IncreaseEnemiesLevel();
            //return to overworld
            GetTree().ChangeSceneToFile("res://Scenes/map_interface.tscn");
        }
        else
        {
            LoadCurrentRoom();
        }
    }

    public int GetCurrentRoomIndex()
    {
        return _currentRoomIndex;
    }

    public string GetCurrentRoomType()
    {
        return _roomTypes[_currentRoomIndex];
    }
}
