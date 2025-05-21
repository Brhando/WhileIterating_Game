using Godot;
using System;

public partial class SceneData : Node
{
    public string SpawnPointName = "default";

    public string GetSpawnPointName()
    {
        return SpawnPointName;
    }

    public void ChangeSpawnPointName(string newName)
    {
        SpawnPointName = newName;
    }
}