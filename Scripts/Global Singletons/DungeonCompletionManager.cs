using Godot;

public partial class DungeonCompletionManager : Node
{
    public static DungeonCompletionManager Instance;

    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();
        
    }
    public bool DungeonDefeated = false;

    public void HandleDungeonEnd(bool success)
    {
        if (success)
        {
            DungeonManager.Instance.IncreaseCurrentDungeonLevel();
            DungeonManager.Instance.IncreaseEnemiesLevel();
            //Add summary or reward screen
            //normalize devotion points earned in combat
            DevotionTree.Instance.NormalizeDevotionPoints();
        }

        SceneManager.Instance.Change("res://Scenes/map_interface.tscn");
    }
}