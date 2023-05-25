public class Trade
{
    public string ForceName { get; set; }
    public ushort TimeRemain { get; set; }
    public short Iron { get; set; }
    public short Nuke { get; set; }
    public short Jewel { get; set; }
    public int AnnualIncome { get; set; }

    public Trade(string forceName, ushort timeRemain, short iron, short nuke, short jewel, int annualIncome)
    {
        ForceName = forceName;
        TimeRemain = timeRemain;
        Iron = iron;
        Nuke = nuke;
        Jewel = jewel;
        AnnualIncome = annualIncome;
    }
}
