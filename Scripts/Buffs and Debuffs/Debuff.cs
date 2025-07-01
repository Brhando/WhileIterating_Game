using System;

public class Debuff
{
    public DebuffType Type {get; set;}
    public bool IsWeak = false;
    public bool IsReflect = false;
    public bool IsFatigue = false;
    public int Damage = 0;
    public int ReflectVal = 0;
    public int CountAmt = 0;
}