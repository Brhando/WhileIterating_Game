using Godot;
using System;

public partial class SceneData : Node
{
    public static SceneData DataInstance;
    public string SpawnPointName = "default";

    public override void _Ready()
    {
        if (DataInstance == null)
        {
            DataInstance = this;
        }
        else
        {
            QueueFree();
        }
    }
    
    public string GetSpawnPointName()
    {
        return SpawnPointName;
    }

    public void ChangeSpawnPointName(string newName)
    {
        SpawnPointName = newName;
    }
}