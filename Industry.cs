using System;
using System.Collections.Generic;

public class Industry
{
    public string Name { get; set; }
    public Commodity OutputCommodity { get; set; }
    public int OutputAmount { get; set; }
    public List<Commodity> InputCommodities { get; } = new List<Commodity>();

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
        Console.WriteLine($"Producing {OutputAmount} units of {OutputCommodity.Name}");

        foreach (var inputCommodity in InputCommodities)
        {
            Console.WriteLine($"Consuming {inputCommodity.Name}");
        }
    }
}