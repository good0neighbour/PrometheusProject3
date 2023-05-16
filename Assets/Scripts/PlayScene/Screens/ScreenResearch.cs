using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenResearch : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("ÂüÁ¶")]
    [SerializeField] private PopUpScreenTechTree _popUpTechScreen = null;
    [SerializeField] private TMP_Text _techResearchTitleText = null;
    [SerializeField] private TMP_Text _techResearchRemainText = null;
    [SerializeField] private Image _techResearchProgreesionImage = null;



    /* ==================== Public Methods ==================== */

    public void BtnTechScreen()
    {
        _popUpTechScreen.ActiveThis(TechTreeType.Tech);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    public void BtnThoughtScreen()
    {
        _popUpTechScreen.ActiveThis(TechTreeType.Thought);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */
}
