using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public class BondsMarket
{
        private List<Bond> availableBonds = new List<Bond>();
        private List<Bond> purchasedBonds = new List<Bond>();
        private List<Bond> matureBonds = new List<Bond>();
        private List<Bond> allBonds = new List<Bond>();
        public bool DoUpdate => purchasedBonds.Count > 0;
        public int AvailibleBondsCount => availableBonds.Count;
        public Bond GetAvailibleBond(int index) => availableBonds[index];
        public Bond GetPurchasedBond(int index) => purchasedBonds[index];
        public Bond GetBondByName(string name) => allBonds.Find(bond => bond.Name == name);

    public void IssueBond(Bond bond)
    {
        availableBonds.Add(bond);
        allBonds.Add(bond);
        Console.WriteLine($"New bond issued: {bond.Name}");
    }

    public void BondMaturedEvent(Bond bond)
    {
        if (purchasedBonds.Contains(bond))
        {
            // Assume simple logic for matured bond, remove it from purchased bonds
            purchasedBonds.Remove(bond);
            allBonds.Remove(bond);
            bond.owned.ChangeLiquidMoney(bond.CurrentValue); //Add the value of the bond back into liquid money
        }
        else
        {
            Console.WriteLine($"Bond not found in purchased bonds: {bond}");
        }
    }

    public void PurchaseBond(Bond bond, Assets owner)
    {
        if (availableBonds.Contains(bond))
        {
            // Move the bond from available bonds to purchased bonds
            availableBonds.Remove(bond);
            purchasedBonds.Add(bond);
            bond.owned = owner;

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
            if (bond.CheckMaturity())
                matureBonds.Add(bond);
        }
        if (matureBonds.Count != 0){
            foreach (var bond in matureBonds){
                bond.MatureBond();
            }
            matureBonds.Clear();
        }
    }

    public void DisplayBonds()
    {
        Console.WriteLine("Bonds on market:");
        int num = 1;
        foreach (var bond in availableBonds)
        {
            Console.WriteLine(num + ": " + bond.Name + " at price: " + bond.PurchasePrice);
            num++;
        }
        if (purchasedBonds.Count != 0)
        {
            Console.WriteLine("Purchased Bonds:");
            foreach (var bond in purchasedBonds)
            {
                Console.WriteLine(bond.Name + " bought at: " + bond.PurchasePrice + " and current price is: " + bond.CurrentValue);
            }
        }
    }
}
