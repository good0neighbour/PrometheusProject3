using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    public static OnChangeDelegate OnPlayUpdate = null;
    public static OnChangeDelegate OnMonthChange = null;
    public static OnChangeDelegate OnYearChange = null;

    [Header("참조")]
    [SerializeField] private GameObject _landSlot = null;
    [SerializeField] private GameObject _citySlot = null;
    [SerializeField] private GameObject _tradeSlot = null;
    [SerializeField] private Transform _landListContentArea = null;
    [SerializeField] private Transform _cityListContentArea = null;
    [SerializeField] private Transform _tradeListContentArea = null;
    [SerializeField] private TechTrees _techTreeData = null;
    [SerializeField] private ScreenResearch _researchScreen = null;
    [SerializeField] private ScreenSociety _societyScreen = null;
    [SerializeField] private ScreenPhoto _photoScreen = null;
    [SerializeField] private ScreenBreath _breathScreen = null;

    private float[][] _adoptedData = new float[(int)TechTreeType.TechTreeEnd][];
    private JsonData _data;
    private float _timer = 0.0f;
    private float _gameSpeed = 1.0f;
    private float _etcAirMassGoal = 0.0f;
    private float _temperatureMovement = 0.0f;
    private float _totalWaterVolumeGoal = 0.0f;
    private float _incomeEnergy_C = 0.0f;
    private float _facilitySupportGoal = 0.0f;
    private float _researchSupportGoal = 0.0f;
    private float _societySupportGoal = 0.0f;
    private float _diplomacySupportGoal = 0.0f;
    private float _totalCarboneRatioGoal = 0.0f;
    private float _carbonRatio = 1.0f / Constants.EARTH_CARBON_RATIO_ppm;
    private double _cloudReflectionMultiply = 0.0d;
    private bool _autoSave = false;
    private bool _isGameEnded = false;

    public static PlayManager Instance
    {
        get;
        private set;
    }

    public bool IsPlaying
    {
        get;
        set;
    }

    public float GameResume
    {
        get;
        set;
    }

    public float GameSpeed
    {
        get
        {
            return _gameSpeed * GameResume;
        }
        set
        {
            _gameSpeed = value;
        }
    }

    public byte FacilityLength
    {
        get;
        private set;
    }

    public float FacilitySupportGoal
    {
        get
        {
            return _facilitySupportGoal;
        }
        set
        {
            _facilitySupportGoal = value;
            if (100.0f < _facilitySupportGoal)
            {
                _facilitySupportGoal = 100.0f;
            }
            else if (0.0f > _facilitySupportGoal)
            {
                _facilitySupportGoal = 0.0f;
            }
        }
    }

    public float ResearchSupportGoal
    {
        get
        {
            return _researchSupportGoal;
        }
        set
        {
            _researchSupportGoal = value;
            if (100.0f < _researchSupportGoal)
            {
                _researchSupportGoal = 100.0f;
            }
            else if (0.0f > _researchSupportGoal)
            {
                _researchSupportGoal = 0.0f;
            }
        }
    }

    public float SocietySupportGoal
    {
        get
        {
            return _societySupportGoal;
        }
        set
        {
            _societySupportGoal = value;
            if (100.0f < _societySupportGoal)
            {
                _societySupportGoal = 100.0f;
            }
            else if (0.0f > _societySupportGoal)
            {
                _societySupportGoal = 0.0f;
            }
        }
    }

    public float DiplomacySupportGoal
    {
        get
        {
            return _diplomacySupportGoal;
        }
        set
        {
            _diplomacySupportGoal = value;
            if (100.0f < _diplomacySupportGoal)
            {
                _diplomacySupportGoal = 100.0f;
            }
            else if (0.0f > _diplomacySupportGoal)
            {
                _diplomacySupportGoal = 0.0f;
            }
        }
    }

    #region JsonData의 배열에 접근
    /// <summary>
    /// 편리한 접근을 위해 만들었다. byte.
    /// </summary>
    public byte this[VariableByte variable]
    {
        get
        {
            return _data.ByteArray[(int)variable];
        }
        set
        {
            _data.ByteArray[(int)variable] = value;
        }
    }

    /// <summary>
    /// 편리한 접근을 위해 만들었다. short.
    /// </summary>
    public short this[VariableShort variable]
    {
        get
        {
            return _data.ShortArray[(int)variable];
        }
        set
        {
            _data.ShortArray[(int)variable] = value;
        }
    }

    /// <summary>
    /// 편리한 접근을 위해 만들었다. ushort.
    /// </summary>
    public ushort this[VariableUshort variable]
    {
        get
        {
            return _data.UshortArray[(int)variable];
        }
        set
        {
            _data.UshortArray[(int)variable] = value;
        }
    }

    /// <summary>
    /// 편리한 접근을 위해 만들었다. int.
    /// </summary>
    public int this[VariableInt variable]
    {
        get
        {
            return _data.IntArray[(int)variable];
        }
        set
        {
            _data.IntArray[(int)variable] = value;
        }
    }

    /// <summary>
    /// 편리한 접근을 위해 만들었다. uint.
    /// </summary>
    public uint this[VariableUint variable]
    {
        get
        {
            return _data.UintArray[(int)variable];
        }
        set
        {
            _data.UintArray[(int)variable] = value;
        }
    }

    /// <summary>
    /// 편리한 접근을 위해 만들었다. long.
    /// </summary>
    public long this[VariableLong variable]
    {
        get
        {
            return _data.LongArray[(int)variable];
        }
        set
        {
            _data.LongArray[(int)variable] = value;
        }
    }

    /// <summary>
    /// 편리한 접근을 위해 만들었다. float.
    /// </summary>
    public float this[VariableFloat variable]
    {
        get
        {
            return _data.FloatArray[(int)variable];
        }
        set
        {
            _data.FloatArray[(int)variable] = value;
        }
    }

    /// <summary>
    /// 편리한 접근을 위해 만들었다. double.
    /// </summary>
    public double this[VariableDouble variable]
    {
        get
        {
            return _data.DoubleArray[(int)variable];
        }
        set
        {
            _data.DoubleArray[(int)variable] = value;
        }
    }

    /// <summary>
    /// 편리한 접근을 위해 만들었다. bool.
    /// </summary>
    public bool this[VariableBool variable]
    {
        get
        {
            return _data.BoolArray[(int)variable];
        }
        set
        {
            _data.BoolArray[(int)variable] = value;
        }
    }
    #endregion



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 토지 정보 가져온다.
    /// </summary>
    public Land GetLand(ushort index)
    {
        return _data.Lands[index];
    }


    /// <summary>
    /// 도시 정보를 가져온다.
    /// </summary>
    public City GetCity(ushort index)
    {
        return _data.Cities[index];
    }


    /// <summary>
    /// 세력 정보를 가져온다.
    /// </summary>
    public Force GetForce(ushort index)
    {
        return _data.Forces[index];
    }


    /// <summary>
    /// 무역 정보를 가져온다.
    /// </summary>
    public Trade GetTrade(ushort index)
    {
        return _data.Trades[index];
    }


    /// <summary>
    /// 무역 정보를 제거한다.
    /// </summary>
    public void RemoveTrade(Trade trade)
    {
        _data.Trades.Remove(trade);
    }


    /// <summary>
    /// 토지 슬롯 추가
    /// </summary>
    public void AddLandSlot(ushort landNum)
    {
        // 토지 버튼 추가 및 초기화
        Instantiate(_landSlot, _landListContentArea).GetComponent<SlotLand>().SlotInitialize(landNum);
    }


    /// <summary>
    /// 도시 추가
    /// </summary>
    public void AddCity(string cityName, ushort capacity)
    {
        // 가변배열에 도시 추가
        _data.Cities.Add(new City(cityName, capacity));
    }


    /// <summary>
    /// 도시 슬롯 추가
    /// </summary>
    public void AddCitySlot(ushort cityNum)
    {
        // 도시 버튼 추가 및 초기화
        Instantiate(_citySlot, _cityListContentArea).GetComponent<SlotCity>().SlotInitialize(cityNum);
    }


    /// <summary>
    /// 거래 추가
    /// </summary>
    public void AddTrade(Trade trade)
    {
        // 가변배열에 거래 추가
        _data.Trades.Add(trade);
    }


    /// <summary>
    /// 거래 슬롯 추가
    /// </summary>
    public void AddTradeSlot(ushort tradeNum, bool firstAdd)
    {
        // 거래 버튼 추가 및 초기화
        SlotTrade slot = Instantiate(_tradeSlot, _tradeListContentArea).GetComponent<SlotTrade>();
        slot.SlotInitialize(tradeNum);

        // 저장된 데이타 불러오는 경우가 아닐 때
        if (firstAdd)
        {
            slot.RunTrade(1);
        }
    }


    /// <summary>
    /// 테크 트리 정보 가져온다.
    /// </summary>
    public TechTrees GetTechTreeData()
    {
        return _techTreeData;
    }


    /// <summary>
    /// 활성화 여부 정보 가져온다.
    /// </summary>
    public float[][] GetAdoptedData()
    {
        return _adoptedData;
    }


    /// <summary>
    /// 사회 하위 요소 진행도 반환
    /// </summary>
    public float[] GetSocietyElementProgression()
    {
        return _data.SocietyElementProgression;
    }


    /// <summary>
    /// 배열 생성 후 반환
    /// </summary>
    public float[] GetSocietyElementProgression(byte length)
    {
        _data.SocietyElementProgression = new float[length];
        return _data.SocietyElementProgression;
    }


    /// <summary>
    /// 게임 저장
    /// </summary>
    public void SaveGame()
    {
        File.WriteAllText($"{Application.dataPath}/Resources/Saves.Json", JsonUtility.ToJson(_data, false));
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 탐사 진행
    /// </summary>
    private void ExploreProgress()
    {
        // 토지 수가 ushort의 최대치를 넘지 않을 때만
        if (ushort.MaxValue > this[VariableUshort.LandNum])
        {
            // 탐사 완료 시
            if (this[VariableFloat.ExploreProgress] >= this[VariableFloat.ExploreGoal])
            {
                // 가변배열에 토지 추가
                _data.Lands.Add(new Land(this[VariableUshort.LandNum], RandomResources()));

                // 토지 슬롯 추가
                AddLandSlot(this[VariableUshort.LandNum]);

                // 토지 개수 추가
                ++this[VariableUshort.LandNum];

                // 메세지
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "새로운 토지를 발견했습니다."
                    ]);

                // 탐사 진행 초기화
                this[VariableFloat.ExploreProgress] = 0.0f;

                // 탐사 목표 증가
                this[VariableFloat.ExploreGoal] *= Constants.EXPLORE_GOAL_INCREASEMENT;
            }
            // 진행 중일 때
            else
            {
                // 시간에 따른 진행도 상승
                this[VariableFloat.ExploreProgress] += this[VariableByte.ExploreDevice] * Time.deltaTime * Constants.EXPLORE_SPEEDMULT * GameSpeed;
            }
        }
    }


    /// <summary>
    /// 무작위 자원 생성
    /// </summary>
    private byte[] RandomResources()
    {
        // 자원 변수
        int[] resources = new int[(int)ResourceType.End];
        int total;

        // 어느 하나라도 0이 아닐 때까지 반복
        do
        {
            // 총 값 초기화
            total = 0;

            for (byte i = 0; i < (byte)ResourceType.End; ++i)
            {
                // 무작위 값
                resources[i] = Random.Range(Constants.RESOURCE_MIN_MAX[i, 0], Constants.RESOURCE_MIN_MAX[i, 1] + 1);

                // 0은 제외
                if (0 > resources[i])
                {
                    resources[i] = 0;
                }

                // 총합
                total += resources[i];
            }
        } while (0 == total);

        // 배열 생성
        byte[] result = new byte[(int)ResourceType.End];
        for (byte i = 0; i < (byte)ResourceType.End; ++i)
        {
            result[i] = (byte)resources[i];
        }

        //반환
        return result;
    }


    /// <summary>
    /// 환경 조정은 천천히 적용.
    /// </summary>
    private void EnvironmentMovementAdopt()
    {
        this[VariableFloat.EtcAirMass_Tt] += (_etcAirMassGoal - this[VariableFloat.EtcAirMass_Tt]) * Time.deltaTime * GameSpeed;
        this[VariableFloat.TotalWater_PL] += (_totalWaterVolumeGoal - this[VariableFloat.TotalWater_PL]) * Time.deltaTime * GameSpeed;
        _temperatureMovement += (this[VariableShort.TemperatureMovement] - _temperatureMovement) * Time.deltaTime * GameSpeed;
        this[VariableFloat.TotalCarbonRatio_ppm] += (_totalCarboneRatioGoal - this[VariableFloat.TotalCarbonRatio_ppm]) * Time.deltaTime * GameSpeed;
    }


    /// <summary>
    /// 물리량 계산
    /// </summary>
    private void PhysicsCalculate()
    {
        // 계산용 지역변수
        double double0;
        double double1;
        float float0;

        // 천문학적인 계산은 기본적으로 큰 자료형으로 변환 후 계산한다.
        #region 대기압
        // 기체질량
        this[VariableFloat.TotalAirMass_Tt] = this[VariableFloat.WaterGas_PL] + this[VariableFloat.CarbonGasMass_Tt] + this[VariableFloat.EtcAirMass_Tt];

        // 대기압
        this[VariableFloat.TotalAirPressure_hPa] = (float)(Constants.E7 * this[VariableFloat.TotalAirMass_Tt] * this[VariableFloat.GravityAccelation_m_s2] / this[VariableFloat.PlanetArea_km2]);
        #endregion 대기압

        #region 기온
        // 수증기 온실
        this[VariableFloat.WaterGreenHouse_C] = (float)(24.06923987d * Math.Log10(this[VariableFloat.WaterGas_PL] + 1.0d));

        // 탄소 온실
        this[VariableFloat.CarbonGreenHouse_C] = (float)(20.97411189d * Math.Log10(this[VariableFloat.CarbonGasMass_Tt] + 1.0d));

        // 기타 온실
        this[VariableFloat.EtcGreenHouse_C] = (float)((1.53162266d) * Math.Log10(this[VariableFloat.EtcAirMass_Tt] + 1.0d));

        // 구름 반사율 계산
        double0 = this[VariableFloat.WaterGas_PL] * _cloudReflectionMultiply;
        // 행성이 두꺼운 대기로 덮여있을 때
        if (double0 > 1.0d)
        {
            this[VariableFloat.CloudReflection] = 0.35f;
            this[VariableFloat.IceReflection] = 0.0f;
            this[VariableFloat.WaterReflection] = 0.0f;
            this[VariableFloat.GroundReflection] = 0.0f;
        }
        // 복사에너지가 지표면까지 도달
        else
        {
            // 구름 반사율
            this[VariableFloat.CloudReflection] = (float)(double0 * 0.35d);

            // 빙하 반사율 계산
            double0 = 1.0d - double0;
            double1 = this[VariableFloat.WaterSolid_PL] * 1.3409961685823754789272030651341e-5d;
            // 행성 표면이 전부 얼음으로 덮여있을 때
            if (double1 > 1.0d)
            {
                this[VariableFloat.IceReflection] = (float)(double0 * 0.9d);
                this[VariableFloat.WaterReflection] = 0.0f;
                this[VariableFloat.GroundReflection] = 0.0f;
            }
            // 행성 표면이 전부 얼음으로 덮여있지 않을 때
            else
            {
                // 빙하 반사율
                double1 *= double0;
                this[VariableFloat.IceReflection] = (float)(double1 * 0.9d);

                // 물 반사율 계산
                double0 -= double1;
                double1 = double0 * this[VariableFloat.WaterLiquid_PL] * 2.9397176744512440041043142009695e-8d;
                // 행성 표면이 물로 덮여있을 때
                if (double1 > 1.0d)
                {
                    this[VariableFloat.WaterReflection] = (float)(double0 * 0.035d);
                    this[VariableFloat.GroundReflection] = 0.0f;
                }
                // 지각이 드러날 때
                else
                {
                    // 물 반사율
                    double1 *= double0;
                    this[VariableFloat.WaterReflection] = (float)(double1 * 0.035d);

                    // 지각 반사율
                    double0 -= double1;
                    this[VariableFloat.GroundReflection] = (float)(double0 * 0.1d);
                }
            }
        }

        // 총 반사율
        this[VariableFloat.TotalReflection] = this[VariableFloat.GroundReflection] + this[VariableFloat.WaterReflection] + this[VariableFloat.IceReflection] + this[VariableFloat.CloudReflection];

        // 흡수에너지
        this[VariableFloat.AbsorbEnergy] = this[VariableFloat.IncomeEnergy] * (1.0f - this[VariableFloat.TotalReflection]);

        // 기온
        this[VariableFloat.TotalTemperature_C] = Constants.MIN_KELVIN + _incomeEnergy_C + this[VariableFloat.AbsorbEnergy] * 15.797788309636650868878357030016f + this[VariableFloat.WaterGreenHouse_C] + this[VariableFloat.CarbonGreenHouse_C] + this[VariableFloat.EtcGreenHouse_C] + _temperatureMovement;
        if (this[VariableFloat.TotalTemperature_C] < Constants.MIN_KELVIN)
        {
            this[VariableFloat.TotalTemperature_C] = Constants.MIN_KELVIN;
        }
        #endregion 기온

        #region 물의 체적
        // 전체

        // 기체
        double0 = 8.3269949383584792498079949276151e-7d * Math.Pow(1.0630781589386992356184590458984d, this[VariableFloat.TotalTemperature_C] + 23.94160508d);
        if (double0 > 1.0d)
        {
            double0 = 1.0d;
        }
        this[VariableFloat.WaterGas_PL] = (float)(this[VariableFloat.TotalWater_PL] * double0);

        // 고체
        if (this[VariableFloat.TotalTemperature_C] > Constants.MAX_ICE_TEMP)
        {
            this[VariableFloat.WaterSolid_PL] = 0.0f;
        }
        else
        {
            double1 = 0.02645536938010781533821225333651d * Math.Log10(Constants.MAX_ICE_TEMP_LOG - this[VariableFloat.TotalTemperature_C]);
            if (double1 < 1.0d)
            {
                this[VariableFloat.WaterSolid_PL] = (float)(this[VariableFloat.TotalWater_PL] * (1.0d - double0) * double1);
            }
            else
            {
                this[VariableFloat.WaterSolid_PL] = (float)(this[VariableFloat.TotalWater_PL] * (1.0d - double0));
            }
        }

        // 액체
        this[VariableFloat.WaterLiquid_PL] = this[VariableFloat.TotalWater_PL] - (this[VariableFloat.WaterGas_PL] - this[VariableFloat.WaterSolid_PL]);
        #endregion 물의 체적

        #region 탄소 순환
        // 기권
        this[VariableFloat.CarbonGasMass_Tt] = this[VariableFloat.TotalAirPressure_hPa] * 7.1058475203552923760177646188009e-4f * this[VariableFloat.TotalCarbonRatio_ppm] * _carbonRatio;

        // 수권
        this[VariableFloat.CarbonLiquidMass_Tt] = this[VariableFloat.WaterLiquid_PL] * 2.6548961538079303309817862766004e-5f * this[VariableFloat.TotalCarbonRatio_ppm] * _carbonRatio;

        // 생물권
        this[VariableFloat.CarbonLifeMass_Tt] = (this[VariableFloat.PhotoLifeStability] + this[VariableFloat.BreathLifeStability]) * this[VariableFloat.TotalCarbonRatio_ppm] * _carbonRatio * Constants.E_2;

        // 암권
        this[VariableFloat.CarbonSolidMass_Tt] = 60041.3f * this[VariableFloat.TotalCarbonRatio_ppm] * _carbonRatio;
        #endregion 탄소 순환

        #region 생물 생존률
        // 공통 계산
        float0 = Mathf.Abs(this[VariableFloat.TotalAirPressure_hPa] / Constants.EARTH_AIR_PRESSURE - 1.0f) + Math.Abs(this[VariableFloat.TotalTemperature_C] / Constants.EARTH_TEMPERATURE - 1.0f);

        // 광합성 생물 생존율
        this[VariableFloat.PhotoLifePosibility] = (1.0f - float0) * this[VariableFloat.WaterLiquid_PL] / Constants.EARTH_WATER_LIQUID * 100.0f;
        if (100.0f < this[VariableFloat.PhotoLifePosibility])
        {
            this[VariableFloat.PhotoLifePosibility] = 100.0f;
        }

        // 호흡 생물 생존율
        this[VariableFloat.BreathLifePosibility] = (1.0f - float0 - Mathf.Abs(this[VariableFloat.OxygenRatio] / 21.0f - 1.0f)) * this[VariableFloat.WaterLiquid_PL] / Constants.EARTH_WATER_LIQUID * 100.0f;
        if (100.0f < this[VariableFloat.BreathLifePosibility])
        {
            this[VariableFloat.BreathLifePosibility] = 100.0f;
        }
        #endregion 생물 생존률

        #region 생물 안정도
        // 광합성 생물
        if (this[VariableFloat.PhotoLifeStability] > 0.0f)
        {
            this[VariableFloat.PhotoLifeStability] += (this[VariableFloat.PhotoLifePosibility] - this[VariableFloat.PhotoLifeStability]) * Time.deltaTime * GameSpeed * Constants.LIFE_STABILITY_SPEEDMULT;
            if (0.0f > this[VariableFloat.PhotoLifeStability])
            {
                this[VariableFloat.PhotoLifeStability] = 0.0f;
            }
        }

        // 호흡 생물
        if (this[VariableFloat.BreathLifeStability] > 0.0f)
        {
            this[VariableFloat.BreathLifeStability] += (this[VariableFloat.BreathLifePosibility] - this[VariableFloat.BreathLifeStability]) * Time.deltaTime * GameSpeed * Constants.LIFE_STABILITY_SPEEDMULT;
            if (0.0f > this[VariableFloat.BreathLifeStability])
            {
                this[VariableFloat.BreathLifeStability] = 0.0f;
            }
        }
        #endregion 생물 안정도

        #region 산소 농도
        if (0.0f < this[VariableFloat.PhotoLifeStability])
        {
            this[VariableFloat.OxygenRatio] = this[VariableFloat.PhotoLifeStability] * 0.42f - this[VariableFloat.BreathLifeStability] * 0.21f;
        }
        else
        {
            this[VariableFloat.OxygenRatio] = 0.0f;
        }
        #endregion 산소 농도
    }


    /// <summary>
    /// 인위적 환경 조정
    /// </summary>
    private void EnvironmentAdjust()
    {
        // 대기 질량
        switch (this[VariableShort.AirMassMovement])
        {
            case 0:
                break;
            default:
                _etcAirMassGoal += this[VariableShort.AirMassMovement] * Constants.AIRMASS_MOVEMENT;
                if (0.0f > _etcAirMassGoal)
                {
                    _etcAirMassGoal = 0.0f;
                }
                break;
        }

        // 물 양
        switch (this[VariableShort.WaterMovement])
        {
            case 0:
                break;
            default:
                _totalWaterVolumeGoal += this[VariableShort.WaterMovement] * Constants.WATER_VOLUME_MOVEMENT;
                if (0.0f > _totalWaterVolumeGoal)
                {
                    _totalWaterVolumeGoal = 0.0f;
                }
                break;
        }

        // 탄소 농도
        switch (this[VariableShort.CarbonMovement])
        {
            case 0:
                break;
            default:
                _totalCarboneRatioGoal += this[VariableShort.CarbonMovement] * Constants.CARBON_RATIO_MOVEMENT;
                if (0.0f > _totalCarboneRatioGoal)
                {
                    _totalCarboneRatioGoal = 0.0f;
                }
                break;
        }
    }


    /// <summary>
    /// 지지율 변화
    /// </summary>
    private void SupportRateMovement()
    {
        float speedmult = Constants.SUPPORT_RATE_SPEEDMULT * GameSpeed * Time.deltaTime;

        // 목표 지지율. 프로퍼티의 함수를 이용하기 위해 프로퍼티를 사용한다.
        FacilitySupportGoal -= Constants.SUPPORT_RATE_DECREASEMENT * speedmult;
        ResearchSupportGoal -= Constants.SUPPORT_RATE_DECREASEMENT * speedmult;
        SocietySupportGoal -= Constants.SUPPORT_RATE_DECREASEMENT * speedmult;
        DiplomacySupportGoal -= Constants.SUPPORT_RATE_DECREASEMENT * speedmult;

        // 실제 지지율
        this[VariableFloat.FacilitySupportRate] += (_facilitySupportGoal - this[VariableFloat.FacilitySupportRate]) * speedmult;
        this[VariableFloat.ResearchSupportRate] += (_researchSupportGoal - this[VariableFloat.ResearchSupportRate]) * speedmult;
        this[VariableFloat.SocietySupportRate] += (_societySupportGoal - this[VariableFloat.SocietySupportRate]) * speedmult;
        this[VariableFloat.DiplomacySupportRate] += (_diplomacySupportGoal - this[VariableFloat.DiplomacySupportRate]) * speedmult;
    }


    /// <summary>
    /// 월간 비용
    /// </summary>
    private long MonthlyCost()
    {
        return this[VariableShort.AirMassMovement] + this[VariableShort.WaterMovement] + this[VariableShort.TemperatureMovement] + this[VariableShort.CarbonMovement]; 
    }


    /// <summary>
    /// 연간 수익
    /// </summary>
    private void AnnualGains()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Income);

        // 세금 수익
        uint taxIncome = 0;
        foreach (City city in _data.Cities)
        {
            taxIncome += city.TaxIncome;
        }

        // 연간 수익
        this[VariableLong.Funds] += this[VariableInt.AnnualFund] + this[VariableInt.TradeIncome] - this[VariableUint.Maintenance] + taxIncome;
        this[VariableUint.Research] += this[VariableUshort.AnnualResearch];
        this[VariableUint.Culture] += this[VariableUshort.AnnualCulture];
    }


    /// <summary>
    /// 게임 새로 생성
    /// </summary>
    private void CreateGame()
    {
        _data = new JsonData(true);
        this[VariableInt.AnnualFund] = GameManager.Instance.StartFund;
        this[VariableUint.Research] = GameManager.Instance.StartResearch;
        this[VariableUshort.TotalIron] = GameManager.Instance.StartResources;
        this[VariableUshort.TotalNuke] = GameManager.Instance.StartResources;
        this[VariableUshort.TotalJewel] = GameManager.Instance.StartResources;

        this[VariableFloat.EtcAirMass_Tt] = GameManager.Instance.AirMass;
        this[VariableFloat.TotalWater_PL] = GameManager.Instance.WaterVolume;
        this[VariableFloat.TotalCarbonRatio_ppm] = GameManager.Instance.CarbonRatio;
        this[VariableFloat.PlanetRadius_km] = GameManager.Instance.Radius;
        this[VariableFloat.PlanetDensity_g_cm3] = GameManager.Instance.Density;
        this[VariableFloat.PlanetDistance_AU] = GameManager.Instance.Distance;

        this[VariableUshort.CurrentIron] = this[VariableUshort.TotalIron];
        this[VariableUshort.CurrentNuke] = this[VariableUshort.TotalNuke];
        this[VariableUshort.CurrentJewel] = this[VariableUshort.TotalJewel];

        this[VariableByte.Era] = 1;
        this[VariableByte.Month] = 1;
        this[VariableLong.Funds] = Constants.START_FUND;
        this[VariableFloat.ExploreGoal] = Constants.INITIAL_EXPLORE_GOAL;
        this[VariableFloat.PopulationAdjustment] = 1.0f;
        this[VariableFloat.FacilitySupportRate] = 100.0f;
        this[VariableFloat.ResearchSupportRate] = 100.0f;
        this[VariableFloat.SocietySupportRate] = 100.0f;
        this[VariableFloat.DiplomacySupportRate] = 100.0f;

        this[VariableFloat.PlanetArea_km2] = (float)(4.0d * Math.PI * Math.Pow(this[VariableFloat.PlanetRadius_km], 2.0d) * Constants.PLANET_AREA_ADJUST);
        this[VariableFloat.PlanetMass_Tt] = (float)(4.0d / 3.0d * Math.PI * Math.Pow(this[VariableFloat.PlanetRadius_km], 3.0d) * this[VariableFloat.PlanetDensity_g_cm3] * Constants.PLANET_MASS_ADJUST);
        this[VariableFloat.GravityAccelation_m_s2] = (float)(Constants.GRAVITY_COEFICIENT * this[VariableFloat.PlanetMass_Tt] / Math.Pow(this[VariableFloat.PlanetRadius_km], 2.0d) * Constants.PLANET_GRAVITY_ADJUST);
        this[VariableFloat.IncomeEnergy] = Mathf.Atan(1.0f / this[VariableFloat.PlanetDistance_AU]) * 4.0f / Constants.PI;

        // bool 배열은 메세지 출력을 위한 것.
        for (byte i = 0; i < _data.BoolArray.Length; ++i)
        {
            _data.BoolArray[i] = true;
        }

        // 테크트리 정보
        _techTreeData.GetReady();
        FacilityLength = (byte)_techTreeData.GetNodes(TechTreeType.Facility).Length;
        _data.TechAdopted = new float[_techTreeData.GetNodes(TechTreeType.Tech).Length];
        _data.ThoughtAdopted = new float[_techTreeData.GetNodes(TechTreeType.Thought).Length];

        // 세력 이름 직접 입력
        _data.Forces[0] = new Force("지구", 0);
        _data.Forces[0].Culture = 100;
        _data.Forces[1] = new Force("세력1", 1);
        _data.Forces[2] = new Force("세력2", 2);
        _data.Forces[3] = new Force("세력3", 3);
    }


    /// <summary>
    /// 게임 불러온다.
    /// </summary>
    private void LoadGame()
    {
        try
        {
            // 저장된 게임 불러온다.
            _data = JsonUtility.FromJson<JsonData>(Resources.Load("Saves").ToString());
        }
        catch
        {
            // 불러오기 실패하면.
            CreateGame();
            return;
        }

        // 테크트리 정보 준비
        _techTreeData.GetReady();
        FacilityLength = (byte)_techTreeData.GetNodes(TechTreeType.Facility).Length;

        // 토지
        for (ushort i = 0; i < _data.Lands.Count; ++i)
        {
            AddLandSlot(i);
        }

        // 도시
        for (ushort i = 0; i < _data.Cities.Count; ++i)
        {
            AddCitySlot(i);

            // 도시 활성화
            _data.Cities[i].BeginCityRunning();
        }

        // 무역
        for (byte i = 0; i < _data.Trades.Count; ++i)
        {
            AddTradeSlot(i, false);
        }

        // 종자 요청 진행
        if (0 < this[VariableByte.PhotoRequest])
        {
            _photoScreen.RequestCountDown();
        }
        if (0 < this[VariableByte.BreathRequest])
        {
            _breathScreen.RequestCountDown();
        }
    }


    /// <summary>
    /// 게임 완료
    /// </summary>
    private void EndGame(bool isWin)
    {
        IsPlaying = false;
        _isGameEnded = true;
        OnYearChange = null;
        OnMonthChange = null;
        OnPlayUpdate = null;
        _timer = 0.0f;
        GameManager.Instance.IsGameWin = isWin;
    }


    /// <summary>
    /// 메세지 생성
    /// </summary>
    private void MessageEnqueue()
    {
        #region 대기압
        if (this[VariableBool.AirPressure])
        {
            if (900.0f < this[VariableFloat.TotalAirPressure_hPa] && 1100.0f > this[VariableFloat.TotalAirPressure_hPa])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "이상적인 {물리량}에 가까워지고 있습니다."
                    ], Language.Instance["대기압"]);
                this[VariableBool.AirPressure] = false;
            }
        }
        else
        {
            if (900.0f > this[VariableFloat.TotalAirPressure_hPa] || 1100.0f < this[VariableFloat.TotalAirPressure_hPa])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "이상적인 {물리량}에서 멀어지고 있습니다."
                    ], Language.Instance["대기압"]);
                this[VariableBool.AirPressure] = true;
            }
        }
        #endregion

        #region 온도
        if (this[VariableBool.Temperature])
        {
            if (10.0f < this[VariableFloat.TotalTemperature_C] && 20.0f > this[VariableFloat.TotalTemperature_C])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "이상적인 {물리량}에 가까워지고 있습니다."
                    ], Language.Instance["기온"]);
                this[VariableBool.Temperature] = false;
            }
        }
        else
        {
            if (10.0f > this[VariableFloat.TotalTemperature_C] || 20.0f < this[VariableFloat.TotalTemperature_C])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "이상적인 {물리량}에서 멀어지고 있습니다."
                    ], Language.Instance["기온"]);
                this[VariableBool.Temperature] = true;
            }
        }
        #endregion

        #region 광합성 생물
        if (this[VariableBool.Photo])
        {
            if (0.0f < this[VariableFloat.PhotoLifePosibility])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{생물}의 생존률이 올라갑니다."
                    ], Language.Instance["광합성 생물"]);
                this[VariableBool.Photo] = false;
            }
        }
        else
        {
            if (0.0f >= this[VariableFloat.PhotoLifePosibility])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{생물}이 생존할 수 없습니다."
                    ], Language.Instance["광합성 생물"]);
                this[VariableBool.Photo] = true;
            }
        }
        #endregion

        #region 호흡 생물
        if (this[VariableBool.Breath])
        {
            if (0.0f < this[VariableFloat.BreathLifePosibility])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{생물}의 생존률이 올라갑니다."
                    ], Language.Instance["호흡 생물"]);
                this[VariableBool.Breath] = false;
            }
        }
        else
        {
            if (0.0f >= this[VariableFloat.BreathLifePosibility])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{생물}이 생존할 수 없습니다."
                    ], Language.Instance["호흡 생물"]);
                this[VariableBool.Breath] = true;
            }
        }
        #endregion

        #region 산소 농도
        if (this[VariableBool.OxygenRatio])
        {
            if (30.0f < this[VariableFloat.OxygenRatio])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "산소 농도가 너무 높습니다. 호흡 생물의 생존률에 악영향을 줍니다."
                    ]);
                this[VariableBool.OxygenRatio] = false;
            }
        }
        else
        {
            if (25.0f > this[VariableFloat.OxygenRatio])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "산소 농도가 낮아지고 있습니다."
                    ]);
                this[VariableBool.OxygenRatio] = true;
            }
        }
        #endregion
    }


    private void Awake()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        #region 번역 관련
        // 현재 씬에서 모든 AutoTranslation을 찾는다.
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>(true);

        // 모든 AutoTranslation을 준비시킨다.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }

        // 현재 씬에서 모든 InputFieldFontChange 찾는다.
        InputFieldFontChange[] inputFieldFontChange = FindObjectsOfType<InputFieldFontChange>(true);

        // 모든 InputFieldFontChange 준비시킨다.
        for (ushort i = 0; i < inputFieldFontChange.Length; ++i)
        {
            inputFieldFontChange[i].GetReady();
        }
        #endregion

        if (GameManager.Instance.IsNewGame)
        {
            // 새 게임
            CreateGame();
        }
        else
        {
            // 불러온 게임
            LoadGame();
        }

        // 배열 참조
        _adoptedData[(int)TechTreeType.Tech] = _data.TechAdopted;
        _adoptedData[(int)TechTreeType.Thought] = _data.ThoughtAdopted;

        // 미리 활성화할 것.
        _researchScreen.Activate();
        _societyScreen.Activate();

        // 새로 생성된 배열 참조
        _data.SocietyAdopted = _adoptedData[(int)TechTreeType.Society];

        // 고정 값. Update 함수에서 연산을 줄이기 위해 반복해서 사용하는 값은 변수로 저장한다.
        _incomeEnergy_C = this[VariableFloat.IncomeEnergy] * 240.0f;
        _cloudReflectionMultiply = 0.25d / 12.7d / 0.35d;

        // 저장된 값
        _etcAirMassGoal = this[VariableFloat.EtcAirMass_Tt];
        _temperatureMovement = this[VariableShort.TemperatureMovement];
        _totalWaterVolumeGoal = this[VariableFloat.TotalWater_PL];
        _totalCarboneRatioGoal = this[VariableFloat.TotalCarbonRatio_ppm];
        _facilitySupportGoal = this[VariableFloat.FacilitySupportRate];
        _researchSupportGoal = this[VariableFloat.ResearchSupportRate];
        _societySupportGoal = this[VariableFloat.SocietySupportRate];
        _diplomacySupportGoal = this[VariableFloat.DiplomacySupportRate];
    }


    private void Update()
    {
        // 물리량 계산
        PhysicsCalculate();

        // 인위적 환경조정 적용
        EnvironmentMovementAdopt();

        // 플레이 중이 아닐 때
        if (!IsPlaying)
        {
            if (_isGameEnded)
            {
                _timer += Time.deltaTime;
                if (_timer >= 3.0f)
                {
                    if (!GameManager.Instance.IsGameWin)
                    {
                        AudioManager.Instance.ForceStopThemeMusic();
                    }
                    Language.OnLanguageChange = null;
                    SceneManager.LoadScene(2);
                }
            }

            return;
        }

        // 시간 경과
        _timer += Time.deltaTime * GameSpeed;

        // 탐사 진행
        ExploreProgress();

        // 지지율 변화
        SupportRateMovement();

        // 한 달 간격
        if (_timer >= Constants.MONTH_TIMER)
        {
            // 자금이 있을 때
            if (this[VariableLong.Funds] > 0)
            {
                // 인위적 환경 조정
                EnvironmentAdjust();

                // 비용 지출
                this[VariableLong.Funds] -= MonthlyCost();
            }

            // 전체 인구 계산
            this[VariableUint.TotalPopulation] = 0;
            foreach (City city in _data.Cities)
            {
                this[VariableUint.TotalPopulation] += (uint)city.Population;
            }

            // 날짜 변경
            _timer -= Constants.MONTH_TIMER;
            switch (this[VariableByte.Month])
            {
                case 12:
                    {
                        // 새해
                        ++this[VariableUshort.Year];
                        this[VariableByte.Month] = 1;

                        // 연간 수익
                        AnnualGains();

                        // 매해 호출
                        OnYearChange?.Invoke();

                        // 자동 저장
                        _autoSave = true;
                    }
                    break;

                default:
                    {
                        // 다음 달
                        ++this[VariableByte.Month];

                        // 승리 조건
                        if (50.0f < this[VariableFloat.BreathLifeStability])
                        {
                            EndGame(true);
                            MessageBox.Instance.EnqueueMessage(Language.Instance[
                                "생물 안정도가 일정 수준 이상에 도달했습니다. 이 우주에 생명이 살아가는 또다른 행성이 탄생한 순간입니다. 당신은 임무를 완료했습니다."
                                ]);
                            GameManager.Instance.EndGameMessage = "당신의 세력은 행성 테라포밍의 최다 공로를 인정받았습니다.";
                        }
                        else if (4 <= this[VariableByte.Conquested])
                        {
                            EndGame(true);
                            MessageBox.Instance.EnqueueMessage(Language.Instance[
                                "모든 세력을 당신의 속국으로 만들었습니다. 이제 전 행성의 주권은 당신의 세력에게 있습니다. 당신은 임무를 완료했습니다."
                                ]);
                            GameManager.Instance.EndGameMessage = "당신의 세력은 행성을 정복하여 그 강력함을 인정받았습니다.";
                        }

                        // 패배 조건

                        // 메세지 생성
                        MessageEnqueue();
                    }
                    break;
            }

            // 매달 호출
            OnMonthChange?.Invoke();
        }

        // 매 프레임 호출
        OnPlayUpdate?.Invoke();

        // 게임 저장
        if (_autoSave)
        {
            SaveGame();
            _autoSave = false;
        }
    }



    /* ==================== Struct ==================== */

    [Serializable]
    public struct JsonData
    {
        public byte[] ByteArray;
        public short[] ShortArray;
        public ushort[] UshortArray;
        public int[] IntArray;
        public uint[] UintArray;
        public long[] LongArray;
        public float[] FloatArray;
        public double[] DoubleArray;
        public bool[] BoolArray;
        public float[] TechAdopted;
        public float[] ThoughtAdopted;
        public float[] SocietyAdopted;
        public float[] SocietyElementProgression;
        public Force[] Forces;
        public List<Land> Lands;
        public List<City> Cities;
        public List<Trade> Trades;

        public JsonData(bool initialize)
        {
            if (initialize)
            {
                ByteArray = new byte[(int)VariableByte.EndByte];
                ShortArray = new short[(int)VariableShort.EndShort];
                UshortArray = new ushort[(int)VariableUshort.EndUshort];
                IntArray = new int[(int)VariableInt.EndInt];
                UintArray = new uint[(int)VariableUint.EndUint];
                LongArray = new long[(int)VariableLong.EndLong];
                FloatArray = new float[(int)VariableFloat.EndFloat];
                DoubleArray = new double[(int)VariableDouble.EndDouble];
                BoolArray = new bool[(int)VariableBool.EndBool];
                Forces = new Force[Constants.NUMBER_OF_FORCES];
                Lands = new List<Land>();
                Cities = new List<City>();
                Trades = new List<Trade>();
            }
            else
            {
                ByteArray = null;
                ShortArray = null;
                UshortArray = null;
                IntArray = null;
                UintArray = null;
                LongArray = null;
                FloatArray = null;
                DoubleArray = null;
                BoolArray = null;
                Forces = null;
                Lands = null;
                Cities = null;
                Trades = null;
            }


            TechAdopted = null;
            ThoughtAdopted = null;
            SocietyAdopted = null;
            SocietyElementProgression = null;
    }
    }
}
