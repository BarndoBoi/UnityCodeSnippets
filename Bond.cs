using System;

public class Bond
{
    public string Name { get; set; }
    public float FaceValue { get; set; }
    public float PurchasePrice { get; set; }
    public float CurrentValue { get; private set; }
    public int MaturityPeriod { get; set; }
    public int RemainingPeriods { get; private set; }

    // Event to be triggered when the bond matures
    public event EventHandler BondMatured;

    public Bond(string name, float faceValue, float purchasePrice, int maturityPeriod)
    {
        Name = name;
        FaceValue = faceValue;
        PurchasePrice = purchasePrice;
        CurrentValue = purchasePrice; // Initial value is the purchase price
        MaturityPeriod = maturityPeriod;
        RemainingPeriods = maturityPeriod;
    }

    public void CalculateReturnRate()
    {
        // Simulate fluctuation in return rate (for demonstration purposes)
        Random random = new Random();
        double fluctuation = random.NextDouble() * 0.1 - 0.05; // Fluctuate between -5% and 5%
        double newReturnRate = fluctuation + (CurrentValue / PurchasePrice - 1);

        Console.WriteLine($"Return rate fluctuation: {fluctuation * 100:F2}%");
        Console.WriteLine($"New Return Rate: {newReturnRate * 100:F2}%");
    }

    public void CheckMaturity()
    {
        RemainingPeriods--;

        if (RemainingPeriods == 0)
        {
            Console.WriteLine($"Bond '{Name}' has matured!");

            // Trigger BondMatured event
            OnBondMatured(EventArgs.Empty);
        }
        else
        {
            Console.WriteLine($"Bond '{Name}' has {RemainingPeriods} periods remaining.");
        }
    }

    protected virtual void OnBondMatured(EventArgs e)
    {
        // Trigger the BondMatured event
        BondMatured?.Invoke(this, e);
    }
}