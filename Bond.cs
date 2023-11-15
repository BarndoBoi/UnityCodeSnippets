using System;

public class Bond
{
    public string Name { get; set; }
    public float PurchasePrice { get; set; }
    public float CurrentValue { get; private set; }
    public int MaturityPeriod { get; set; }
    public int RemainingPeriods { get; private set; }
    public float MinRange { get; set; }
    public float MaxRange { get; set; }
    public Assets owned;
    private Random random = new Random();

    // Custom delegate type for handling bond maturity
    public delegate void BondMaturedDelegate(Bond bond);

    // Event to be triggered when the bond matures
    private BondMaturedDelegate onBondMatured;

    public Bond(string name, float purchasePrice, int maturityPeriod, float minRange, float maxRange)
    {
        Name = name;
        PurchasePrice = purchasePrice;
        CurrentValue = purchasePrice; // Initial value is the purchase price
        MaturityPeriod = maturityPeriod;
        RemainingPeriods = maturityPeriod;
        MinRange = minRange;
        MaxRange = maxRange;
    }

    public void CalculateReturnRate()
    {
        // Simulate fluctuation in return rate (for demonstration purposes)
        double fluctuation = random.NextDouble() * (MaxRange - MinRange) + MinRange; // Fluctuate between MinRange and MaxRange
        double newReturnRate = fluctuation + (CurrentValue / PurchasePrice - 1);

        // Update CurrentValue based on the fluctuation
        CurrentValue *= (float)(1 + newReturnRate);
    }

    public bool CheckMaturity()
    {
        RemainingPeriods--;

        if (RemainingPeriods == 0)
        {
            return true;
        }
        else
        {
            Console.WriteLine($"Bond '{Name}' has {RemainingPeriods} periods remaining.");
            return false;
        }
    }

    public void MatureBond()
    {
        if (RemainingPeriods == 0)
        {
            Console.WriteLine($"Bond '{Name}' has matured!");

            // Trigger onBondMatured delegate
            onBondMatured?.Invoke(this);
        }
    }

    // Subscribe a method to the onBondMatured delegate
    public void SubscribeToMaturity(BondMaturedDelegate handler)
    {
        onBondMatured += handler;
    }

    // Unsubscribe a method from the onBondMatured delegate
    public void UnsubscribeFromMaturity(BondMaturedDelegate handler)
    {
        onBondMatured -= handler;
    }
}
