/// <summary>
/// 변경이 있을 때 호출할 대리자.
/// </summary>
public delegate void OnChangeDelegate();

/// <summary>
/// 전체적으로 프로그램을 관리
/// </summary>
public class GameManager
{
    /* ==================== Variables ==================== */

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }

    public LanguageType CurrentLanguage
    {
        get;
        set;
    }

    public bool IsNewGame
    {
        get;
        set;
    }

    public float AirMass
    {
        get;
        set;
    }

    public float WaterVolume
    {
        get;
        set;
    }

    public float CarbonRatio
    {
        get;
        set;
    }

    public float Radius
    {
        get;
        set;
    }

    public float Density
    {
        get;
        set;
    }

    public float Distance
    {
        get;
        set;
    }




    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */
}
