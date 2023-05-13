public class Land
{
    public ushort LandNum { get; private set; }
    public string CityName { get; set; }
    public byte[] Resources { get; private set; }

    public Land(ushort landNum, byte[] resources)
    {
        LandNum = landNum;
        Resources = resources;
        CityName = null;
    }
}
