/// <summary>
/// UI�� � ������ ǥ���ϴµ�, ���� ������ ���� ȭ�鿡 ǥ���� ���, ���� ���� ���ڿ��� �����ϴ� �ͺ��� �� ���ڿ��� �����ϴ� ���� �� ���� ������ �Ǵ��Ͽ� ���� Ŭ����.
/// </summary>
public class UIString
{
    /* ==================== Variables ==================== */

    private static UIString _instance = null;

    public static UIString Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIString();
            }

            return _instance;
        }
    }

    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */
}
