using System;
using System.Collections.Generic;

public class Industry
{
    public string Name { get; set; }
    public Dictionary<Commodity, int> OutputCommodities { get; } = new Dictionary<Commodity, int>();
    public Dictionary<Commodity, int> InputCommodities { get; } = new Dictionary<Commodity, int>();
    public Inventory IndustryInventory { get; } = new Inventory();

    public Industry(string name, Commodity outputCommodity, int outputAmount)
    {
        Name = name;
        OutputCommodities.Add(outputCommodity, outputAmount);
    }

    public void AddInputCommodity(Commodity inputCommodity, int quantity)
    {
        InputCommodities.Add(inputCommodity, quantity);
    }

    public void AddOutputCommodity(Commodity outputCommodity, int quantity){
        OutputCommodities.Add(outputCommodity, quantity);
    }

    public void ProcessOutput()
    {
        Console.WriteLine($"Processing output in {Name}");

        // Check if there are enough input goods in the inventory
        bool enoughInputGoods = true;
        foreach (var input in InputCommodities)
        {
            if (!IndustryInventory.RemoveGoods(input.Key, input.Value))
            {
                enoughInputGoods = false;
                break;
            }
        }

        // If there are enough input goods, produce the output
        if (enoughInputGoods)
        {
            foreach (var output in OutputCommodities)
            {
                Commodity outputCommodity = output.Key;
                int outputQuantity = output.Value;

                Console.WriteLine($"Producing {outputQuantity} units of {outputCommodity.Name}");

                // Add the produced goods to the industry's inventory
                IndustryInventory.AddGoods(outputCommodity, outputQuantity);
            }
        }
        else
        {
            Console.WriteLine($"Not enough input goods to produce output in {Name}");
        }
    }
}
