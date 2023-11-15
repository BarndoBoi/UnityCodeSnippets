public class Assets
{
    // Fields
    public float liquidMoney {private set; get;}
    public float assetsInBonds {private set; get;}
    public float assetsInInventory {private set; get;}
    private List<Bond> ownedBonds = new List<Bond>();

    // Properties
    public float NetTotal => liquidMoney + assetsInBonds + assetsInInventory;

    // Constructor
    public Assets(float initialLiquidMoney = 0, float initialAssetsInBonds = 0, float initialAssetsInInventory = 0)
    {
        liquidMoney = initialLiquidMoney;
        assetsInBonds = initialAssetsInBonds;
        assetsInInventory = initialAssetsInInventory;
    }

    // Methods to update values
    public void ChangeLiquidMoney(float amount)
    {
        liquidMoney += amount;
        Console.WriteLine($"Adding {amount} to liquid money. Net total is now {NetTotal}");
    }

    public void AddOwnedBond(Bond bond)
    {
        ownedBonds.Add(bond);
    }

    public float GetBondsValue()
    {
        assetsInBonds = 0;
        foreach (Bond bond in ownedBonds)
        {
            assetsInBonds += bond.CurrentValue;
        }
        return assetsInBonds;
    }

    public void AddAssetsInInventory(float amount)
    {
        assetsInInventory += amount;
    }
}