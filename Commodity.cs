using System;

public class Commodity
{
    public string Name { get; set; }
    public string Sector { get; set; }
    public ushort NetID { get; set; }
    public float BasePrice { get; set; }
    public int Amount { get; private set; }

    public Commodity(string name, ushort netID, float basePrice, string sector)
    {
        Name = name;
        NetID = netID;
        BasePrice = basePrice;
        Amount = 0;
        Sector = sector;
    }
}
