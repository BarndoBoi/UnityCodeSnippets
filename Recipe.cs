public class Recipe
{
    public Dictionary<Commodity, int> InputGoods { get; } = new Dictionary<Commodity, int>();
    public Dictionary<Commodity, int> OutputGoods { get; } = new Dictionary<Commodity, int>();
}