using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class MarketCommodities
{
    public List<Commodity> CommoditiesList { get; private set; }

    public MarketCommodities() => CommoditiesList = new List<Commodity>();

    // Populate the CommoditiesList from a JSON file
    public void LoadFromJsonFile(string filePath)
    {
        try
        {
            string jsonString = File.ReadAllText(filePath);
            CommoditiesList = JsonSerializer.Deserialize<List<Commodity>>(jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from JSON file: {ex.Message}");
        }
    }

    // Export the contents of the CommoditiesList to a JSON file
    public void ExportToJsonFile(string filePath)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(CommoditiesList);
            File.WriteAllText(filePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting to JSON file: {ex.Message}");
        }
    }

    public Commodity GetCommodityByName(string commodityName)
    {
        foreach (Commodity commodity in CommoditiesList)
        {
            if (commodity.Name == commodityName)
            {
                // Commodity with the specified name found
                return commodity;
            }
        }

        // Commodity with the specified name not found
        Console.WriteLine($"Commodity with name '{commodityName}' not found.");
        return null;
    }
}