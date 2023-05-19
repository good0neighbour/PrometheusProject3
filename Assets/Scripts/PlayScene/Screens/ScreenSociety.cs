using UnityEngine;

public class ScreenSociety : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private PopUpScreenElementTechTree _popUpElementTechScreen = null;
    [SerializeField] private PopUpViewSociety _societyView = null;



    /* ==================== Public Methods ==================== */

    public void Activate()
    {
        _societyView.Activate();
    }


    public void BtnSocietyView()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��ũƮ�� â Ȱ��ȭ
        _popUpElementTechScreen.ActiveThis(0);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */
}