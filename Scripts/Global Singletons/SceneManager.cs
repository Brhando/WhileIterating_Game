using Godot;

public partial class SceneManager : Node
{
    public static SceneManager Instance;

    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();
        
    }

    public void Change(string path)
    {
        GetTree().ChangeSceneToFile(path);
    }
    
}