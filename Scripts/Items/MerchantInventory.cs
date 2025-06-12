using System.Collections.Generic;

public class MerchantInventory
{
    public string MerchantID { get; set; }
    public Dictionary<string, int> Inventory { get; set; }  // ItemID, Quantity (-1 = infinite)
    
    //constructor
    public MerchantInventory(string merchantID)
    {
        MerchantID = merchantID;
        Inventory = new Dictionary<string, int>();
    }
    
    //functions
    public void AddItem(string itemID, int quantity = -1)
    {
        if (!Inventory.ContainsKey(itemID))
            Inventory.Add(itemID, quantity);
        else
            Inventory[itemID] += quantity;
    }

    public bool HasItem(string itemID)
    {
        return Inventory.ContainsKey(itemID);
    }
    
}