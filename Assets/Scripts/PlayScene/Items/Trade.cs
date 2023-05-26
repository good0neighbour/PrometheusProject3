public class Trade
{
    public Force CurrentForce { get; set; }
    public ushort TimeRemain { get; set; }
    public short Iron { get; set; }
    public short Nuke { get; set; }
    public short Jewel { get; set; }
    public int AnnualIncome { get; set; }

    public Trade(Force force, ushort timeRemain, short iron, short nuke, short jewel, int annualIncome)
    {
        CurrentForce = force;
        TimeRemain = timeRemain;
        Iron = iron;
        Nuke = nuke;
        Jewel = jewel;
        AnnualIncome = annualIncome;
    }
}
