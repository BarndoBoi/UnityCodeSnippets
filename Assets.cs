public class Assets
{
    // Fields
    private float liquidMoney;
    private float assetsInBonds;
    private float assetsInInventory;

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
    public void AddLiquidMoney(float amount)
    {
        liquidMoney += amount;
    }

    public void AddAssetsInBonds(float amount)
    {
        assetsInBonds += amount;
    }

    public void AddAssetsInInventory(float amount)
    {
        assetsInInventory += amount;
    }
}