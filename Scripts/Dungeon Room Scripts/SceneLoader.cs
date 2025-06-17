using Godot;

public class SceneLoader
{
    public void LoadDungeonRoom(RoomType type)
    {
        string path = type switch
        {
            RoomType.Enemy => "res://Scenes/Dungeon Rooms/enemy_room.tscn",
            RoomType.Rest => "res://Scenes/Dungeon Rooms/rest_room.tscn",
            RoomType.Boss => "res://Scenes/Dungeon Rooms/boss_room.tscn",
            _ => "res://Scenes/Dungeon Rooms/enemy_room.tscn"
        };

        GD.Print($"Loading scene: {path}");
        SceneManager.Instance?.Change(path);
    }
}