public class Inventory
{
    public Dictionary<Commodity, int> Goods { get; } = new Dictionary<Commodity, int>();

    public void AddGoods(Commodity commodity, int quantity)
    {
        if (Goods.ContainsKey(commodity))
        {
            Goods[commodity] += quantity;
        }
        else
        {
            Goods.Add(commodity, quantity);
        }
    }

    public bool RemoveGoods(Commodity commodity, int quantity)
    {
        if (Goods.ContainsKey(commodity) && Goods[commodity] >= quantity)
        {
            Goods[commodity] -= quantity;
            return true;
        }
        else
        {
            return false; // Not enough goods in the inventory
        }
    }
}