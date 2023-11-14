﻿using System.Text.Json;

class Program
{

    private static List<Event> marketEvents = new List<Event>();
    private static int globalTrend = 0;
    private static int updateCounter = 0;
    private static int ticksToRollEvents = 3;
    private static int marketNumber = 1;

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

        //Graph code tests
        //Tests.RunPathfindingExample();

        //Add some sample market events to the list

        marketEvents.Add(new Event(1, "Market Boom", "Stocks surge to record highs as investors celebrate unprecedented gains.", 10, 5));
        marketEvents.Add(new Event(2, "Market Crash", "Panic ensues as stock prices plummet, wiping out trillions in market value.", 5, -8));
        marketEvents.Add(new Event(3, "Railway Mania", "Investors caught up in the excitement of the railway boom, driving up stock prices to dizzying heights.", 10, 5));
        marketEvents.Add(new Event(4, "Economic Downturn", "Economic woes sweep through the city, leading to a market downturn and financial hardship.", 5, -8));
        marketEvents.Add(new Event(5, "Colonial Discovery", "Reports of a prosperous colony spark investment fever, fueling hopes of great returns.", 8, 3));
        marketEvents.Add(new Event(6, "Industrial Innovation", "A wave of new inventions and industrial breakthroughs revitalizes the market and boosts investor confidence.", 15, 6));
        marketEvents.Add(new Event(7, "Banking Panic", "A series of bank failures triggers widespread panic among investors and depositors.", 12, -7));
        marketEvents.Add(new Event(8, "Opium Crisis", "Trade imbalances and Opium Wars contribute to a financial crisis with repercussions in global markets.", 8, -4));
        marketEvents.Add(new Event(9, "Potato Blight", "The Potato Famine disrupts agricultural markets and leads to economic repercussions throughout the empire.", 10, -8));
        marketEvents.Add(new Event(10, "Railway Debacle", "Speculative investments in poorly planned railway projects result in financial ruin for many investors.", 12, -9));
        marketEvents.Add(new Event(11, "Tariff Troubles", "Tariff disputes and changes in import policies create uncertainty and negatively impact investments.", 10, -4));
        marketEvents.Add(new Event(12, "Factory Fires", "A series of devastating fires in numerous factories causes significant losses for investors and insurers.", 12, -7));
        marketEvents.Add(new Event(13, "Currency Devaluation", "Government decisions lead to currency devaluation, causing turmoil in financial markets and trade.", 15, -8));

        // Export events to a file
        ExportEvents("market_events.json", marketEvents);

        

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
                CheckEventTrigger();
                UpdateAndPrintChanges(markets);
                updateCounter++;
            }
            else
            {
                Console.WriteLine("Exiting program.");
                break;
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
