using System;
using UnityEngine;

#region JsonData �迭 �ε����� ���� ������
/// <summary>
/// JsonData�� ByteArray �ε��� ������ ���� ������
/// </summary>
public enum VariableByte
{
    AirPressureInfra,
    TemperatureInfra,
    WaterInfra,
    CarbonInfra,
    EndByte
}

/// <summary>
/// JsonData�� ShortArray �ε��� ������ ���� ������
/// </summary>
public enum VariableShort
{
    AirMassMovement,
    TemperatureMovement,
    WaterMovement,
    EndShort
}

/// <summary>
/// JsonData�� IntArray �ε��� ������ ���� ������
/// </summary>
public enum VariableInt
{
    EndInt
}

/// <summary>
/// JsonData�� LongArray �ε��� ������ ���� ������
/// </summary>
public enum VariableLong
{
    Funds,
    EndLong
}

/// <summary>
/// JsonData�� FloatArray �ε��� ������ ���� ������
/// </summary>
public enum VariableFloat
{
    TotalAirPressure_hPa,
    TotalAirMass_Tt,
    EtcAirMass_Tt,
    TotalTemperature_C,
    IncomeEnergy,
    AbsorbEnergy,
    TotalReflection,
    GroundReflection,
    WaterReflection,
    IceReflection,
    CloudReflection,
    WaterGreenHouse_C,
    CarbonGreenHouse_C,
    EtcGreenHouse_C,
    TotalWater_PL,
    WaterGas_PL,
    WaterLiquid_PL,
    WaterSolid_PL,
    CarbonGasMass_Tt,
    CarbonLiquidMass_Tt,
    CarbonSolidMass_Tt,
    GravityAccelation_m_s2,
    PlanetRadius_km,
    PlanetDensity_g_cm3,
    PlanetMass_Tt,
    PlanetDistance_AU,
    PlanetArea_km2,
    EndFloat
}

/// <summary>
/// JsonData�� DoubleArray �ε��� ������ ���� ������
/// </summary>
public enum VariableDouble
{
    EndDouble
}
#endregion

public class PlayManager : MonoBehaviour
{
    public delegate void OnMonthChange();



    /* ==================== Variables ==================== */

    // �ӽ�
    [SerializeField] private GameObject _audioMamagerPrefab = null;

    public static OnMonthChange OMC = null;

    [SerializeField] private float _monthTimer = 2.0f;

    private JsonData _data;
    private float _timer = 0.0f;
    private float _etcAirMassGoal = 0.0f;
    private float _temperatureMovement = 0.0f;
    private float _totalWaterVolumeGoal = 0.0f;
    private float _incomeEnergy_C = 0.0f;
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

    public float GameSpeed
    {
        get;
        set;
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

    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // �ӽ�
        if (null == AudioManager.Instance)
        {
            GameObject audioManager = Instantiate(_audioMamagerPrefab);
            AudioManager.Instance = audioManager.GetComponent<AudioManager>();
            DontDestroyOnLoad(audioManager);
        }
        _data = new JsonData(true);
        this[VariableLong.Funds] = 500000;
        this[VariableFloat.EtcAirMass_Tt] = 5134.58f;
        this[VariableFloat.TotalWater_PL] = 1408718f;
        /*
        �Һ��� ������

        ���ӵ� = (�߷»�� * �༺����) / �����ݰ�^2 * ���ӵ�����
        �༺���� = 4 / 3 * pi * �����ݰ�^3 * �е� * ��������
        ���� = 4 * pi * �����ݰ�^2 * ��������

        �Ի翡���� = �Ÿ�����^2
        �Ÿ����� = 1(AU) / �Ÿ�(AU)
        */
        this[VariableFloat.PlanetRadius_km] = 6378.14f;
        this[VariableFloat.PlanetDensity_g_cm3] = 5.51f;

        this[VariableFloat.PlanetArea_km2] = (float)(4.0d * Math.PI * Math.Pow(this[VariableFloat.PlanetRadius_km], 2.0d) * Constants.PLANET_AREA_ADJUST);

        this[VariableFloat.PlanetMass_Tt] = (float)(4.0d / 3.0d * Math.PI * Math.Pow(this[VariableFloat.PlanetRadius_km], 3.0d) * this[VariableFloat.PlanetDensity_g_cm3] * Constants.PLANET_MASS_ADJUST);

        this[VariableFloat.GravityAccelation_m_s2] = (float)(Constants.GRAVITY_COEFICIENT * this[VariableFloat.PlanetMass_Tt] / Math.Pow(this[VariableFloat.PlanetRadius_km], 2.0d) * Constants.PLANET_GRAVITY_ADJUST);

        this[VariableFloat.IncomeEnergy] = 1.0f;

        // ����Ƽ�� �̱�������
        Instance = this;

        // ���� ��. Update �Լ����� ������ ���̱� ���� �ݺ��Ǵ� ���� ������ �����Ѵ�.
        _incomeEnergy_C = this[VariableFloat.IncomeEnergy] * 240.0f;
        _cloudReflectionMultiply = 0.25d / 12.7d / 0.35d;

        _etcAirMassGoal = this[VariableFloat.EtcAirMass_Tt];
        _temperatureMovement = this[VariableShort.TemperatureMovement];
        _totalWaterVolumeGoal = this[VariableFloat.TotalWater_PL];
    }


