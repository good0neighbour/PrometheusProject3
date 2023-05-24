public class Force
{
    public string ForceName { get; set; }
    public byte TradeTime { get; set; }
    public ushort JewelTrade { get; set; }
    public ushort Culture { get; set; }
    public float Friendly { get; set; }
    public float Hostile { get; set; }
    public float Conquest { get; set; }
    public float Resist { get; set; }
    public bool Info { get; set; }

    public Force(string nationName)
    {
        ForceName = nationName;
    }
}
