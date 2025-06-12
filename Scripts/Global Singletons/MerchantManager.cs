using Godot;
using System.Collections.Generic;
public partial class MerchantManager : Node
{
    public static MerchantManager Instance;
    public Dictionary<string, MerchantInventory> Merchants = new();
    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();

        LoadMerchants();
    }

    private void LoadMerchants()
    {
        var doc = new MerchantInventory("doc");
        var generalist = new MerchantInventory("generalist");
        
        doc.AddItem("bandage");
        doc.AddItem("stone_plate");
        doc.AddItem("gold");
        generalist.AddItem("stone_chunk", 5);
        generalist.AddItem("wood_log", 3);
        generalist.AddItem("plank");
        generalist.AddItem("gold");
        
        Merchants.Add("doc", doc);
        Merchants.Add("generalist", generalist);
    }
    
    public MerchantInventory GetMerchant(string merchantId)
    {
        if (Merchants.ContainsKey(merchantId))
            return Merchants[merchantId];

        GD.PrintErr("Merchant not found: " + merchantId);
        return null;
    }
    
    
}