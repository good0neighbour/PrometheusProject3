using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenExplore : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("비용")]
    [SerializeField] private ushort _deviceCost = 0;

    [Header("참조")]
    [SerializeField] private Image _progressImage = null;
    [SerializeField] private TMP_Text _addDeviceBtn = null;
    [SerializeField] private TMP_Text _deviceNum = null;
    [SerializeField] private GameObject _landScreen = null;

    private bool _addDeviceAvailable = true;

    // 토지 버튼 때문에 싱글턴패턴이 필요하다.
    public static ScreenExplore Instance
    {
        get;
        private set;
    }

    public SlotLand CurrentSlot
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 토지 버튼 클릭 시 호출한다. 토지 화면의 배경은 반투명하기 때문에 이러한 작업이 필요하다.
    /// </summary>
    public void OpenLandScreen(bool open)
    {
        // 활성화, 비활성화
        _landScreen.SetActive(open);
        gameObject.SetActive(!open);
        GeneralMenuButtons.Instance.EnableThis(!open);

        // 개임 일시정지, 재개
        if (open)
        {
            PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
        }
        else
        {
            PlayManager.Instance.GameResume = Constants.GAME_RESUME;
        }
    }


    public void BtnDeviceAdd()
    {
        // 사용 불가
        if (!_addDeviceAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 비용 지출
        PlayManager.Instance[VariableLong.Funds] -= _deviceCost;

        // 장비 개수 추가
        ++PlayManager.Instance[VariableByte.ExploreDevice];

        // 장비 개수 표시
        _deviceNum.text = PlayManager.Instance[VariableByte.ExploreDevice].ToString();
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        // 장비 개수 표시
        _deviceNum.text = PlayManager.Instance[VariableByte.ExploreDevice].ToString();
    }


    private void Update()
    {
        // 시각 정보 표시
        _progressImage.fillAmount = PlayManager.Instance[VariableFloat.ExploreProgress] / PlayManager.Instance[VariableFloat.ExploreGoal];

        // 장비 추가 가능 여부
        if (_addDeviceAvailable)
        {
            // 건설 비용이 없거나 255개가 됐을 때
            if (_deviceCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.ExploreDevice] >= byte.MaxValue)
            {
                _addDeviceAvailable = false;
                _addDeviceBtn.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
        // 비용이 생겼을 때
        else if (_deviceCost < PlayManager.Instance[VariableLong.Funds])
        {
            _addDeviceAvailable = true;
            _addDeviceBtn.color = Constants.WHITE;
        }

        // 단축키 동작
#if PLATFORM_STANDALONE_WIN
        // 키보드 동작
        if (Input.GetKeyUp(KeyCode.S))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentLeftIndex + 1);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(false);
        }
#endif
        // PC, 모바일 공용
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(false);
        }
    }
}
