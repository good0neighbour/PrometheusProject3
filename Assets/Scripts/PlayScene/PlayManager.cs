using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    public static OnChangeDelegate OnPlayUpdate = null;
    public static OnChangeDelegate OnMonthCahnge = null;

    [Header("����")]
    [SerializeField] private GameObject _audioManagerPrefab = null;
    [SerializeField] private GameObject _landSlot = null;
    [SerializeField] private GameObject _citySlot = null;
    [SerializeField] private Transform _landListContentArea = null;
    [SerializeField] private Transform _cityListContentArea = null;
    [SerializeField] private TechTrees _techTreeData = null;
    [SerializeField] private ScreenResearch _researchScreen = null;
    [SerializeField] private ScreenSociety _societyScreen = null;

    private JsonData _data;
    private List<Land> _lands = new List<Land>();
    private List<City> _cities = new List<City>();
    private float _timer = 0.0f;
    private float _gameSpeed = 1.0f;
    private float _etcAirMassGoal = 0.0f;
    private float _temperatureMovement = 0.0f;
    private float _totalWaterVolumeGoal = 0.0f;
    private float _totalCarboneRatioGoal = 0.0f;
    private float _incomeEnergy_C = 0.0f;
    private float _carbonRatio = 1.0f / Constants.EARTH_CARBON_RATIO;
    private double _cloudReflectionMultiply = 0.0d;

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

    #region JsonData�� �迭�� ����
    /// <summary>
    /// ���� ������ ���� �������. byte.
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
    /// ���� ������ ���� �������. short.
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
    /// ���� ������ ���� �������. ushort.
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
    /// ���� ������ ���� �������. int.
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
    /// ���� ������ ���� �������. long.
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
    /// ���� ������ ���� �������. float.
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
    /// ���� ������ ���� �������. double.
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
    #endregion



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ���� ���� �����´�.
    /// </summary>
    public Land GetLand(ushort index)
    {
        return _lands[index];
    }


    /// <summary>
    /// ���� ������ �����´�.
    /// </summary>
    public City GetCity(ushort index)
    {
        return _cities[index];
    }


    /// <summary>
    /// ���� ���� �߰�
    /// </summary>
    public void AddLandSlot(ushort landNum)
    {
        // ���� ��ư �߰� �� �ʱ�ȭ
        Instantiate(_landSlot, _landListContentArea).GetComponent<SlotLand>().SlotInitialize(landNum);
    }


    /// <summary>
    /// ���� �߰�
    /// </summary>
    public void AddCity(ushort landNum, string cityName, ushort capacity)
    {
        // �����迭�� ���� �߰�
        _cities.Add(new City(this[VariableUshort.CityNum], landNum, cityName, capacity));
    }


    /// <summary>
    /// ���� ���� �߰�
    /// </summary>
    public void AddCitySlot(ushort cityNum)
    {
        // ���� ��ư �߰� �� �ʱ�ȭ
        Instantiate(_citySlot, _cityListContentArea).GetComponent<SlotCity>().SlotInitialize(cityNum);
    }


    /// <summary>
    /// ��ũ Ʈ�� ���� �����´�.
    /// </summary>
    public TechTrees GetTechTreeData()
    {
        return _techTreeData;
    }


    /// <summary>
    /// Ȱ��ȭ ���� ���� �����´�.
    /// </summary>
    public float[][] GetAdoptedData()
    {
        return _data.Adopted;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// Ž�� ����
    /// </summary>
    private void ExploreProgress()
    {
        // ���� ���� ushort�� �ִ�ġ�� ���� ���� ����
        if (ushort.MaxValue > this[VariableUshort.LandNum])
        {
            // Ž�� �Ϸ� ��
            if (this[VariableFloat.ExploreProgress] >= this[VariableFloat.ExploreGoal])
            {
                // �����迭�� ���� �߰�
                _lands.Add(new Land(this[VariableUshort.LandNum], RandomResources()));

                // ���� ���� �߰�
                AddLandSlot(this[VariableUshort.LandNum]);

                // ���� ���� �߰�
                ++this[VariableUshort.LandNum];

                // Ž�� ���� �ʱ�ȭ
                this[VariableFloat.ExploreProgress] = 0.0f;

                // Ž�� ��ǥ ����
                this[VariableFloat.ExploreGoal] *= Constants.EXPLORE_GOAL_INCREASEMENT;
            }
            // ���� ���� ��
            else
            {
                // �ð��� ���� ���൵ ���
                this[VariableFloat.ExploreProgress] += this[VariableByte.ExploreDevice] * Time.deltaTime * Constants.EXPLORE_SPEEDMULT * GameSpeed;
            }
        }
    }


    /// <summary>
    /// ������ �ڿ� ����
    /// </summary>
    private byte[] RandomResources()
    {
        // �ڿ� ����
        int[] resources = new int[(int)ResourceType.End];
        int total;

        // ��� �ϳ��� 0�� �ƴ� ������ �ݺ�
        do
        {
            // �� �� �ʱ�ȭ
            total = 0;

            for (byte i = 0; i < (byte)ResourceType.End; ++i)
            {
                // ������ ��
                resources[i] = Random.Range(Constants.RESOURCE_MIN_MAX[i, 0], Constants.RESOURCE_MIN_MAX[i, 1] + 1);

                // 0�� ����
                if (0 > resources[i])
                {
                    resources[i] = 0;
                }

                // ����
                total += resources[i];
            }
        } while (0 == total);

        // �迭 ����
        byte[] result = new byte[(int)ResourceType.End];
        for (byte i = 0; i < (byte)ResourceType.End; ++i)
        {
            result[i] = (byte)resources[i];
        }

        //��ȯ
        return result;
    }


    /// <summary>
    /// ȯ�� ������ õõ�� ����.
    /// </summary>
    private void EnvironmentMovementAdopt()
    {
        this[VariableFloat.EtcAirMass_Tt] += (_etcAirMassGoal - this[VariableFloat.EtcAirMass_Tt]) * Time.deltaTime * GameSpeed;
        this[VariableFloat.TotalWater_PL] += (_totalWaterVolumeGoal - this[VariableFloat.TotalWater_PL]) * Time.deltaTime * GameSpeed;
        _temperatureMovement += (this[VariableShort.TemperatureMovement] - _temperatureMovement) * Time.deltaTime * GameSpeed;
        this[VariableFloat.TotalCarbonRatio_ppm] += (_totalCarboneRatioGoal - this[VariableFloat.TotalCarbonRatio_ppm]) * Time.deltaTime * GameSpeed;
    }


    /// <summary>
    /// ������ ���
    /// </summary>
    private void PhysicsCalculate()
    {
        /*
        �Һ��� ������

        ���ӵ� = (�߷»�� * �༺����) / �����ݰ�^2 * ���ӵ�����
        �༺���� = 4 / 3 * pi * �����ݰ�^3 * �е� * ��������
        ���� = 4 * pi * �����ݰ�^2 * ��������

        �Ի翡���� = �Ÿ�����^2
        �Ÿ����� = 1(AU) / �Ÿ�(AU)
        */

        // ���� ��������
        double double0;
        double double1;
        float float0;

        // õ�������� ����� �⺻������ ū �ڷ������� ��ȯ �� ����Ѵ�.
        #region ����
        // ��ü����
        this[VariableFloat.TotalAirMass_Tt] = this[VariableFloat.WaterGas_PL] + this[VariableFloat.CarbonGasMass_Tt] + this[VariableFloat.EtcAirMass_Tt];

        // ����
        this[VariableFloat.TotalAirPressure_hPa] = (float)(Constants.E7 * this[VariableFloat.TotalAirMass_Tt] * this[VariableFloat.GravityAccelation_m_s2] / this[VariableFloat.PlanetArea_km2]);
        #endregion ����

        #region ���
        // ������ �½�
        this[VariableFloat.WaterGreenHouse_C] = (float)(24.06923987d * Math.Log10(this[VariableFloat.WaterGas_PL] + 1.0d));

        // ź�� �½�
        this[VariableFloat.CarbonGreenHouse_C] = (float)(20.97411189d * Math.Log10(this[VariableFloat.CarbonGasMass_Tt] + 1.0d));

        // ��Ÿ �½�
        this[VariableFloat.EtcGreenHouse_C] = (float)((1.53162266d) * Math.Log10(this[VariableFloat.EtcAirMass_Tt] + 1.0d));

        // ���� �ݻ��� ���
        double0 = this[VariableFloat.WaterGas_PL] * _cloudReflectionMultiply;
        // �༺�� �β��� ���� �������� ��
        if (double0 > 1.0d)
        {
            this[VariableFloat.CloudReflection] = 0.35f;
            this[VariableFloat.IceReflection] = 0.0f;
            this[VariableFloat.WaterReflection] = 0.0f;
            this[VariableFloat.GroundReflection] = 0.0f;
        }
        // ���翡������ ��ǥ����� ����
        else
        {
            // ���� �ݻ���
            this[VariableFloat.CloudReflection] = (float)(double0 * 0.35d);

            // ���� �ݻ��� ���
            double0 = 1.0d - double0;
            double1 = this[VariableFloat.WaterSolid_PL] * 1.3409961685823754789272030651341e-5d;
            // �༺ ǥ���� ���� �������� �������� ��
            if (double1 > 1.0d)
            {
                this[VariableFloat.IceReflection] = (float)(double0 * 0.9d);
                this[VariableFloat.WaterReflection] = 0.0f;
                this[VariableFloat.GroundReflection] = 0.0f;
            }
            // �༺ ǥ���� ���� �������� �������� ���� ��
            else
            {
                // ���� �ݻ���
                double1 *= double0;
                this[VariableFloat.IceReflection] = (float)(double1 * 0.9d);

                // �� �ݻ��� ���
                double0 -= double1;
                double1 = double0 * this[VariableFloat.WaterLiquid_PL] * 2.9397176744512440041043142009695e-8d;
                // �༺ ǥ���� ���� �������� ��
                if (double1 > 1.0d)
                {
                    this[VariableFloat.WaterReflection] = (float)(double0 * 0.035d);
                    this[VariableFloat.GroundReflection] = 0.0f;
                }
                // ������ �巯�� ��
                else
                {
                    // �� �ݻ���
                    double1 *= double0;
                    this[VariableFloat.WaterReflection] = (float)(double1 * 0.035d);

                    // ���� �ݻ���
                    double0 -= double1;
                    this[VariableFloat.GroundReflection] = (float)(double0 * 0.1d);
                }
            }
        }

        // �� �ݻ���
        this[VariableFloat.TotalReflection] = this[VariableFloat.GroundReflection] + this[VariableFloat.WaterReflection] + this[VariableFloat.IceReflection] + this[VariableFloat.CloudReflection];

        // ���������
        this[VariableFloat.AbsorbEnergy] = this[VariableFloat.IncomeEnergy] * (1.0f - this[VariableFloat.TotalReflection]);

        // ���
        this[VariableFloat.TotalTemperature_C] = Constants.MIN_KELVIN + _incomeEnergy_C + this[VariableFloat.AbsorbEnergy] * 15.797788309636650868878357030016f + this[VariableFloat.WaterGreenHouse_C] + this[VariableFloat.CarbonGreenHouse_C] + this[VariableFloat.EtcGreenHouse_C] + _temperatureMovement;
        if (this[VariableFloat.TotalTemperature_C] < Constants.MIN_KELVIN)
        {
            this[VariableFloat.TotalTemperature_C] = Constants.MIN_KELVIN;
        }
        #endregion ���

        #region ���� ü��
        // ��ü

        // ��ü
        double0 = 8.3269949383584792498079949276151e-7d * Math.Pow(1.0630781589386992356184590458984d, this[VariableFloat.TotalTemperature_C] + 23.94160508d);
        if (double0 > 1.0d)
        {
            double0 = 1.0d;
        }
        this[VariableFloat.WaterGas_PL] = (float)(this[VariableFloat.TotalWater_PL] * double0);

        // ��ü
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

        // ��ü
        this[VariableFloat.WaterLiquid_PL] = this[VariableFloat.TotalWater_PL] - (this[VariableFloat.WaterGas_PL] + this[VariableFloat.WaterSolid_PL]);
        #endregion ���� ü��

        #region ź�� ��ȯ
        // ���
        this[VariableFloat.CarbonGasMass_Tt] = this[VariableFloat.TotalAirPressure_hPa] * 7.1058475203552923760177646188009e-4f * this[VariableFloat.TotalCarbonRatio_ppm] * _carbonRatio;

        // ����
        this[VariableFloat.CarbonLiquidMass_Tt] = this[VariableFloat.WaterLiquid_PL] * 2.6548961538079303309817862766004e-5f * this[VariableFloat.TotalCarbonRatio_ppm] * _carbonRatio;

        // ������
        this[VariableFloat.CarbonLifeMass_Tt] = (this[VariableFloat.PhotoLifeStability] + this[VariableFloat.BreathLifeStability]) * this[VariableFloat.TotalCarbonRatio_ppm] * _carbonRatio * Constants.E_2;

        // �ϱ�
        this[VariableFloat.CarbonSolidMass_Tt] = 60041.3f * this[VariableFloat.TotalCarbonRatio_ppm] * _carbonRatio;
        #endregion ź�� ��ȯ

        #region ���� ������
        // ���� ���
        float0 = Mathf.Abs(this[VariableFloat.TotalAirPressure_hPa] / Constants.EARTH_AIR_PRESSURE - 1.0f) + Math.Abs(this[VariableFloat.TotalTemperature_C] / Constants.EARTH_TEMPERATURE - 1.0f);

        // ���ռ� ���� ������
        this[VariableFloat.PhotoLifePosibility] = (1.0f - float0) * this[VariableFloat.WaterLiquid_PL] / Constants.EARTH_WATER_LIQUID * 100.0f;
        if (100.0f < this[VariableFloat.PhotoLifePosibility])
        {
            this[VariableFloat.PhotoLifePosibility] = 100.0f;
        }

        // ȣ�� ���� ������
        this[VariableFloat.BreathLifePosibility] = (1.0f - float0 - Mathf.Abs(this[VariableFloat.OxygenRatio] / 21.0f - 1.0f)) * this[VariableFloat.WaterLiquid_PL] / Constants.EARTH_WATER_LIQUID * 100.0f;
        if (100.0f < this[VariableFloat.BreathLifePosibility])
        {
            this[VariableFloat.BreathLifePosibility] = 100.0f;
        }
        #endregion ���� ������

        #region ���� ������
        // ���ռ� ����
        if (this[VariableFloat.PhotoLifeStability] > 0.0f)
        {
            this[VariableFloat.PhotoLifeStability] += (this[VariableFloat.PhotoLifePosibility] - this[VariableFloat.PhotoLifeStability]) * Time.deltaTime * GameSpeed * Constants.LIFE_STABILITY_SPEEDMULT;
            if (0.0f > this[VariableFloat.PhotoLifeStability])
            {
                this[VariableFloat.PhotoLifeStability] = 0.0f;
            }
        }

        // ȣ�� ����
        if (this[VariableFloat.BreathLifeStability] > 0.0f)
        {
            this[VariableFloat.BreathLifeStability] += (this[VariableFloat.BreathLifePosibility] - this[VariableFloat.BreathLifeStability]) * Time.deltaTime * GameSpeed * Constants.LIFE_STABILITY_SPEEDMULT;
            if (0.0f > this[VariableFloat.BreathLifeStability])
            {
                this[VariableFloat.BreathLifeStability] = 0.0f;
            }
        }
        #endregion ���� ������

        #region ��� ��
        if (0.0f < this[VariableFloat.PhotoLifeStability])
        {
            this[VariableFloat.OxygenRatio] = this[VariableFloat.PhotoLifeStability] * 0.42f - this[VariableFloat.BreathLifeStability] * 0.21f;
        }
        else
        {
            this[VariableFloat.OxygenRatio] = 0.0f;
        }
        #endregion ��� ��
    }


    /// <summary>
    /// ������ ȯ�� ����
    /// </summary>
    private void EnvironmentAdjust()
    {
        // ��� ����
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

        // �� ��
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

        // ź�� ��
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
    /// ���� ���
    /// </summary>
    private long MonthlyCost()
    {
        return this[VariableShort.AirMassMovement] + this[VariableShort.WaterMovement] + this[VariableShort.TemperatureMovement] + this[VariableShort.CarbonMovement]; 
    }


    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        #region �ӽ�
        _data = new JsonData(true);
        this[VariableFloat.ExploreGoal] = Constants.INITIAL_EXPLORE_GOAL;
        this[VariableLong.Funds] = 500000;
        this[VariableFloat.EtcAirMass_Tt] = 5134.58f;
        this[VariableFloat.TotalWater_PL] = Constants.EARTH_WATER_VOLUME;
        this[VariableFloat.PlanetRadius_km] = Constants.EARTH_RADIUS;
        this[VariableFloat.PlanetDensity_g_cm3] = Constants.EARTH_DENSITY;
        this[VariableFloat.TotalCarbonRatio_ppm] = Constants.EARTH_CARBON_RATIO;

        this[VariableFloat.PlanetArea_km2] = (float)(4.0d * Math.PI * Math.Pow(this[VariableFloat.PlanetRadius_km], 2.0d) * Constants.PLANET_AREA_ADJUST);

        this[VariableFloat.PlanetMass_Tt] = (float)(4.0d / 3.0d * Math.PI * Math.Pow(this[VariableFloat.PlanetRadius_km], 3.0d) * this[VariableFloat.PlanetDensity_g_cm3] * Constants.PLANET_MASS_ADJUST);

        this[VariableFloat.GravityAccelation_m_s2] = (float)(Constants.GRAVITY_COEFICIENT * this[VariableFloat.PlanetMass_Tt] / Math.Pow(this[VariableFloat.PlanetRadius_km], 2.0d) * Constants.PLANET_GRAVITY_ADJUST);

        this[VariableFloat.IncomeEnergy] = 1.0f;
        #endregion

        #region AudioManager ����
        // ���� �� �׽�Ʈ �� ���� ����
        if (null == AudioManager.Instance)
        {
            GameObject audioManager = Instantiate(_audioManagerPrefab);
            AudioManager.Instance = audioManager.GetComponent<AudioManager>();
            DontDestroyOnLoad(audioManager);
        }
        #endregion

        #region ���� �غ�
        // ���� ������ ��� AutoTranslation�� ã�´�.
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>(true);

        // ��� AutoTranslation�� �غ��Ų��.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }

        // ���� ������ ��� InputFieldFontChange ã�´�.
        InputFieldFontChange[] inputFieldFontChange = FindObjectsOfType<InputFieldFontChange>(true);

        // ��� InputFieldFontChange �غ��Ų��.
        for (ushort i = 0; i < inputFieldFontChange.Length; ++i)
        {
            inputFieldFontChange[i].GetReady();
        }

        // ��� �ҷ��´�.
        Language.Instance.LoadLangeage(GameManager.Instance.CurrentLanguage);
        #endregion

        // ��ũƮ�� ���� �غ�
        _techTreeData.GetReady();
        for (TechTreeType i = 0; i < TechTreeType.TechTreeEnd; ++i)
        {
            switch (i)
            {
                case TechTreeType.Facility:
                    // �ü� ����� ũ�⸸ �����´�.
                    FacilityLength = (byte)_techTreeData.GetNodes(i).Length;
                    break;
                case TechTreeType.Society:
                    // ��ȸ ����� �������� �ʴ´�.
                    break;
                default:
                    byte length = (byte)_techTreeData.GetNodes(i).Length;
                    _data.Adopted[(int)i] = new float[length];
                    break;
            }
        }

        // �̸� Ȱ��ȭ�� ��.
        _researchScreen.Activate();
        _societyScreen.Activate();

        // ���� ��. Update �Լ����� ������ ���̱� ���� �ݺ��Ǵ� ���� ������ �����Ѵ�.
        _incomeEnergy_C = this[VariableFloat.IncomeEnergy] * 240.0f;
        _cloudReflectionMultiply = 0.25d / 12.7d / 0.35d;

        // ����� ��
        _etcAirMassGoal = this[VariableFloat.EtcAirMass_Tt];
        _temperatureMovement = this[VariableShort.TemperatureMovement];
        _totalWaterVolumeGoal = this[VariableFloat.TotalWater_PL];
        _totalCarboneRatioGoal = this[VariableFloat.TotalCarbonRatio_ppm];
    }


    private void Update()
    {
        // ������ ���
        PhysicsCalculate();

        // ������ ȯ������ ����
        EnvironmentMovementAdopt();

        // ���� ���̸� �Լ� ����.
        if (!IsPlaying)
        {
            return;
        }

        // �ð� ���
        _timer += Time.deltaTime * GameSpeed;

        // Ž�� ����
        ExploreProgress();

        // �� ������ ȣ��
        OnPlayUpdate?.Invoke();

        // �� �� ����
        if (_timer >= Constants.MONTH_TIMER)
        {
            _timer -= Constants.MONTH_TIMER;

            // �ڱ��� ���� ��
            if (this[VariableLong.Funds] > 0)
            {
                // ������ ȯ�� ����
                EnvironmentAdjust();

                // ��� ����
                this[VariableLong.Funds] -= MonthlyCost();
            }

            // �Ŵ� ȣ��
            OnMonthCahnge?.Invoke();
        }
    }



    /* ==================== Struct ==================== */

    private struct JsonData
    {
        public byte[] ByteArray;
        public short[] ShortArray;
        public ushort[] UshortArray;
        public int[] IntArray;
        public long[] LongArray;
        public float[] FloatArray;
        public double[] DoubleArray;
        public float[][] Adopted;

        public JsonData(bool initialize)
        {
            if (initialize)
            {
                ByteArray = new byte[(int)VariableByte.EndByte];
                ShortArray = new short[(int)VariableShort.EndShort];
                UshortArray = new ushort[(int)VariableUshort.EndUshort];
                IntArray = new int[(int)VariableInt.EndInt];
                LongArray = new long[(int)VariableLong.EndLong];
                FloatArray = new float[(int)VariableFloat.EndFloat];
                DoubleArray = new double[(int)VariableDouble.EndDouble];
                Adopted = new float[(int)TechTreeType.TechTreeEnd][];
            }
            else
            {
                ByteArray = null;
                ShortArray = null;
                UshortArray = null;
                IntArray = null;
                LongArray = null;
                FloatArray = null;
                DoubleArray = null;
                Adopted = null;
            }
        }
    }
}
