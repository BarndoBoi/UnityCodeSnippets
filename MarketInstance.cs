using System;
using System.Collections.Generic;
using System.Text.Json;

public class MarketInstance
{
    public MarketCommodities MarketCommodities { get; private set; }
    public Dictionary<string, float> LocalMarketPrices { get; private set; }
    public List<Dictionary<string, float>> LastMarketPrices { get; private set; }
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
            LocalMarketPrices.Add(commodity.Name, commodity.BasePrice + GenerateRandomPrice(50.0, 200.0));
        }
    }

    // Update local market prices (simulate market fluctuations)
    public void UpdateMarketPrices()
    {
        LastMarketPrices.Add(new Dictionary<string, float>(LocalMarketPrices));
        if (LastMarketPrices.Count > HistoryLength)
        {
            //Remove the oldest data
            LastMarketPrices.RemoveAt(0);
        }
        foreach (string commodityName in LocalMarketPrices.Keys)
        {
            // Simulate random fluctuations in prices
            float priceChange = GenerateRandomPrice(-10.0f, 10.0f);
            LocalMarketPrices[commodityName] += priceChange;
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

    private float GenerateRandomPrice(double minValue, double maxValue)
    {
        return (float)(minValue + (maxValue - minValue) * random.NextDouble());
    }

}
