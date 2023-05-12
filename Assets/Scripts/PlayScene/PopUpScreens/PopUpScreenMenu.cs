using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PopUpScreenMenu : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _mainScreen = null;
    [SerializeField] private GameObject _languagePopUpScreen = null;



    /* ==================== Public Methods ==================== */

    public void BtnLanguage()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 언어 선택 창 열기
        _languagePopUpScreen.SetActive(true);
    }


    public void BtnBack()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 변경
        _mainScreen.SetActive(true);
        gameObject.SetActive(false);
        GeneralMenuButtons.Instance.EnableThis(true);
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;
    }


    public void BtnLanguageSelect(int index)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 언어 선택 창 닫기
        _languagePopUpScreen.SetActive(false);

        // 언어 불러오기
        Language.Instance.LoadLangeage((LanguageType)index);
    }



    /* ==================== Private Methods ==================== */
}
