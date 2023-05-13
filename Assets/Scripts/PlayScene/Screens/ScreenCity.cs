using UnityEngine;

public class ScreenCity : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _popUpFacilityScreen = null;



    /* ==================== Public Methods ==================== */

    public void BtnFacility()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 창 열기
        _popUpFacilityScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */
}
