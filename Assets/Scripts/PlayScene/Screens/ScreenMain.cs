using UnityEngine;

public class ScreenMain : PlayScreenBase
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */

    private void Update()
    {
        // 단축키 동작
#if PLATFORM_STANDALONE_WIN
        // 키보드 동작
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
