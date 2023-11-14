public class SectorTrend
{
    public string SectorName { get; set; }
    public float ChangeMin { get; set; }
    public float ChangeMax { get; set; }

    private float currentTrend;

    public float CurrentTrend
    {
        get { return currentTrend; }
        set { currentTrend = value; }
    }
}