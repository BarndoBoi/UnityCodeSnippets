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

        // Add multiple MarketInstances to the list
        for (int i = 0; i < 3; i++)
        {
            MarketInstance marketInstance = new MarketInstance();
            marketInstance.InitializeMarket("market_commodities.json");
            markets.Add(marketInstance);
        }

        // Print commodity information and track percentage change
        PrintCommodityInfo(markets);

        // Update market prices and print changes
        WaitForUserInputAndUpdate(markets);

        string localPricesJson = JsonSerializer.Serialize(markets);
        System.IO.File.WriteAllText("local_market_prices.json", localPricesJson);
    }

    static void PrintCommodityInfo(List<MarketInstance> markets)
    {
        foreach (MarketInstance marketInstance in markets)
        {
            Console.WriteLine($"Market #{markets.IndexOf(marketInstance) + 1}");

            // Print commodity information
            foreach (var commodity in marketInstance.MarketCommodities.CommoditiesList)
            {
                Console.WriteLine($"{commodity.Name}: ${marketInstance.LocalMarketPrices[commodity.Name]:F2}");
            }

            Console.WriteLine();
        }
    }

    static void UpdateAndPrintChanges(List<MarketInstance> markets)
    {

        Console.WriteLine("After updating prices:");

        // Print commodity information and track percentage change
        foreach (MarketInstance marketInstance in markets)
        {
            Console.WriteLine($"MarketInstance #{markets.IndexOf(marketInstance) + 1}");

            // Print commodity information and track percentage change
            foreach (var commodity in marketInstance.MarketCommodities.CommoditiesList)
            {
                float oldPrice = marketInstance.LocalMarketPrices[commodity.Name];
                marketInstance.UpdateMarketPrices();
                float newPrice = marketInstance.LocalMarketPrices[commodity.Name];
                float percentageChange = ((newPrice - oldPrice) / oldPrice) * 100;

                Console.WriteLine($"{commodity.Name}: ${newPrice:F2} ({percentageChange:F2}%)");
            }

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
