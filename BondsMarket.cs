using System;
using System.Collections.Generic;

public class BondsMarket
{
    private List<Bond> availableBonds = new List<Bond>();
    private List<Bond> purchasedBonds = new List<Bond>();
    public bool DoUpdate => purchasedBonds.Count > 0;

    public void IssueBond(string name, float faceValue, float purchasePrice, int maturityPeriod)
    {
        Bond newBond = new Bond(name, faceValue, purchasePrice, maturityPeriod);
        availableBonds.Add(newBond);
        Console.WriteLine($"New bond issued: {newBond}");
    }

    public void IssueBond(Bond bond)
    {
        availableBonds.Add(bond);
        Console.WriteLine($"New bond issued: {bond}");
    }

    public void BondMaturedEvent(Bond bond)
    {
        if (purchasedBonds.Contains(bond))
        {
            // Assume simple logic for matured bond, remove it from purchased bonds
            purchasedBonds.Remove(bond);
            Console.WriteLine($"Bond matured: {bond}");
        }
        else
        {
            Console.WriteLine($"Bond not found in purchased bonds: {bond}");
        }
    }

    public void PurchaseBond(Bond bond)
    {
        if (availableBonds.Contains(bond))
        {
            // Move the bond from available bonds to purchased bonds
            availableBonds.Remove(bond);
            purchasedBonds.Add(bond);

            // Subscribe the BondMaturedEvent method to the onBondMatured event
            bond.SubscribeToMaturity(BondMaturedEvent);

            Console.WriteLine($"Bond purchased outright: {bond}");
        }
        else
        {
            Console.WriteLine($"Bond not available for purchase: {bond}");
        }
    }

    public void UpdateBonds()
    {
        // Simulate updating the values of purchased bonds
        foreach (var bond in purchasedBonds)
        {
            bond.CalculateReturnRate();
            bond.CheckMaturity();
            Console.WriteLine($"Updated bond: {bond}");
        }
    }

    public void DisplayBonds()
    {
        Console.WriteLine("Bonds on market:");
        foreach (var bond in availableBonds)
        {
            Console.WriteLine(bond);
        }
        if (purchasedBonds.Count != 0)
        {
            Console.WriteLine("Purchased Bonds:");
            foreach (var bond in purchasedBonds)
            {
                Console.WriteLine(bond);
            }
        }
    }
}
