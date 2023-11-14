using System;
using System.Collections.Generic;
using System.Text.Json;

public class MarketInstance
{
    public MarketCommodities MarketCommodities { get; private set; }
    public Dictionary<string, float> LocalMarketPrices { get; private set; }
    public List<Dictionary<string, float>> LastMarketPrices { get; private set; }
    public Dictionary<string, float> SectorTrend { get; private set; }
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
            float commodityBaseTrend = MarketCommodities.GetCommodityByName(commodityName).BaseFluctuationRange;
            float priceChangeFromTrend = Helpers.CalculatePercentageOf(LocalMarketPrices[commodityName], commodityBaseTrend);
            float priceChange = Helpers.GenerateRandomRange(priceChangeFromTrend * -1, priceChangeFromTrend);
            ApplySectorTrend(commodityName, ref priceChange);
            LocalMarketPrices[commodityName] += priceChange;
        }
    }

    private void UpdateSectorTrends()
    {
        SectorTrend = new Dictionary<string, float>();
        foreach (Commodity commodity in MarketCommodities.CommoditiesList)
        {
            if (SectorTrend.ContainsKey(commodity.Sector))
                continue;
            else
            {
                SectorTrend.Add(commodity.Sector, Helpers.GenerateRandomRange(-5f, 5f));
            }
        }
    }

    private void ApplySectorTrend(string commodityName, ref float priceChange)
    {
        if (MarketCommodities.CommoditiesList != null)
        {
            Commodity commodity = MarketCommodities.CommoditiesList.Find(c => c.Name == commodityName);

            if (commodity != null && SectorTrend.ContainsKey(commodity.Sector))
            {
                // Apply sector trend to the price change
                priceChange *= (1 + SectorTrend[commodity.Sector] / 100.0f);
            }
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
