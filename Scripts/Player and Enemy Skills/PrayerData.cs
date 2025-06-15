using System.Collections.Generic;

public class PrayerData
{
    //dictionary to hold cycling logic
    public static Dictionary<PrayerType, (PrayerType Next, string Buff)> PrayerCycle = new()
    {
        { PrayerType.UnnamedAttackGod, (PrayerType.UnnamedDefenseGod, "Invigorated") },
        { PrayerType.UnnamedDefenseGod, (PrayerType.UnnamedAttackGod, "Stalwart") },
        { PrayerType.Mars, (PrayerType.Odin, "Mars's Pulse") },
        { PrayerType.Odin, (PrayerType.Montu, "Raven's Claws") },
        { PrayerType.Montu, (PrayerType.Mars, "Vampiric Feast") },
        { PrayerType.Anicetus, (PrayerType.Eir, "Strategic Knowledge") },
        { PrayerType.Eir, (PrayerType.Bastet, "Eir's Blessing") },
        { PrayerType.Bastet, (PrayerType.Anicetus, "Quick Reflexes") }
    };
}

