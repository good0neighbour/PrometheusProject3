public struct LandResources
{
    public byte Iron;
    public byte Nuke;

    public LandResources(byte iron, byte nuke)
    {
        Iron = iron;
        Nuke = nuke;
    }
}


public class Land
{
    public ushort LandNum { get; set; }
    public string CityName { get; set; }
    public LandResources Resources { get; set; }

    public Land(ushort landNum, LandResources resources)
    {
        LandNum = landNum;
        Resources = resources;
        CityName = null;
    }
}
