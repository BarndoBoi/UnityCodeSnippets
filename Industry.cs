using System.Text.Json;

public class Industry
{

    public string IndustryName { get; set; }
    public string Sector { get; set; }

    public Inventory Inventory { get; } = new Inventory();
    public Recipe ProductionRecipe { get; } = new Recipe();

    public bool TryProcessOutput(out Dictionary<Commodity, int> createdGoods)
    {
        if (ProductionRecipe.InputGoods.All(input => Inventory.Goods.ContainsKey(input.Key) && Inventory.Goods[input.Key] >= input.Value))
        {
            foreach (var input in ProductionRecipe.InputGoods)
            {
                Inventory.Goods[input.Key] -= input.Value;
            }

            foreach (var output in ProductionRecipe.OutputGoods)
            {
                if (Inventory.Goods.ContainsKey(output.Key))
                    Inventory.Goods[output.Key] += output.Value;
                else
                    Inventory.Goods.Add(output.Key, output.Value);
            }

            createdGoods = new Dictionary<Commodity, int>(ProductionRecipe.OutputGoods);
            return true;
        }
        else
        {
            createdGoods = null;
            return false;
        }
    }

    public void ExportIndustry(string jsonFilePath)
    {
        var exportData = new ImportData
        {
            Goods = Inventory.Goods.Select(kv => new CommodityAmountEntry { Commodity = kv.Key, Quantity = kv.Value }).ToList()
        };

        string jsonString = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(jsonFilePath, jsonString);
    }

    public void ImportIndustry(string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            var importData = JsonSerializer.Deserialize<ImportData>(jsonString);

            foreach (var entry in importData.Goods)
            {
                Inventory.Goods[entry.Commodity] = entry.Quantity;
            }
        }
        else
        {
            throw new FileNotFoundException("The specified JSON file was not found.", jsonFilePath);
        }
    }
}