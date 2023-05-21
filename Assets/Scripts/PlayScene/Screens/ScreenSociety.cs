using UnityEngine;
using UnityEngine.UI;

public class ScreenSociety : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private PopUpScreenElementTechTree _popUpElementTechScreen = null;
    [SerializeField] private PopUpViewSociety _societyView = null;
    [SerializeField] private Image _societyBtnImage = null;
    [SerializeField] private Sprite[] _societySprites = null;



    /* ==================== Public Methods ==================== */

    public void Activate()
    {
        _societyView.Activate();
        SocietyImageUpdate();
    }


    public void BtnSocietyView()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��ũƮ�� â Ȱ��ȭ
        _popUpElementTechScreen.ActiveThis(0);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    /// <summary>
    /// ��ȸ �̹��� ������Ʈ
    /// </summary>
    public void SocietyImageUpdate()
    {
        _societyBtnImage.sprite = _societySprites[PlayManager.Instance[VariableByte.Era] - 1];
    }



    /* ==================== Private Methods ==================== */
}