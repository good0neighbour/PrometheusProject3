using UnityEngine;

public class LandSlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 토지 화면 활성화
        ScreenExplore.Instance.OpenLandScreen(true);
    }



    /* ==================== Private Methods ==================== */
}
