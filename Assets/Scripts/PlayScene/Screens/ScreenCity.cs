using UnityEngine;

public class ScreenCity : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _popUpFacilityScreen = null;



    /* ==================== Public Methods ==================== */

    public void BtnFacility()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // â ����
        _popUpFacilityScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */
}
