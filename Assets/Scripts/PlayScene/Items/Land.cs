using System;

[Serializable]
public class Land
{
    public ushort LandNum = 0;
    public string CityName = null;
    public byte[] Resources = null;

    public Land(ushort landNum, byte[] resources)
    {
        LandNum = landNum;
        Resources = resources;
    }
}
