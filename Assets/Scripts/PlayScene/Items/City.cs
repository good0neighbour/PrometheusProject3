using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class City
{
    /* ==================== Variables ==================== */

    public bool[] FacilityAdopted = null;
    public bool[] FacilityEnabled = null;
    public string CityName = null;
    public byte NumOfFacility = 0;
    public ushort Capacity = 0;
    public short AnnualFund = 0;
    public ushort AnnualResearch = 0;
    public float Stability = 0.0f;
    public float Population = Constants.INITIAL_POPULATION;    
    public float PopulationMovement = 0.0f;
    public float PopulationMovementMultiply = Constants.INITIAL_POPULATION_MOVEMENT;
    public float Crime = 0.0f;
    public float Disease = 0.0f;
    public float Injure = 0.0f;
    public float CrimePosibility = 0.0f;
    public float DiseasePosibility = 0.0f;
    public float InjurePosibility = 0.0f;
    public float Police = 0.0f;
    public float Health = 0.0f;
    public float Safety = 0.0f;



    /* ==================== Public Methods ==================== */

    public City(string cityName, ushort capacity)
    {
        // ��ġ
        CityName = cityName;
        Capacity = capacity;

        // ���������� ����
        TechTrees.Node[] data = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Facility);
        Dictionary<string, byte> nodeIndex = PlayManager.Instance.GetTechTreeData().GetIndexDictionary();
        float[][] adoptedData = PlayManager.Instance.GetAdoptedData();

        // �迭 ����
        byte length = PlayManager.Instance.FacilityLength;
        FacilityAdopted = new bool[length];
        FacilityEnabled = new bool[length];

        // �ü� ��� ���� ����
        for (byte i = 0; i < data.Length; ++i)
        {
            // �ϴ� true
            FacilityEnabled[i] = true;

            // ��� ���� ���� Ȯ��
            for (byte j = 0; j < data[i].Requirments.Length; ++j)
            {
                TechTrees.SubNode node = data[i].Requirments[j];
                switch (node.Type)
                {
                    case TechTreeType.Facility:
                        // �ü��� �ʿ�� �ϸ� �ݵ�� false
                        FacilityEnabled[i] = false;
                        break;
                    default:
                        // �� ���� Ȱ��ȭ�� �� ������ false
                        if (1.0f > adoptedData[(int)node.Type][nodeIndex[node.NodeName]])
                        {
                            FacilityEnabled[i] = false;
                        }
                        break;
                }

                // false�� �ٷ� Ż��
                if (!FacilityEnabled[i])
                {
                    break;
                }
            }
        }

        // ���� Ȱ��ȭ
        BeginCityRunning();
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
        Population += PopulationMovement;
    }


    private void OnMonthChange()
    {
        SafetyAndHealth();
        PopulationMove();
    }
}
