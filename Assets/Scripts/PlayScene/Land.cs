public class Land
{
    public ushort LandNum { get; set; }
    public string CityName { get; set; }
    public Land(ushort landNum)
    {
        LandNum = landNum;
        CityName = null;
    }
}
