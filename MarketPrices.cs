using System;
using System.Collections.Generic;

public class MarketPrices
{
    private Dictionary<Commodity, float> currentPrices = new Dictionary<Commodity, float>();
    private Dictionary<Commodity, Queue<float>> priceHistory = new Dictionary<Commodity, Queue<float>>();

    public void Initialize(List<Commodity> commodities){
        // Initialize current prices from the commodity's change rate
        foreach (var commodity in commodities)
        {
            float priceChangePercent = Helpers.GenerateRandomRange(commodity.ChangeRateMin, commodity.ChangeRateMax);
            currentPrices[commodity] = commodity.BasePrice + Helpers.CalculatePercentageOf(commodity.BasePrice, priceChangePercent);
            priceHistory[commodity] = new Queue<float>(capacity: 52);
        }
    }

    public float GetPrice(Commodity commodity)
    {
        return currentPrices[commodity];
    }

    public void UpdatePrice(Commodity commodity, float newPrice)
    {
        // Update current price
        currentPrices[commodity] = newPrice;

        // Update price history
        UpdatePriceHistory(commodity, newPrice);
    }

    private void UpdatePriceHistory(Commodity commodity, float newPrice)
    {
        var history = priceHistory[commodity];

        // Add the new price to the history
        history.Enqueue(newPrice);

        // Maintain a rolling window of the last 52 prices
        if (history.Count > 52)
        {
            history.Dequeue();
        }
    }

    public List<float> GetPriceHistory(Commodity commodity)
    {
        return new List<float>(priceHistory[commodity]);
    }
}
