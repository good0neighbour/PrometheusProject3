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



    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */
}