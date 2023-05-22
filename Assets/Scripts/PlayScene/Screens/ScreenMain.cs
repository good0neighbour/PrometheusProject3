using UnityEngine;

public class ScreenMain : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("참조")]
    [SerializeField] private GameObject _popUpMenuScreen = null;
    [SerializeField] private GameObject _popMessageLogScreen = null;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 메뉴 화면 열기
    /// </summary>
    public void BtnPopUpMenu()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 변경
        _popUpMenuScreen.SetActive(true);
        gameObject.SetActive(false);
        GeneralMenuButtons.Instance.EnableThis(false);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    public void BtnMessageLog(bool openPopUpScreen)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 변경
        if (openPopUpScreen)
        {
            _popUpMenuScreen.SetActive(true);
            gameObject.SetActive(false);
            GeneralMenuButtons.Instance.EnableThis(false);
            PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
        }
        else
        {
            _popUpMenuScreen.SetActive(false);
            gameObject.SetActive(true);
            GeneralMenuButtons.Instance.EnableThis(true);
            PlayManager.Instance.GameResume = Constants.GAME_RESUME;
        }
    }



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
