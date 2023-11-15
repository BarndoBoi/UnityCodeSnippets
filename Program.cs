using System.Text.Json;

class Program
{

    private static List<Event> marketEvents = new List<Event>();
    private static BondsMarket bonds = new BondsMarket();
    private static int globalTrend = 0;
    private static int updateCounter = 0;
    private static int ticksToRollEvents = 4;
    private static int marketNumber = 1;
    private static Assets playerAssets = new Assets();

    static void Main()
    {
        MarketCommodities commodities = new MarketCommodities();

        // Load commodities from JSON file
        commodities.LoadFromJsonFile("market_commodities.json");

        // Access the list of commodities
        List<Commodity> commoditiesList = commodities.CommoditiesList;

        // ... Perform operations with the commodities list ...

        // Export the modified list back to a JSON file
        //commodities.ExportToJsonFile("updated_market_commodities.json");

        // Create a list of MarketInstance objects
        List<MarketInstance> markets = new List<MarketInstance>();

        //Example bonds for now
        Bond bond1 = new Bond("Bond A", 1000, 950, 5);
        Bond bond2 = new Bond("Bond B", 1200, 1100, 4);
        Bond bond3 = new Bond("Bond C", 800, 750, 6);
        bonds.IssueBond(bond1);
        bonds.IssueBond(bond2);
        bonds.IssueBond(bond3);

        //Graph code tests
        //Tests.RunPathfindingExample();

        //Fetch saved events and import
        marketEvents = ImportEvents("market_events.json");

        // Export events to a file
        //ExportEvents("market_events.json", marketEvents); //Commented out for now until new events need to be made



        // Add MarketInstances to the list up to the marketNumber
        for (int i = 0; i < marketNumber; i++)
        {
            MarketInstance marketInstance = new MarketInstance();
            marketInstance.InitializeMarket("market_commodities.json");
            markets.Add(marketInstance);
            PrintCommodityInfo(marketInstance);

        }

        // Update market prices and print changes
        WaitForUserInputAndUpdate(markets);
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
                var lastPrice = marketInstance.LastMarketPrices[marketInstance.LastMarketPrices.Count -1][commodityName];
                var oldestPrice = marketInstance.LastMarketPrices[0][commodityName];
                percentageChange = Helpers.CalculatePercentageChange(marketInstance.LastMarketPrices[marketInstance.LastMarketPrices.Count - 1][commodityName], currentPrice);
                Console.WriteLine($"Last price for {commodity.Name} is {marketInstance.LastMarketPrices[marketInstance.LastMarketPrices.Count - 1][commodityName]}");
                if (marketInstance.LastMarketPrices.Count > 1)
                {
                    historicChange = Helpers.CalculatePercentageChange(marketInstance.LastMarketPrices[0][commodityName], currentPrice);
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
            Console.WriteLine("1 - Update and Print");
            Console.WriteLine("2 - View Bonds Market");
            Console.WriteLine("Press any other key to exit.");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    // Update and print logic
                    CheckEventTrigger();
                    UpdateAndPrintChanges(markets);
                    if (bonds.DoUpdate)
                        bonds.UpdateBonds();
                    break;
                case "2":
                    // View Bonds Market logic
                    bonds.DisplayBonds();
                    break;
                default:
                    Console.WriteLine("Unrecognized input, exiting program.");
                    return;
            }
        }
    }

    private static void CheckEventTrigger()
    {
        if (updateCounter % ticksToRollEvents == 0)
        {
            // Simulate a random event trigger
            int randomEventIndex = Helpers.GenerateRandomRange(0, marketEvents.Count);
            Event randomEvent = marketEvents[randomEventIndex];

            // Trigger the event with the current global trend
            randomEvent.TriggerEvent(globalTrend);

            // Display event information in a rectangle of # characters
            Console.WriteLine(new string('#', 40));
            Console.WriteLine($"#{"Selected Event:",-38}#");
            Console.WriteLine($"#{randomEvent.Name,-38}#");
            Console.WriteLine(new string('#', 40));
            Console.WriteLine($"#{"Event Text:",-38}#");
            Console.WriteLine($"#{randomEvent.Text,-38}");
            Console.WriteLine(new string('#', 40));
        }
    }

    private static void ExportEvents(string fileName, List<Event> marketEvents)
    {
        string json = JsonSerializer.Serialize(marketEvents, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
        Console.WriteLine($"Events exported to {fileName}");
    }

    private static List<Event> ImportEvents(string fileName)
    {
        if (File.Exists(fileName))
        {
            string json = File.ReadAllText(fileName);
            List<Event> importedEvents = JsonSerializer.Deserialize<List<Event>>(json);
            Console.WriteLine($"Events imported from {fileName}");
            return importedEvents;
        }
        else
        {
            Console.WriteLine($"File {fileName} not found. Returning an empty list.");
            return new List<Event>();
        }
    }
}
