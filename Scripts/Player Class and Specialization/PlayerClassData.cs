using System.Collections.Generic;

public class PlayerClassData
{
    public string Name { get; set; }
    public PlayerClassType Type { get; set; }
    public Dictionary<string, Skill> Skills { get; set; }

    public PlayerClassData(string name = "", PlayerClassType type = PlayerClassType.None)
    {
        Name = name;
        Type = type;
        Skills = new Dictionary<string, Skill>();
    }
}