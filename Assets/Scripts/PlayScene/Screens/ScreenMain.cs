using UnityEngine;

public class ScreenMain : PlayScreenBase
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */

    private void Update()
    {
        // ����Ű ����
#if PLATFORM_STANDALONE_WIN
        // Ű���� ����
        if (Input.GetKeyUp(KeyCode.A))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(false);
        }
#endif
    }
}
