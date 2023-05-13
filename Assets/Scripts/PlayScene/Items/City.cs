public class City
{
    /* ==================== Variables ==================== */

    public ushort CityNum { get; private set; }
    public ushort LandNum { get; private set; }
    public string CityName { get; private set; }
    public ushort Capacity { get; private set; }
    public ushort Population { get; set; }



    /* ==================== Public Methods ==================== */

    public City(ushort cityNum, ushort landNum, string cityName, ushort capacity)
    {
        CityNum = cityNum;
        LandNum = landNum;
        CityName = cityName;
        Capacity = capacity;
        Population = Constants.INITIAL_POPULATION;
    }



    /* ==================== Private Methods ==================== */
}
