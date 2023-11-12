using System.Text.Json.Serialization;

public class Event
{
    public ushort EventID { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    [JsonIgnore]
    public Action<int> OnEventPicked { get; set; }
    public int ChanceInOneHundred { get; set; }
    public int GlobalTrend { get; set; }

    public Event(ushort eventID, string name, string text, int chanceInOneHundred, int globalTrend)
    {
        EventID = eventID;
        Name = name;
        Text = text;
        ChanceInOneHundred = chanceInOneHundred;
        GlobalTrend = globalTrend;

        // Initialize OnEventPicked to an empty delegate to avoid null reference warnings
        OnEventPicked = (trend) => { };
    }

    public void TriggerEvent(int globalTrend)
    {
        OnEventPicked?.Invoke(globalTrend);
    }
}