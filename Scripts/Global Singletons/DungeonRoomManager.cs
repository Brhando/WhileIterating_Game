using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonRoomManager : Node
{
    public static DungeonRoomManager Instance;

    private int _currentRoomIndex = 0;
    private List<RoomNode> _rooms = new(); // Example: Enemy, Rest, Enemy, ...
    private SceneLoader _sceneLoader = new();

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
        _rooms = GenerateRoomTypes(); // Decide layout
        LoadCurrentRoom();
    }

    private List<RoomNode> GenerateRoomTypes()
    {
        var layout = new List<RoomNode>();

        for (int i = 0; i < DungeonManager.Instance.ActiveDungeonEncounters.Count; i++)
        {
            var type = (i != 0 && i % 3 == 2) ? RoomType.Rest : RoomType.Enemy; //spawn a 'rest' room every third room
            layout.Add(new RoomNode(type));
        }
        
        //var room = new RoomNode(RoomType.Boss)
        //layout.Add(room); 
        return layout;
    }

    public void LoadCurrentRoom()
    {
        var roomType = _rooms[_currentRoomIndex].Type;
        _sceneLoader.LoadDungeonRoom(roomType);
    }

    public void NextRoom()
    {
        _currentRoomIndex++;

        if (_currentRoomIndex >= _rooms.Count)
        {
            GD.Print("Dungeon complete!");
            DungeonCompletionManager.Instance.HandleDungeonEnd(true);
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

    public RoomType GetCurrentRoomType()
    {
        return _rooms[_currentRoomIndex].Type;
    }
}
