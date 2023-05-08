using UnityEngine;

#region JsonData 배열 인덱스를 위한 열거형
/// <summary>
/// JsonData의 ByteArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableByte
{

}

/// <summary>
/// JsonData의 ShortArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableShort
{

}

/// <summary>
/// JsonData의 IntArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableInt
{

}

/// <summary>
/// JsonData의 LongArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableLong
{

}

/// <summary>
/// JsonData의 FloatArray 인덱스 접근을 위한 열거형
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
/// JsonData의 DoubleArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableDouble
{
    PlanetArea_km2,
}
#endregion

public class PlayManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    // 임시
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

    #region JsonData의 배열에 접근
    /// <summary>
    /// 편리한 접근을 위해 만들었다.
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
    /// 편리한 접근을 위해 만들었다.
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
    /// 편리한 접근을 위해 만들었다.
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
    /// 편리한 접근을 위해 만들었다.
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
    /// 편리한 접근을 위해 만들었다.
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
    /// 편리한 접근을 위해 만들었다.
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
        // 임시
        if (null == AudioManager.Instance)
        {
            GameObject audioManager = Instantiate(_audioMamagerPrefab);
            AudioManager.Instance = audioManager.GetComponent<AudioManager>();
            DontDestroyOnLoad(audioManager);
        }

        // 유니티식 싱글턴패턴
        Instance = this;
    }


    private void Update()
    {
        /*
        대기압 = 기체질량 * 가속도 / 면적
기체질량 = 수증기 + 탄소 + 기타
가속도 = (중력상수 * 행성질량) / 적도반경^2 * 가속도보정
행성질량 = 4 / 3 * pi * 적도반경^3 * 밀도 * 질량보정
면적 = 4 * pi * 적도반경^2 * 면적보정
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
