/// <summary>
/// ������ ���� �� ȣ���� �븮��.
/// </summary>
public delegate void OnChangeDelegate();

/// <summary>
/// ��ü������ ���α׷��� ����
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
