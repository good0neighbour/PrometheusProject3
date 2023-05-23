using UnityEngine;

public class ScreenMain : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("참조")]
    [SerializeField] private GameObject _popUpMenuScreen = null;
    [SerializeField] private GameObject _popMessageLogScreen = null;

    private bool _isMessageLogOpened = false;



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
            _popMessageLogScreen.SetActive(true);
            gameObject.SetActive(false);
            GeneralMenuButtons.Instance.EnableThis(false);
            PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
        }
        else
        {
            _popMessageLogScreen.SetActive(false);
            gameObject.SetActive(true);
            GeneralMenuButtons.Instance.EnableThis(true);
            PlayManager.Instance.GameResume = Constants.GAME_RESUME;
        }

        _isMessageLogOpened = openPopUpScreen;
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        // 단축키 동작
        // 메세지 로그 열렸을 때
        if (_isMessageLogOpened)
        {
            // 공통 동작
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                BtnMessageLog(false);
            }
        }
        // 중앙 화면일 때
        else
        {
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
            // 공통 동작
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                BtnPopUpMenu();
            }
        }
    }
}
