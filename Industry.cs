using System;
using System.Collections.Generic;

public class Industry
{
    public string Name { get; set; }
    public Commodity OutputCommodity { get; set; }
    public int OutputAmount { get; set; }
    public List<Commodity> InputCommodities { get; } = new List<Commodity>();
    public Inventory IndustryInventory { get; } = new Inventory();

    public Industry(string name, Commodity outputCommodity, int outputAmount)
    {
        Name = name;
        OutputCommodity = outputCommodity;
        OutputAmount = outputAmount;
    }

    public void AddInputCommodity(Commodity inputCommodity)
    {
        InputCommodities.Add(inputCommodity);
    }

    public void ProcessOutput()
    {
        Console.WriteLine($"Processing output in {Name}");

        // Check if there are enough input goods in the inventory
        bool enoughInputGoods = true;
        foreach (var inputCommodity in InputCommodities)
        {
            if (!IndustryInventory.RemoveGoods(inputCommodity, 1))
            {
                enoughInputGoods = false;
                break;
            }
        }

        // If there are enough input goods, produce the output
        if (enoughInputGoods)
        {
            Console.WriteLine($"Producing {OutputAmount} units of {OutputCommodity.Name}");

            // Add the produced goods to the industry's inventory
            IndustryInventory.AddGoods(OutputCommodity, OutputAmount);
        }
        else
        {
            Console.WriteLine($"Not enough input goods to produce output in {Name}");
        }
    }
}