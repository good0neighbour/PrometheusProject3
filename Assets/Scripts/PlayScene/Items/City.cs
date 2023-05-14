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
        // ��ġ
        CityNum = cityNum;
        LandNum = landNum;
        CityName = cityName;
        Capacity = capacity;
        Population = Constants.INITIAL_POPULATION;

        // �迭 true�� �ʱ�ȭ
        for (byte i = 0; i < _facilityEnabled.Length; ++i)
        {
            _facilityEnabled[i] = true;
        }

        // �ü� ��� ���� ����
        TechTrees.Node[] data = PlayManager.Instance.GetTechTreeData().GetFacilityNodes();
        for (byte i = 0; i < data.Length; ++i)
        {
            if (0 < data[i].PreviousNodes.Length)
            {
                // �ش� ��忡 �ش��ϴ� ��Ҹ� false�� ��ȯ
                _facilityEnabled[i] = false;
            }
        }
    }


    /// <summary>
    /// �ü� ���� ���� �����´�.
    /// </summary>
    public bool[] GetFacilityAdopted()
    {
        return _facilityAdopted;
    }


    /// <summary>
    /// �ü� ��� ���� ���� �����´�.
    /// </summary>
    public bool[] GetFacilityEnabled()
    {
        return _facilityEnabled;
    }



    /* ==================== Private Methods ==================== */
}
