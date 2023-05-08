/// <summary>
/// UI에 어떤 정보를 표시하는데, 같은 정보를 여러 화면에 표시할 경우, 각각 따로 문자열을 생성하는 것보다 한 문자열을 참조하는 것이 더 나을 것으로 판단하여 만든 클래스.
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
