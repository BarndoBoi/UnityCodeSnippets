using System.Text.Json;

public class Inventory
{
    public Dictionary<Commodity, int> Goods { get; } = new Dictionary<Commodity, int>();

    public void AddGoods(Commodity commodity, int quantity)
    {
        if (Goods.ContainsKey(commodity))
        {
            Goods[commodity] += quantity;
        }
        else
        {
            Goods.Add(commodity, quantity);
        }
    }

    public bool RemoveGoods(Commodity commodity, int quantity)
    {
        if (Goods.ContainsKey(commodity) && Goods[commodity] >= quantity)
        {
            Goods[commodity] -= quantity;
            return true;
        }
        else
        {
            return false; // Not enough goods in the inventory
        }
    }

    public bool HasItem(Commodity commodity)
    {
        return Goods.ContainsKey(commodity);
    }

    public int GetQuantity(Commodity commodity)
    {
        return Goods.ContainsKey(commodity) ? Goods[commodity] : 0;
    }

    public void ExportInventory(string jsonFilePath)
    {
        // Create a new ImportData object to represent the data you want to export
        var exportData = new ImportData
        {
            Goods = Goods.Select(kv => new CommodityAmountEntry { Commodity = kv.Key, Quantity = kv.Value }).ToList()
        };

        // Convert the data to JSON
        string jsonString = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });

        // Write the JSON to the specified file
        File.WriteAllText(jsonFilePath, jsonString);
    }

    public void ImportInventory(string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            // Read the JSON from the file
            string jsonString = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON to the data structure
            var importData = JsonSerializer.Deserialize<ImportData>(jsonString);

            // Populate the Goods dictionary
            foreach (var entry in importData.Goods)
            {
                Goods[entry.Commodity] = entry.Quantity;
            }
        }
        else
        {
            throw new FileNotFoundException("The specified JSON file was not found.", jsonFilePath);
        }
    }
}
