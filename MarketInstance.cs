using System;
using System.Collections.Generic;
using System.Text.Json;

public class MarketInstance
{
    public MarketCommodities MarketCommodities { get; private set; }
    public Dictionary<Commodity, float> LocalMarketPrices { get; private set; }
    public List<Dictionary<Commodity, float>> LastMarketPrices { get; private set; }
    public MarketPrices MarketPrices { get; private set; }
    public Dictionary<Commodity, int> localCommodityBalance = new Dictionary<Commodity, int>();
    private Dictionary<Commodity, int> lastCommodityBalance;
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
        MarketPrices = new MarketPrices();
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

        MarketPrices.Initialize(MarketCommodities.CommoditiesList);

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

        //Calculate local industry needs
        if (localCommodityBalance.Count != 0)
            lastCommodityBalance = new Dictionary<Commodity, int> (localCommodityBalance); //Only store a last balance if it exists
        
        localCommodityBalance.Clear();
        foreach (Industry industry in Industries)
        {
            foreach (Commodity commodity in industry.ProductionRecipe.InputGoods.Keys)
            {
                if (localCommodityBalance.ContainsKey(commodity))
                {
                    localCommodityBalance[commodity] -= industry.ProductionRecipe.InputGoods[commodity]; //Subtract required inputs from the balance
                }
                else
                {
                    localCommodityBalance.Add(commodity, industry.ProductionRecipe.InputGoods[commodity] * -1); //Invert the value we add so inputs are subtractive
                }
            }
            foreach (Commodity commodity in industry.ProductionRecipe.OutputGoods.Keys)
            {
                if (localCommodityBalance.ContainsKey(commodity))
                {
                    localCommodityBalance[commodity] += industry.ProductionRecipe.OutputGoods[commodity]; //Add outputs to the balance
                }
                else
                {
                    localCommodityBalance.Add(commodity, industry.ProductionRecipe.InputGoods[commodity]);
                }
            }
        }

        foreach (Commodity commodity in MarketCommodities.CommoditiesList)
        {
            //Need to check the localCommodityBalance to determine price changes from demand
            var balance = localCommodityBalance[commodity];
            //Now I gotta figure out how to turn this into a percentage demand based on change from last balance
            var previousBalance = 0;
            if (lastCommodityBalance != null)
                previousBalance = lastCommodityBalance[commodity];
            
            //Then need to apply the SectorTrend to the price
            //Finally update the MarketPrices with the new value
        }

        /*foreach (Commodity commodity in LocalMarketPrices.Keys)
        {
            //Simulate random fluctuations in prices
            float priceChangePercent = Helpers.GenerateRandomRange(commodity.ChangeRateMin, commodity.ChangeRateMax);
            ApplySectorTrend(commodity, ref priceChangePercent);
            //Try applying the change as a percentage instead of a flat change
            LocalMarketPrices[commodity] += Helpers.CalculatePercentageOf(LocalMarketPrices[commodity], priceChangePercent);
            //LocalMarketPrices[commodityName] += priceChangePercent;
        }*/
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
