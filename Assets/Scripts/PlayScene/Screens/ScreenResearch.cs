using UnityEngine;

public class ScreenResearch : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _popUpTechScreen = null;
    [SerializeField] private GameObject _popUpThoughtScreen = null;



    /* ==================== Public Methods ==================== */

    public void BtnTechScreen()
    {
        _popUpTechScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    public void BtnThoughtScreen()
    {
        _popUpThoughtScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */
}
