public class City
{
    /* ==================== Variables ==================== */

    private bool[] _facilityAdopted = new bool[(int)FaciityTag.FacilityEnd];
    private bool[] _facilityEnabled = new bool[(int)FaciityTag.FacilityEnd];

    public string CityName { get; private set; }
    public ushort CityNum { get; private set; }
    public ushort LandNum { get; private set; }
    public ushort Capacity { get; private set; }
    public float Population { get; set; }



    /* ==================== Public Methods ==================== */

    public City(ushort cityNum, ushort landNum, string cityName, ushort capacity)
    {
        // 수치
        CityNum = cityNum;
        LandNum = landNum;
        CityName = cityName;
        Capacity = capacity;
        Population = Constants.INITIAL_POPULATION;

        // 시설 사용 가능 여부
        TechTrees.Node[] data = PlayManager.Instance.GetTechTreeData().GetFacilityNodes();
        for (byte i = 0; i < data.Length; ++i)
        {
            if (0 == data[i].PreviousNodes.Length)
            {
                // 이전 노드가 없으면 사용 가능
                _facilityEnabled[i] = true;
            }
        }
    }


    /// <summary>
    /// 시설 승인 여부 가져온다.
    /// </summary>
    public bool[] GetFacilityAdopted()
    {
        return _facilityAdopted;
    }


    /// <summary>
    /// 시설 사용 가능 여부 가져온다.
    /// </summary>
    public bool[] GetFacilityEnabled()
    {
        return _facilityEnabled;
    }



    /* ==================== Private Methods ==================== */
}
