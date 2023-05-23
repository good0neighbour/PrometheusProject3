using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenDiplomacy : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _forceNameText = null;
    [SerializeField] private Image _friendlyImage = null;
    [SerializeField] private Image _hostileImage = null;
    [SerializeField] private Image _conquestImage = null;
    [SerializeField] private GameObject _true = null;
    [SerializeField] private GameObject _false = null;
    [SerializeField] private GameObject _popUpDiplomacy = null;
    [SerializeField] private GameObject _popUpTrade = null;
    [SerializeField] private GameObject _popUpConquest = null;

    private byte _current = 0;

    public Force CurrentForce
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnForceSelect(int index)
    {
        // 이미 같은 버튼을 눌렀다.
        if (_current == index)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 세력 변경
        _current = (byte)index;
        CurrentForce = PlayManager.Instance.GetForce(_current);
        _forceNameText.text = CurrentForce.ForceName;

        // 표시할 화면
        bool isTrue = CurrentForce.Info;
        _true.SetActive(isTrue);
        _false.SetActive(!isTrue);
    }


    public void BtnTrue()
    {
        CurrentForce.Info = true;
        _true.SetActive(true);
        _false.SetActive(false);
    }


    public void BtnDiplomacy()
    {
        // 현재 창 닫는다.
        CloseScreen();

        //화면 전환
        _popUpDiplomacy.SetActive(true);
    }


    public void BtnTrade()
    {
        // 현재 창 닫는다.
        CloseScreen();

        //화면 전환
        _popUpTrade.SetActive(true);
    }


    public void BtnConquest()
    {
        // 현재 창 닫는다.
        CloseScreen();

        //화면 전환
        _popUpConquest.SetActive(true);
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 현재 창 닫는다.
    /// </summary>
    private void CloseScreen()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 닫는다.
        gameObject.SetActive(false);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    private void Awake()
    {
        CurrentForce = PlayManager.Instance.GetForce(_current);
    }


    private void Update()
    {
        _friendlyImage.fillAmount = CurrentForce.Friendly;
        _hostileImage.fillAmount = CurrentForce.Hostile;
        _conquestImage.fillAmount = CurrentForce.Conquest;

        // 단축키 동작
#if PLATFORM_STANDALONE_WIN
        // 키보드 동작
        if (Input.GetKeyUp(KeyCode.W))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex - 1);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex + 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
#endif
        // PC, 모바일 공용
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
    }
}
