using System.Text.Json;

class Program
{
    static void Main()
    {
        MarketCommodities commodities = new MarketCommodities();

        // Load commodities from JSON file
        commodities.LoadFromJsonFile("market_commodities.json");

        // Access the list of commodities
        List<Commodity> commoditiesList = commodities.CommoditiesList;

        // ... Perform operations with the commodities list ...

        // Export the modified list back to a JSON file
        commodities.ExportToJsonFile("updated_market_commodities.json");

        // Create a list of MarketInstance objects
        List<MarketInstance> markets = new List<MarketInstance>();

        int marketNumber = 3;

        // Add multiple MarketInstances to the list
        for (int i = 0; i < marketNumber; i++)
        {
            MarketInstance marketInstance = new MarketInstance();
            marketInstance.InitializeMarket("market_commodities.json");
            markets.Add(marketInstance);
            PrintCommodityInfo(marketInstance);

        }

        // Update market prices and print changes
        WaitForUserInputAndUpdate(markets);

        string localPricesJson = JsonSerializer.Serialize(markets);
        System.IO.File.WriteAllText("local_market_prices.json", localPricesJson);
    }

    static void PrintCommodityInfo(MarketInstance marketInstance)
    {
        foreach (var commodity in marketInstance.MarketCommodities.CommoditiesList)
        {
            string commodityName = commodity.Name;

            float currentPrice = marketInstance.LocalMarketPrices[commodityName];
            float percentageChange = 0;
            float historicChange = 0;
            if (marketInstance.LastMarketPrices.Count != 0)
            {
                percentageChange = CalculatePercentageChange(marketInstance.LastMarketPrices[marketInstance.LastMarketPrices.Count - 1][commodityName], currentPrice);
                if (marketInstance.LastMarketPrices.Count > 1)
                {
                    historicChange = CalculatePercentageChange(marketInstance.LastMarketPrices[0][commodityName], currentPrice);
                }
            }
            if (marketInstance.LastMarketPrices.Count > 1)
            {
                Console.WriteLine($"{commodityName}: ${currentPrice:F2} ({(percentageChange >= 0 ? "+" : "")}{percentageChange:F2}%) {marketInstance.LastMarketPrices.Count} Week Change: ({(historicChange >= 0 ? "+" : "")}{historicChange:F2}%)");
            }
            else
                Console.WriteLine($"{commodityName}: ${currentPrice:F2} ({(percentageChange >= 0 ? "+" : "")}{percentageChange:F2}%)");
        }
    }

    static float CalculatePercentageChange(float initialValue, float currentValue)
    {
        if (float.IsNaN(initialValue) || initialValue == 0)
        {
            // Avoid division by zero or NaN when there is no initial value
            return float.NaN;
        }

        return ((currentValue - initialValue) / initialValue) * 100.0f;
    }

    static void UpdateAndPrintChanges(List<MarketInstance> markets)
    {
        // Update market prices
        foreach (MarketInstance marketInstance in markets)
        {
            marketInstance.UpdateMarketPrices();
        }
        // Print commodity information and display weighted average prices
        foreach (MarketInstance marketInstance in markets)
        {
            Console.WriteLine($"Market #{markets.IndexOf(marketInstance) + 1}");
            PrintCommodityInfo(marketInstance);
            Console.WriteLine();
        }
    }

    static void WaitForUserInputAndUpdate(List<MarketInstance> markets)
    {
        while (true)
        {
            Console.WriteLine("Press '1' to update prices again. Press any other key to exit.");
            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                UpdateAndPrintChanges(markets);
            }
            else
            {
                Console.WriteLine("Exiting program.");
                break;
            }
        }
    }
}
