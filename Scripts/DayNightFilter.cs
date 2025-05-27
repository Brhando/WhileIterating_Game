using Godot;
using System;

public partial class DayNightFilter : CanvasLayer
{
    private ColorRect _tint;

    public override void _Ready()
    {
        _tint = GetNode<ColorRect>("Tint");
        UpdateTint();
    }

    private void OnTimeChanged(GameManager.TimeOfDay newTime)
    {
        UpdateTint();
    }

    private void UpdateTint()
    {
        var alpha = 0f;

        switch (GameManager.Instance.CurrentTimeOfDay)
        {
            case GameManager.TimeOfDay.Morning:
                alpha = 0.2f;
                break;
            case GameManager.TimeOfDay.MidMorning:
                alpha = 0.1f;
                break;
            case GameManager.TimeOfDay.Noon:
                alpha = 0.0f;
                break;
            case GameManager.TimeOfDay.Afternoon:
                alpha = 0.2f;
                break;
            case GameManager.TimeOfDay.Night:
                alpha = 0.45f;
                break;
        }

        _tint.Color = new Color(0, 0, 0, alpha);
    }
    
    
}
