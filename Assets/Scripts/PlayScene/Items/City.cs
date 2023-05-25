using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class City
{
    /* ==================== Variables ==================== */

    private bool[] _facilityAdopted = null;
    private bool[] _facilityEnabled = null;

    public string CityName { get; private set; }
    public byte NumOfFacility { get; set; }
    public ushort CityNum { get; private set; }
    public ushort Capacity { get; private set; }
    public short AnnualFund { get; set; }
    public ushort AnnualResearch { get; set; }
    public float Stability { get; set; }
    public float Population { get; private set; }
    public float PopulationMovement { get; private set; }
    public float PopulationMovementMultiply { get; set; }
    public float Crime { get; private set; }
    public float Disease { get; private set; }
    public float Injure { get; private set; }
    public float CrimePosibility { get; private set; }
    public float DiseasePosibility { get; private set; }
    public float InjurePosibility { get; set; }
    public float Police { get; set; }
    public float Health { get; set; }
    public float Safety { get; set; }



    /* ==================== Public Methods ==================== */

    public City(ushort cityNum, string cityName, ushort capacity)
    {
        // ��ġ
        CityNum = cityNum;
        CityName = cityName;
        Capacity = capacity;
        Population = Constants.INITIAL_POPULATION;
        PopulationMovementMultiply = Constants.INITIAL_POPULATION_MOVEMENT;

        // ���������� ����
        TechTrees.Node[] data = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Facility);
        Dictionary<string, byte> nodeIndex = PlayManager.Instance.GetTechTreeData().GetIndexDictionary();
        float[][] adoptedData = PlayManager.Instance.GetAdoptedData();

        // �迭 ����
        byte length = PlayManager.Instance.FacilityLength;
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
                TechTrees.SubNode node = data[i].Requirments[j];
                switch (node.Type)
                {
                    case TechTreeType.Facility:
                        // �ü��� �ʿ�� �ϸ� �ݵ�� false
                        _facilityEnabled[i] = false;
                        break;
                    default:
                        // �� ���� Ȱ��ȭ�� �� ������ false
                        if (1.0f > adoptedData[(int)node.Type][nodeIndex[node.NodeName]])
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

        // ���� Ȱ��ȭ
        BeginCityRunning();
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


    /// <summary>
    /// ���� Ȱ��ȭ
    /// </summary>
    public void BeginCityRunning()
    {
        PlayManager.OnMonthChange += OnMonthChange;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ġ�Ȱ� ����
    /// </summary>
    private void SafetyAndHealth()
    {
        // ���� ���� ����
        float float0;

        // ���� �� ���� �߻�
        if (Constants.MIN_EVEN_POPULATION < Population)
        {
            float0 = Random.Range(0.0f, 100.0f);
            if (4.0f > float0 && 24.0f > CrimePosibility)
            {
                CrimePosibility += Random.Range(1.0f, 5.0f);
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{����}���� ���� ������ �߻��߽��ϴ�."
                ], CityName);
            }
            else if (8.0f > float0 && 60.0f > DiseasePosibility)
            {
                DiseasePosibility += Random.Range(1.0f, 10.0f);
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{����}���� ������ â���߽��ϴ�."
                ], CityName);
            }
        }

        // ������
        float0 = CrimePosibility - Police;
        if (0.0f > float0)
        {
            float0 = 0.0f;
        }
        Crime += (float0 - Crime) * Constants.CRIME_RATE_SPEEDMULT;

        // ����
        float0 = DiseasePosibility - Health;
        if (0.0f > float0)
        {
            float0 = 0.0f;
        }
        Disease += (float0 - Disease) * Constants.DEATH_RATE_SPEEDMULT;

        // �λ�
        float0 = InjurePosibility - Safety;
        if (0.0f > float0)
        {
            float0 = 0.0f;
        }
        Injure += (float0 - Injure) * Constants.DEATH_RATE_SPEEDMULT;
    }


    /// <summary>
    /// �α� ��ȭ
    /// </summary>
    private void PopulationMove()
    {
        PopulationMovement = (Capacity - Population) * PopulationMovementMultiply - (Disease + Injure) * Population * 0.01f;
        Population += PopulationMovement * Constants.YEAR_TO_MONTH;
    }


    private void OnMonthChange()
    {
        SafetyAndHealth();
        PopulationMove();
    }
}
