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
        DebuffLibrary[DebuffType.Bleed] = new Debuff { Type = DebuffType.Bleed, CountAmt = 2, Damage = 5};
        DebuffLibrary[DebuffType.Deflect] = new Debuff { Type = DebuffType.Deflect, CountAmt = 1 };
        DebuffLibrary[DebuffType.Poison] = new Debuff { Type = DebuffType.Poison, CountAmt = 1 };
        DebuffLibrary[DebuffType.Weak] = new Debuff { Type = DebuffType.Weak, CountAmt = 1, Damage = 10};
        DebuffLibrary[DebuffType.BloodFatigue] = new Debuff { Type = DebuffType.BloodFatigue, CountAmt = 1 };
        DebuffLibrary[DebuffType.Disarm] = new Debuff { Type = DebuffType.Disarm, CountAmt = 1 };
    }
}