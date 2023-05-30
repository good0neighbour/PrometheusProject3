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

    [Header("����")]
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
    /// ���� ������ ���� �������. uint.
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

    /// <summary>
    /// ���� ������ ���� �������. bool.
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
    /// ���� ���� �����´�.
    /// </summary>
    public Land GetLand(ushort index)
    {
        return _data.Lands[index];
    }


    /// <summary>
    /// ���� ������ �����´�.
    /// </summary>
    public City GetCity(ushort index)
    {
        return _data.Cities[index];
    }


    /// <summary>
    /// ���� ������ �����´�.
    /// </summary>
    public Force GetForce(ushort index)
    {
        return _data.Forces[index];
    }


    /// <summary>
    /// ���� ������ �����´�.
    /// </summary>
    public Trade GetTrade(ushort index)
    {
        return _data.Trades[index];
    }


    /// <summary>
    /// ���� ������ �����Ѵ�.
    /// </summary>
    public void RemoveTrade(Trade trade)
    {
        _data.Trades.Remove(trade);
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
    public void AddCity(string cityName, ushort capacity)
    {
        // �����迭�� ���� �߰�
        _data.Cities.Add(new City(cityName, capacity));
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
    /// �ŷ� �߰�
    /// </summary>
    public void AddTrade(Trade trade)
    {
        // �����迭�� �ŷ� �߰�
        _data.Trades.Add(trade);
    }


    /// <summary>
    /// �ŷ� ���� �߰�
    /// </summary>
    public void AddTradeSlot(ushort tradeNum, bool firstAdd)
    {
        // �ŷ� ��ư �߰� �� �ʱ�ȭ
        SlotTrade slot = Instantiate(_tradeSlot, _tradeListContentArea).GetComponent<SlotTrade>();
        slot.SlotInitialize(tradeNum);

        // ����� ����Ÿ �ҷ����� ��찡 �ƴ� ��
        if (firstAdd)
        {
            slot.RunTrade(1);
        }
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
        return _adoptedData;
    }


    /// <summary>
    /// ��ȸ ���� ��� ���൵ ��ȯ
    /// </summary>
    public float[] GetSocietyElementProgression()
    {
        return _data.SocietyElementProgression;
    }


    /// <summary>
    /// �迭 ���� �� ��ȯ
    /// </summary>
    public float[] GetSocietyElementProgression(byte length)
    {
        _data.SocietyElementProgression = new float[length];
        return _data.SocietyElementProgression;
    }


    /// <summary>
    /// ���� ����
    /// </summary>
    public void SaveGame()
    {
        File.WriteAllText($"{Application.dataPath}/Resources/Saves.Json", JsonUtility.ToJson(_data, false));
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
                _data.Lands.Add(new Land(this[VariableUshort.LandNum], RandomResources()));

                // ���� ���� �߰�
                AddLandSlot(this[VariableUshort.LandNum]);

                // ���� ���� �߰�
                ++this[VariableUshort.LandNum];

                // �޼���
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "���ο� ������ �߰��߽��ϴ�."
                    ]);

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
        this[VariableFloat.WaterLiquid_PL] = this[VariableFloat.TotalWater_PL] - (this[VariableFloat.WaterGas_PL] - this[VariableFloat.WaterSolid_PL]);
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
    /// ������ ��ȭ
    /// </summary>
    private void SupportRateMovement()
    {
        float speedmult = Constants.SUPPORT_RATE_SPEEDMULT * GameSpeed * Time.deltaTime;

        // ��ǥ ������. ������Ƽ�� �Լ��� �̿��ϱ� ���� ������Ƽ�� ����Ѵ�.
        FacilitySupportGoal -= Constants.SUPPORT_RATE_DECREASEMENT * speedmult;
        ResearchSupportGoal -= Constants.SUPPORT_RATE_DECREASEMENT * speedmult;
        SocietySupportGoal -= Constants.SUPPORT_RATE_DECREASEMENT * speedmult;
        DiplomacySupportGoal -= Constants.SUPPORT_RATE_DECREASEMENT * speedmult;

        // ���� ������
        this[VariableFloat.FacilitySupportRate] += (_facilitySupportGoal - this[VariableFloat.FacilitySupportRate]) * speedmult;
        this[VariableFloat.ResearchSupportRate] += (_researchSupportGoal - this[VariableFloat.ResearchSupportRate]) * speedmult;
        this[VariableFloat.SocietySupportRate] += (_societySupportGoal - this[VariableFloat.SocietySupportRate]) * speedmult;
        this[VariableFloat.DiplomacySupportRate] += (_diplomacySupportGoal - this[VariableFloat.DiplomacySupportRate]) * speedmult;
    }


    /// <summary>
    /// ���� ���
    /// </summary>
    private long MonthlyCost()
    {
        return this[VariableShort.AirMassMovement] + this[VariableShort.WaterMovement] + this[VariableShort.TemperatureMovement] + this[VariableShort.CarbonMovement]; 
    }


    /// <summary>
    /// ���� ����
    /// </summary>
    private void AnnualGains()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Income);

        // ���� ����
        uint taxIncome = 0;
        foreach (City city in _data.Cities)
        {
            taxIncome += city.TaxIncome;
        }

        // ���� ����
        this[VariableLong.Funds] += this[VariableInt.AnnualFund] + this[VariableInt.TradeIncome] - this[VariableUint.Maintenance] + taxIncome;
        this[VariableUint.Research] += this[VariableUshort.AnnualResearch];
        this[VariableUint.Culture] += this[VariableUshort.AnnualCulture];
    }


    /// <summary>
    /// ���� ���� ����
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

        // bool �迭�� �޼��� ����� ���� ��.
        for (byte i = 0; i < _data.BoolArray.Length; ++i)
        {
            _data.BoolArray[i] = true;
        }

        // ��ũƮ�� ����
        _techTreeData.GetReady();
        FacilityLength = (byte)_techTreeData.GetNodes(TechTreeType.Facility).Length;
        _data.TechAdopted = new float[_techTreeData.GetNodes(TechTreeType.Tech).Length];
        _data.ThoughtAdopted = new float[_techTreeData.GetNodes(TechTreeType.Thought).Length];

        // ���� �̸� ���� �Է�
        _data.Forces[0] = new Force("����", 0);
        _data.Forces[0].Culture = 100;
        _data.Forces[1] = new Force("����1", 1);
        _data.Forces[2] = new Force("����2", 2);
        _data.Forces[3] = new Force("����3", 3);
    }


    /// <summary>
    /// ���� �ҷ��´�.
    /// </summary>
    private void LoadGame()
    {
        try
        {
            // ����� ���� �ҷ��´�.
            _data = JsonUtility.FromJson<JsonData>(Resources.Load("Saves").ToString());
        }
        catch
        {
            // �ҷ����� �����ϸ�.
            CreateGame();
            return;
        }

        // ��ũƮ�� ���� �غ�
        _techTreeData.GetReady();
        FacilityLength = (byte)_techTreeData.GetNodes(TechTreeType.Facility).Length;

        // ����
        for (ushort i = 0; i < _data.Lands.Count; ++i)
        {
            AddLandSlot(i);
        }

        // ����
        for (ushort i = 0; i < _data.Cities.Count; ++i)
        {
            AddCitySlot(i);

            // ���� Ȱ��ȭ
            _data.Cities[i].BeginCityRunning();
        }

        // ����
        for (byte i = 0; i < _data.Trades.Count; ++i)
        {
            AddTradeSlot(i, false);
        }

        // ���� ��û ����
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
    /// ���� �Ϸ�
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
    /// �޼��� ����
    /// </summary>
    private void MessageEnqueue()
    {
        #region ����
        if (this[VariableBool.AirPressure])
        {
            if (900.0f < this[VariableFloat.TotalAirPressure_hPa] && 1100.0f > this[VariableFloat.TotalAirPressure_hPa])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "�̻����� {������}�� ��������� �ֽ��ϴ�."
                    ], Language.Instance["����"]);
                this[VariableBool.AirPressure] = false;
            }
        }
        else
        {
            if (900.0f > this[VariableFloat.TotalAirPressure_hPa] || 1100.0f < this[VariableFloat.TotalAirPressure_hPa])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "�̻����� {������}���� �־����� �ֽ��ϴ�."
                    ], Language.Instance["����"]);
                this[VariableBool.AirPressure] = true;
            }
        }
        #endregion

        #region �µ�
        if (this[VariableBool.Temperature])
        {
            if (10.0f < this[VariableFloat.TotalTemperature_C] && 20.0f > this[VariableFloat.TotalTemperature_C])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "�̻����� {������}�� ��������� �ֽ��ϴ�."
                    ], Language.Instance["���"]);
                this[VariableBool.Temperature] = false;
            }
        }
        else
        {
            if (10.0f > this[VariableFloat.TotalTemperature_C] || 20.0f < this[VariableFloat.TotalTemperature_C])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "�̻����� {������}���� �־����� �ֽ��ϴ�."
                    ], Language.Instance["���"]);
                this[VariableBool.Temperature] = true;
            }
        }
        #endregion

        #region ���ռ� ����
        if (this[VariableBool.Photo])
        {
            if (0.0f < this[VariableFloat.PhotoLifePosibility])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{����}�� �������� �ö󰩴ϴ�."
                    ], Language.Instance["���ռ� ����"]);
                this[VariableBool.Photo] = false;
            }
        }
        else
        {
            if (0.0f >= this[VariableFloat.PhotoLifePosibility])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{����}�� ������ �� �����ϴ�."
                    ], Language.Instance["���ռ� ����"]);
                this[VariableBool.Photo] = true;
            }
        }
        #endregion

        #region ȣ�� ����
        if (this[VariableBool.Breath])
        {
            if (0.0f < this[VariableFloat.BreathLifePosibility])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{����}�� �������� �ö󰩴ϴ�."
                    ], Language.Instance["ȣ�� ����"]);
                this[VariableBool.Breath] = false;
            }
        }
        else
        {
            if (0.0f >= this[VariableFloat.BreathLifePosibility])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{����}�� ������ �� �����ϴ�."
                    ], Language.Instance["ȣ�� ����"]);
                this[VariableBool.Breath] = true;
            }
        }
        #endregion

        #region ��� ��
        if (this[VariableBool.OxygenRatio])
        {
            if (30.0f < this[VariableFloat.OxygenRatio])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "��� �󵵰� �ʹ� �����ϴ�. ȣ�� ������ �������� �ǿ����� �ݴϴ�."
                    ]);
                this[VariableBool.OxygenRatio] = false;
            }
        }
        else
        {
            if (25.0f > this[VariableFloat.OxygenRatio])
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "��� �󵵰� �������� �ֽ��ϴ�."
                    ]);
                this[VariableBool.OxygenRatio] = true;
            }
        }
        #endregion
    }


    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        #region ���� ����
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
        #endregion

        if (GameManager.Instance.IsNewGame)
        {
            // �� ����
            CreateGame();
        }
        else
        {
            // �ҷ��� ����
            LoadGame();
        }

        // �迭 ����
        _adoptedData[(int)TechTreeType.Tech] = _data.TechAdopted;
        _adoptedData[(int)TechTreeType.Thought] = _data.ThoughtAdopted;

        // �̸� Ȱ��ȭ�� ��.
        _researchScreen.Activate();
        _societyScreen.Activate();

        // ���� ������ �迭 ����
        _data.SocietyAdopted = _adoptedData[(int)TechTreeType.Society];

        // ���� ��. Update �Լ����� ������ ���̱� ���� �ݺ��ؼ� ����ϴ� ���� ������ �����Ѵ�.
        _incomeEnergy_C = this[VariableFloat.IncomeEnergy] * 240.0f;
        _cloudReflectionMultiply = 0.25d / 12.7d / 0.35d;

        // ����� ��
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
        // ������ ���
        PhysicsCalculate();

        // ������ ȯ������ ����
        EnvironmentMovementAdopt();

        // �÷��� ���� �ƴ� ��
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

        // �ð� ���
        _timer += Time.deltaTime * GameSpeed;

        // Ž�� ����
        ExploreProgress();

        // ������ ��ȭ
        SupportRateMovement();

        // �� �� ����
        if (_timer >= Constants.MONTH_TIMER)
        {
            // �ڱ��� ���� ��
            if (this[VariableLong.Funds] > 0)
            {
                // ������ ȯ�� ����
                EnvironmentAdjust();

                // ��� ����
                this[VariableLong.Funds] -= MonthlyCost();
            }

            // ��ü �α� ���
            this[VariableUint.TotalPopulation] = 0;
            foreach (City city in _data.Cities)
            {
                this[VariableUint.TotalPopulation] += (uint)city.Population;
            }

            // ��¥ ����
            _timer -= Constants.MONTH_TIMER;
            switch (this[VariableByte.Month])
            {
                case 12:
                    {
                        // ����
                        ++this[VariableUshort.Year];
                        this[VariableByte.Month] = 1;

                        // ���� ����
                        AnnualGains();

                        // ���� ȣ��
                        OnYearChange?.Invoke();

                        // �ڵ� ����
                        _autoSave = true;
                    }
                    break;

                default:
                    {
                        // ���� ��
                        ++this[VariableByte.Month];

                        // �¸� ����
                        if (50.0f < this[VariableFloat.BreathLifeStability])
                        {
                            EndGame(true);
                            MessageBox.Instance.EnqueueMessage(Language.Instance[
                                "���� �������� ���� ���� �̻� �����߽��ϴ�. �� ���ֿ� ������ ��ư��� �Ǵٸ� �༺�� ź���� �����Դϴ�. ����� �ӹ��� �Ϸ��߽��ϴ�."
                                ]);
                            GameManager.Instance.EndGameMessage = "����� ������ �༺ �׶������� �ִ� ���θ� �����޾ҽ��ϴ�.";
                        }
                        else if (4 <= this[VariableByte.Conquested])
                        {
                            EndGame(true);
                            MessageBox.Instance.EnqueueMessage(Language.Instance[
                                "��� ������ ����� �ӱ����� ��������ϴ�. ���� �� �༺�� �ֱ��� ����� ���¿��� �ֽ��ϴ�. ����� �ӹ��� �Ϸ��߽��ϴ�."
                                ]);
                            GameManager.Instance.EndGameMessage = "����� ������ �༺�� �����Ͽ� �� �������� �����޾ҽ��ϴ�.";
                        }

                        // �й� ����

                        // �޼��� ����
                        MessageEnqueue();
                    }
                    break;
            }

            // �Ŵ� ȣ��
            OnMonthChange?.Invoke();
        }

        // �� ������ ȣ��
        OnPlayUpdate?.Invoke();

        // ���� ����
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
