using System.Collections.Generic;

public class City
{
    /* ==================== Variables ==================== */

    private bool[] _facilityAdopted = null;
    private bool[] _facilityEnabled = null;

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

        // 지역변수로 참조
        TechTrees.Node[] data = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Facility);
        Dictionary<string, byte> nodeIndex = PlayManager.Instance.GetTechTreeData().GetIndexDictionary();
        bool[][] adoptedData = PlayManager.Instance.GetAdoptedData();

        // 배열 생성
        byte length = (byte)adoptedData[(int)TechTreeType.Facility].Length;
        _facilityAdopted = new bool[length];
        _facilityEnabled = new bool[length];

        // 시설 사용 가능 여부
        for (byte i = 0; i < data.Length; ++i)
        {
            // 일단 true
            _facilityEnabled[i] = true;

            // 사용 가능 여부 확인
            for (byte j = 0; j < data[i].Requirments.Length; ++j)
            {
                TechTrees.Node.RequirmentNode node = data[i].Requirments[j];
                switch (node.Type)
                {
                    case TechTreeType.Facility:
                        // 시설을 필요로 하면 반드시 false
                        _facilityEnabled[i] = false;
                        break;
                    default:
                        // 한 개라도 활성화가 안 됐으면 false
                        if (!adoptedData[(int)node.Type][nodeIndex[node.NodeName]])
                        {
                            _facilityEnabled[i] = false;
                        }
                        break;
                }

                // false면 바로 탈출
                if (!_facilityEnabled[i])
                {
                    break;
                }
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
