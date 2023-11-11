using System;

public class Commodity
{
    public string Name { get; set; }
    public ushort NetID { get; set; }
    public float BasePrice { get; set; }
    public int Amount { get; private set; }

    public Commodity(string name, ushort netID, float basePrice)
    {
        Name = name;
        NetID = netID;
        BasePrice = basePrice;
        Amount = 0;
    }
}
