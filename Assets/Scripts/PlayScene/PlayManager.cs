using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    // 임시
    [SerializeField] private GameObject _audioMamagerPrefab = null;


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

    #region JsonData 프로퍼티

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



    /* ==================== Inner Class ==================== */

    private class JsonData
    {
        public byte[] ByteArray = new byte[32];
        public short[] ShortArray = new short[32];
        public int[] IntArray = new int[32];
        public long[] LongArray = new long[32];
        public float[] FloatArray = new float[32];
        public double[] DoubleArray = new double[32];
    }
}
