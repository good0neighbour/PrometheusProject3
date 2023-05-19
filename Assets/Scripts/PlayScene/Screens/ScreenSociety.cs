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
        // 家府 犁积
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 抛农飘府 芒 劝己拳
        _popUpElementTechScreen.ActiveThis(0);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */
}