using System;
using System.Collections.Generic;
using System.Text.Json;

public class MarketInstance
{
    public MarketCommodities MarketCommodities { get; private set; }
    public Dictionary<Commodity, float> LocalMarketPrices { get; private set; }
    public List<Dictionary<Commodity, float>> LastMarketPrices { get; private set; }
    public List<SectorTrend> SectorTrends { get; } = new List<SectorTrend>();
    public Inventory MarketInventory { get; } = new Inventory();
    public List<Industry> Industries { get; } = new List<Industry>();
    public int LocationId { get; set; }
    public string Name { get; set; }

    private int updateCounter = 0;
    private int HistoryLength = 12;

    public MarketInstance()
    {
        MarketCommodities = new MarketCommodities();
        LocalMarketPrices = new Dictionary<Commodity, float>();
        LastMarketPrices = new List<Dictionary<Commodity, float>>();
    }

    // Initialize the market instance with commodities and random initial prices
    public void InitializeMarket(string commodityFilePath)
    {
        // Load commodities from a JSON file
        MarketCommodities.LoadFromJsonFile(commodityFilePath);

        // Initialize local market prices with random values
        foreach (Commodity commodity in MarketCommodities.CommoditiesList)
        {
            LocalMarketPrices.Add(commodity, commodity.BasePrice + Helpers.GenerateRandomRange(50.0, 100.0));
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

        LastMarketPrices.Add(new Dictionary<Commodity, float>(LocalMarketPrices));
        if (LastMarketPrices.Count > HistoryLength)
        {
            //Remove the oldest data
            LastMarketPrices.RemoveAt(0);
        }
        foreach (Commodity commodity in LocalMarketPrices.Keys)
        {
            //Simulate random fluctuations in prices
            float priceChangePercent = Helpers.GenerateRandomRange(commodity.ChangeRateMin, commodity.ChangeRateMax);
            ApplySectorTrend(commodity, ref priceChangePercent);
            //Try applying the change as a percentage instead of a flat change
            LocalMarketPrices[commodity] += Helpers.CalculatePercentageOf(LocalMarketPrices[commodity], priceChangePercent);
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

            Console.WriteLine("Sector trends imported successfully.");
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

    private void ApplySectorTrend(Commodity commodity, ref float priceChange)
    {
        if (MarketCommodities.CommoditiesList != null)
        {
            SectorTrend sectorTrend = SectorTrends.Find(st => st.SectorName == commodity.Sector);
            if (sectorTrend == null)
                Console.WriteLine("Error finding entry for sectorTrend named: " + commodity.Sector);
            else
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
