using Godot;
using System.Collections.Generic;

public partial class DebuffData : Node
{
    public static DebuffData Instance;

    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();

        InitializeDebuffs();
    }

    public Dictionary<DebuffType, Debuff> DebuffLibrary = new();
    private void InitializeDebuffs()
    {
        DebuffLibrary[DebuffType.Bleed] = new Debuff { Type = DebuffType.Bleed };
        DebuffLibrary[DebuffType.Deflect] = new Debuff { Type = DebuffType.Deflect };
        DebuffLibrary[DebuffType.Poison] = new Debuff { Type = DebuffType.Poison };
        DebuffLibrary[DebuffType.Weak] = new Debuff { Type = DebuffType.Weak };
        DebuffLibrary[DebuffType.BloodFatigue] = new Debuff { Type = DebuffType.BloodFatigue };
    }
}