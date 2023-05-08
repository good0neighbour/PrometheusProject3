using UnityEngine;

#region JsonData �迭 �ε����� ���� ������
/// <summary>
/// JsonData�� ByteArray �ε��� ������ ���� ������
/// </summary>
public enum VariableByte
{

}

/// <summary>
/// JsonData�� ShortArray �ε��� ������ ���� ������
/// </summary>
public enum VariableShort
{

}

/// <summary>
/// JsonData�� IntArray �ε��� ������ ���� ������
/// </summary>
public enum VariableInt
{

}

/// <summary>
/// JsonData�� LongArray �ε��� ������ ���� ������
/// </summary>
public enum VariableLong
{

}

/// <summary>
/// JsonData�� FloatArray �ε��� ������ ���� ������
/// </summary>
public enum VariableFloat
{
    TotalAirPressure_hPa,
    TotalAirMass_Tt,
    GravityAccelation_m_s2,
    PlanetRadius_km,
    PlanetDensity_g_cm3,
    PlanetMass_Zt,
}

/// <summary>
/// JsonData�� DoubleArray �ε��� ������ ���� ������
/// </summary>
public enum VariableDouble
{
    PlanetArea_km2,
}
#endregion

public class PlayManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    // �ӽ�
    [SerializeField] private GameObject _audioMamagerPrefab = null;

    private JsonData _data;


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

    #region JsonData�� �迭�� ����
    /// <summary>
    /// ���� ������ ���� �������.
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
    /// ���� ������ ���� �������.
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
    /// ���� ������ ���� �������.
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
    /// ���� ������ ���� �������.
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
    /// ���� ������ ���� �������.
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
    /// ���� ������ ���� �������.
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

        // ����Ƽ�� �̱�������
        Instance = this;
    }


    private void Update()
    {
        /*
        ���� = ��ü���� * ���ӵ� / ����
��ü���� = ������ + ź�� + ��Ÿ
���ӵ� = (�߷»�� * �༺����) / �����ݰ�^2 * ���ӵ�����
�༺���� = 4 / 3 * pi * �����ݰ�^3 * �е� * ��������
���� = 4 * pi * �����ݰ�^2 * ��������
        */

    }



    /* ==================== Inner Class ==================== */

    private class JsonData
    {
        public byte[] ByteArray = new byte[64];
        public short[] ShortArray = new short[64];
        public int[] IntArray = new int[64];
        public long[] LongArray = new long[64];
        public float[] FloatArray = new float[64];
        public double[] DoubleArray = new double[64];
    }
}
