using Godot;
using System;

public partial class SleepScene : CanvasLayer
{
    private Label _dayLabel;
    private Label _dungeonLevelLabel;
    private RichTextLabel _dreamText;
    private Button _wakeButton;
    private ColorRect _fadeOverlay;

    public override void _Ready()
    {
        _dayLabel = GetNode<Label>("DayCount");
        _dungeonLevelLabel = GetNode<Label>("DungeonLevel");
        _dreamText = GetNode<RichTextLabel>("DreamText");
        _wakeButton = GetNode<Button>("WakeButton");
        _fadeOverlay = GetNode<ColorRect>("FadeOverlay");
        
        
        _dayLabel.Text = "Day: " + (GameManager.Instance.DayCount - 1);
        _dungeonLevelLabel.Text = "DungeonLevel: " + DungeonManager.Instance.CurrentDungeonLevel;
        _wakeButton.Text = "Start Day: " + GameManager.Instance.DayCount;
        
        _wakeButton.Pressed += OnWakeButtonPressed;
        _wakeButton.Disabled = true;
        
        // Fade in
        var tween = GetTree().CreateTween();
        tween.TweenProperty(_fadeOverlay, "modulate:a", 0.0f, 1.5f)
            .SetEase(Tween.EaseType.InOut)
            .SetTrans(Tween.TransitionType.Sine);

        tween.Finished += OnFadeInFinished;
    }

    private async void DisplayInitialText()
    {
        for (var i = 0; i < DreamManager.Instance.DreamTextList1.Count; i++)
        {
            _dreamText.Text = DreamManager.Instance.GetDreamTextInitial(i);
            await ToSignal(GetTree().CreateTimer(2), "timeout");
        }

        // Display stat-related tracker lines
        foreach (var statLine in DreamManager.Instance.DreamTextList2)
        {
            _dreamText.Text = statLine;
            await ToSignal(GetTree().CreateTimer(2), "timeout");
        }

        DisplayFlavorText();
    }


    private void DisplayFlavorText()
    {
        _dreamText.Text = DreamManager.Instance.GetDreamText();
        _wakeButton.Disabled = false;
    }
    
    private void OnFadeInFinished()
    {
        DisplayInitialText();
    }

    private void OnWakeButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/home_inside.tscn");
    }
}
