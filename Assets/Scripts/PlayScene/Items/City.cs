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
        // ��ġ
        CityNum = cityNum;
        LandNum = landNum;
        CityName = cityName;
        Capacity = capacity;
        Population = Constants.INITIAL_POPULATION;

        // ���������� ����
        TechTrees.Node[] data = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Facility);
        Dictionary<string, byte> nodeIndex = PlayManager.Instance.GetTechTreeData().GetIndexDictionary();
        bool[][] adoptedData = PlayManager.Instance.GetAdoptedData();

        // �迭 ����
        byte length = (byte)adoptedData[(int)TechTreeType.Facility].Length;
        _facilityAdopted = new bool[length];
        _facilityEnabled = new bool[length];

        // �ü� ��� ���� ����
        for (byte i = 0; i < data.Length; ++i)
        {
            // �ϴ� true
            _facilityEnabled[i] = true;

            // ��� ���� ���� Ȯ��
            for (byte j = 0; j < data[i].Requirments.Length; ++j)
            {
                TechTrees.Node.RequirmentNode node = data[i].Requirments[j];
                switch (node.Type)
                {
                    case TechTreeType.Facility:
                        // �ü��� �ʿ�� �ϸ� �ݵ�� false
                        _facilityEnabled[i] = false;
                        break;
                    default:
                        // �� ���� Ȱ��ȭ�� �� ������ false
                        if (!adoptedData[(int)node.Type][nodeIndex[node.NodeName]])
                        {
                            _facilityEnabled[i] = false;
                        }
                        break;
                }

                // false�� �ٷ� Ż��
                if (!_facilityEnabled[i])
                {
                    break;
                }
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