    private void Update()
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

        // õ�������� ����� �⺻������ ū �ڷ������� ��ȯ �� ����Ѵ�.

        #region ����
        // ��ü����
        this[VariableFloat.TotalAirMass_Tt] = this[VariableFloat.WaterGas_PL] + this[VariableFloat.CarbonGasMass_Tt] + this[VariableFloat.EtcAirMass_Tt];

        // ����
        this[VariableFloat.TotalAirPressure_hPa] = (float)(Constants.E7 * this[VariableFloat.TotalAirMass_Tt] * this[VariableFloat.GravityAccelation_m_s2] / this[VariableFloat.PlanetArea_km2]);
        #endregion

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
        #endregion

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
            //double1 = 0.75630419551640106261813122516001d * Math.Log10(Constants.MAX_ICE_TEMP_LOG - this[VariableFloat.TotalTemperature_C]);
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
        #endregion

        #region ��� ź�ҷ�
        this[VariableFloat.CarbonGasMass_Tt] = this[VariableFloat.TotalAirPressure_hPa] * 7.1058475203552923760177646188009e-4f;
        #endregion

        // ȯ�� ������ õõ�� ����.
        this[VariableFloat.EtcAirMass_Tt] += (_etcAirMassGoal - this[VariableFloat.EtcAirMass_Tt]) * Time.deltaTime * GameSpeed;
        this[VariableFloat.TotalWater_PL] += (_totalWaterVolumeGoal - this[VariableFloat.TotalWater_PL]) * Time.deltaTime * GameSpeed;
        _temperatureMovement += (this[VariableShort.TemperatureMovement] - _temperatureMovement) * Time.deltaTime * GameSpeed;

        // ���� ���̸� �Լ� ����.
        if (!IsPlaying)
        {
            return;
        }

        // �ð� ���
        _timer += Time.deltaTime * GameSpeed;

        // �� �� ����
        if (_timer >= _monthTimer)
        {
            _timer -= _monthTimer;

            // �ڱ��� ���� ��
            if (this[VariableLong.Funds] > 0)
            {
                #region ������ ȯ�� ����
                // ��� ����
                switch (this[VariableShort.AirMassMovement])
                {
                    case 0:
                        break;
                    default:
                        _etcAirMassGoal += this[VariableShort.AirMassMovement] * Constants.AIRMASS_MOVEMENT;
                        if (0 > this[VariableShort.AirMassMovement])
                        {
                            this[VariableShort.AirMassMovement] = 0;
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
                        if (0 > this[VariableShort.WaterMovement])
                        {
                            this[VariableShort.WaterMovement] = 0;
                        }
                        break;
                }
                #endregion

                // ��� ����
                this[VariableLong.Funds] -= this[VariableShort.AirMassMovement] + this[VariableShort.WaterMovement] + this[VariableShort.TemperatureMovement];
            }
        }
    }



    /* ==================== Struct ==================== */

    private struct JsonData
    {
        public byte[] ByteArray;
        public short[] ShortArray;
        public int[] IntArray;
        public long[] LongArray;
        public float[] FloatArray;
        public double[] DoubleArray;

        public JsonData(bool initialize)
        {
            if (initialize)
            {
                ByteArray = new byte[(int)VariableByte.EndByte];
                ShortArray = new short[(int)VariableShort.EndShort];
                IntArray = new int[(int)VariableInt.EndInt];
                LongArray = new long[(int)VariableLong.EndLong];
                FloatArray = new float[(int)VariableFloat.EndFloat];
                DoubleArray = new double[(int)VariableDouble.EndDouble];
            }
            else
            {
                ByteArray = null;
                ShortArray = null;
                IntArray = null;
                LongArray = null;
                FloatArray = null;
                DoubleArray = null;
            }
    }
    }
}
