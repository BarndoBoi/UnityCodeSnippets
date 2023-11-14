using System;
using System.Collections.Generic;
using System.Text.Json;

public class MarketInstance
{
    public MarketCommodities MarketCommodities { get; private set; }
    public Dictionary<string, float> LocalMarketPrices { get; private set; }
    public List<Dictionary<string, float>> LastMarketPrices { get; private set; }
    public List<SectorTrend> SectorTrends { get; } = new List<SectorTrend>();
    public Inventory MarketInventory { get; } = new Inventory();
    public int LocationId { get; set; }
    public string Name { get; set; }

    private int updateCounter = 0;
    private int HistoryLength = 12;
    private Random random = new Random();

    public MarketInstance()
    {
        MarketCommodities = new MarketCommodities();
        LocalMarketPrices = new Dictionary<string, float>();
        LastMarketPrices = new List<Dictionary<string, float>>();
    }

    // Initialize the market instance with commodities and random initial prices
    public void InitializeMarket(string commodityFilePath)
    {
        // Load commodities from a JSON file
        MarketCommodities.LoadFromJsonFile(commodityFilePath);

        // Initialize local market prices with random values
        foreach (Commodity commodity in MarketCommodities.CommoditiesList)
        {
            LocalMarketPrices.Add(commodity.Name, commodity.BasePrice + Helpers.GenerateRandomRange(50.0, 100.0));
        }

        ImportSectorTrends("sector_trends.json"); //Just for test purposes. Move this string into a config file or something later.

        UpdateSectorTrends();
    }

    // Update local market prices (simulate market fluctuations)
    public void UpdateMarketPrices()
    {
        // Increment the update counter
        updateCounter++;

        if (updateCounter % 4 == 0)
        {
            //Time to update the sector trends
            UpdateSectorTrends();
        }

        LastMarketPrices.Add(new Dictionary<string, float>(LocalMarketPrices));
        if (LastMarketPrices.Count > HistoryLength)
        {
            //Remove the oldest data
            LastMarketPrices.RemoveAt(0);
        }
        foreach (string commodityName in LocalMarketPrices.Keys)
        {
            //Simulate random fluctuations in prices
            Commodity commodity = MarketCommodities.GetCommodityByName(commodityName);
            float priceChangePercent = Helpers.GenerateRandomRange(commodity.ChangeRateMin, commodity.ChangeRateMax);
            ApplySectorTrend(commodityName, ref priceChangePercent);
            //Try applying the change as a percentage instead of a flat change
            LocalMarketPrices[commodityName] += Helpers.CalculatePercentageOf(LocalMarketPrices[commodityName], priceChangePercent);
            //LocalMarketPrices[commodityName] += priceChangePercent;
        }
    }

    public void ImportSectorTrends(string jsonFilePath)
{
    try
    {
        string jsonContent = File.ReadAllText(jsonFilePath);
        List<SectorTrend> sectorTrends = JsonSerializer.Deserialize<List<SectorTrend>>(jsonContent);

        // Replace the existing list with the new one
        SectorTrends.Clear();
        SectorTrends.AddRange(sectorTrends);

        Console.WriteLine("Sector trends imported successfully. Contents:");
        foreach (var trend in SectorTrends)
        {
            Console.WriteLine($"SectorName: {trend.SectorName}, ChangeMin: {trend.ChangeMin}, ChangeMax: {trend.ChangeMax}, CurrentTrend: {trend.CurrentTrend}");
        }
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine($"File not found: {jsonFilePath}");
    }
    catch (JsonException)
    {
        Console.WriteLine($"Error deserializing JSON from {jsonFilePath}");
    }
}

    public void ExportSectorTrends(string jsonFilePath)
    {
        try
        {
            string jsonContent = JsonSerializer.Serialize(SectorTrends, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonFilePath, jsonContent);

            Console.WriteLine("Sector trends exported successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting sector trends: {ex.Message}");
        }
    }

    private void UpdateSectorTrends()
    {
        foreach (SectorTrend sectorTrend in SectorTrends)
        {
            sectorTrend.CurrentTrend = Helpers.GenerateRandomRange(sectorTrend.ChangeMin, sectorTrend.ChangeMax);
        }
    }

    private void ApplySectorTrend(string commodityName, ref float priceChange)
    {
        if (MarketCommodities.CommoditiesList != null)
        {
            Commodity commodity = MarketCommodities.GetCommodityByName(commodityName);
            SectorTrend sectorTrend = SectorTrends.Find(st => st.SectorName == commodity.Sector);
            priceChange *= 1 + sectorTrend.CurrentTrend / 100.0f;
        }
    }

    // Export the market instance data to a JSON file
    public void ExportMarketInstanceToJson()
    {
        // Export market commodities data
        MarketCommodities.ExportToJsonFile("market_commodities.json");

        // Export local market prices data
        string pricesJson = JsonSerializer.Serialize(LocalMarketPrices);
        System.IO.File.WriteAllText("local_market_prices.json", pricesJson);
    }

}
