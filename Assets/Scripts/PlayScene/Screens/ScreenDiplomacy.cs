using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenDiplomacy : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private byte _trueCost = 10;
    [SerializeField] private TMP_Text _forceNameText = null;
    [SerializeField] private TMP_Text _trueCostText = null;
    [SerializeField] private TMP_Text _trueBtnText = null;
    [SerializeField] private Image _friendlyImage = null;
    [SerializeField] private Image _hostileImage = null;
    [SerializeField] private Image _conquestImage = null;
    [SerializeField] private GameObject _true = null;
    [SerializeField] private GameObject _false = null;
    [SerializeField] private GameObject _popUpDiplomacy = null;
    [SerializeField] private GameObject _popUpTrade = null;
    [SerializeField] private GameObject _popUpConquest = null;

    private byte _current = 0;
    private bool _isTrueBtnAvailable = false;

    public static Force CurrentForce
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
        _forceNameText.text = Language.Instance[CurrentForce.ForceName];

        // 표시할 화면
        bool isTrue = CurrentForce.Info;
        _true.SetActive(isTrue);
        _false.SetActive(!isTrue);
    }


    public void BtnTrue()
    {
        // 사용 불가
        if (!_isTrueBtnAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 비용 지출
        PlayManager.Instance[VariableLong.Funds] -= _trueCost;
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Fund);

        // 화면 전환
        CurrentForce.Info = true;
        _true.SetActive(true);
        _false.SetActive(false);
    }


    public void BtnDiplomacy()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 현재 창 닫는다.
        CloseScreen();

        //화면 전환
        _popUpDiplomacy.SetActive(true);
    }


    public void BtnTrade()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 현재 창 닫는다.
        CloseScreen();

        //화면 전환
        _popUpTrade.SetActive(true);
    }


    public void BtnConquest()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

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
        gameObject.SetActive(false);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    private void OnLanguageChange()
    {
        _forceNameText.text = Language.Instance[CurrentForce.ForceName];
    }


    private void Awake()
    {
        CurrentForce = PlayManager.Instance.GetForce(_current);

        // 비용 표시
        _trueCostText.text = _trueCost.ToString();

        // 표시할 화면
        bool isTrue = CurrentForce.Info;
        _true.SetActive(isTrue);
        _false.SetActive(!isTrue);

        // 텍스트 한 번 업데이트
        OnLanguageChange();

        // 대리자 등록
        Language.OnLanguageChange += OnLanguageChange;
    }


    private void Update()
    {
        _friendlyImage.fillAmount = CurrentForce.Friendly;
        _hostileImage.fillAmount = CurrentForce.Hostile;
        _conquestImage.fillAmount = CurrentForce.Conquest;

        // 요청 가능 여부
        if (_isTrueBtnAvailable)
        {
            // 요청 비용이 없됐을 때
            if (_trueCost > PlayManager.Instance[VariableLong.Funds])
            {
                _isTrueBtnAvailable = false;
                _trueBtnText.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
        // 요청 비용이 생겼을 때
        else if (_trueCost <= PlayManager.Instance[VariableLong.Funds])
        {
            _isTrueBtnAvailable = true;
            _trueBtnText.color = Constants.WHITE;
        }

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
