using System;

[Serializable]
public class Trade
{
    public byte ForceNum = 0;
    public ushort TimeRemain = 0;
    public short Iron = 0;
    public short Nuke = 0;
    public short Jewel = 0;
    public int AnnualIncome = 0;

    public Trade(byte forceNum, ushort timeRemain, short iron, short nuke, short jewel, int annualIncome)
    {
        ForceNum = forceNum;
        TimeRemain = timeRemain;
        Iron = iron;
        Nuke = nuke;
        Jewel = jewel;
        AnnualIncome = annualIncome;
    }
}
