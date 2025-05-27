using Godot;
using System;

public partial class TimeLabel : Label
{
    public override void _Ready()
    {
        UpdateTimeLabel();
    }
    
    public void UpdateTimeLabel()
    {
        Text = $"Time: {GameManager.Instance.CurrentTimeOfDay}\nDay: {GameManager.Instance.DayCount}";
    }
    
    
    
}
