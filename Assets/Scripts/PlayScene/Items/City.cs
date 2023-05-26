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
        // 수치
        CityName = cityName;
        Capacity = capacity;

        // 지역변수로 참조
        TechTrees.Node[] data = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Facility);
        Dictionary<string, byte> nodeIndex = PlayManager.Instance.GetTechTreeData().GetIndexDictionary();
        float[][] adoptedData = PlayManager.Instance.GetAdoptedData();

        // 배열 생성
        byte length = PlayManager.Instance.FacilityLength;
        FacilityAdopted = new bool[length];
        FacilityEnabled = new bool[length];

        // 시설 사용 가능 여부
        for (byte i = 0; i < data.Length; ++i)
        {
            // 일단 true
            FacilityEnabled[i] = true;

            // 사용 가능 여부 확인
            for (byte j = 0; j < data[i].Requirments.Length; ++j)
            {
                TechTrees.SubNode node = data[i].Requirments[j];
                switch (node.Type)
                {
                    case TechTreeType.Facility:
                        // 시설을 필요로 하면 반드시 false
                        FacilityEnabled[i] = false;
                        break;
                    default:
                        // 한 개라도 활성화가 안 됐으면 false
                        if (1.0f > adoptedData[(int)node.Type][nodeIndex[node.NodeName]])
                        {
                            FacilityEnabled[i] = false;
                        }
                        break;
                }

                // false면 바로 탈출
                if (!FacilityEnabled[i])
                {
                    break;
                }
            }
        }

        // 도시 활성화
        BeginCityRunning();
    }


    /// <summary>
    /// 도시 활성화
    /// </summary>
    public void BeginCityRunning()
    {
        PlayManager.OnMonthChange += OnMonthChange;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 치안과 보건
    /// </summary>
    private void SafetyAndHealth()
    {
        // 계산용 지역 변수
        float float0;

        // 범죄 빛 역병 발생
        if (Constants.MIN_EVEN_POPULATION < Population)
        {
            float0 = Random.Range(0.0f, 100.0f);
            if (4.0f > float0 && 24.0f > CrimePosibility)
            {
                CrimePosibility += Random.Range(1.0f, 5.0f);
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{도시}에서 범죄 조직이 발생했습니다."
                ], CityName);
            }
            else if (8.0f > float0 && 60.0f > DiseasePosibility)
            {
                DiseasePosibility += Random.Range(1.0f, 10.0f);
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{도시}에서 역병이 창궐했습니다."
                ], CityName);
            }
        }

        // 범죄율
        float0 = CrimePosibility - Police;
        if (0.0f > float0)
        {
            float0 = 0.0f;
        }
        Crime += (float0 - Crime) * Constants.CRIME_RATE_SPEEDMULT;

        // 질병
        float0 = DiseasePosibility - Health;
        if (0.0f > float0)
        {
            float0 = 0.0f;
        }
        Disease += (float0 - Disease) * Constants.DEATH_RATE_SPEEDMULT;

        // 부상
        float0 = InjurePosibility - Safety;
        if (0.0f > float0)
        {
            float0 = 0.0f;
        }
        Injure += (float0 - Injure) * Constants.DEATH_RATE_SPEEDMULT;
    }


    /// <summary>
    /// 인구 변화
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
