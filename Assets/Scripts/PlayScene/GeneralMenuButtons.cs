using Unity.VisualScripting;
using UnityEngine;

public class GeneralMenuButtons : MonoBehaviour
{
    private enum PlayScreenIndex
    {
        Main,
        Explore,
        AirPressure,
        Temperature,
        WaterVolume,
        Carbon,
        Photo,
        Breath,
        LeftEnd,
        City,
        Research,
        Society,
        Diplomacy,
        OwnCulture,
        MediaCulture,
        RightEnd
    }



    /* ==================== Variables ==================== */

    [SerializeField] private PlayScreenBase[] _screens = null;
    [SerializeField] private GameObject _leftButton = null;
    [SerializeField] private GameObject _rightButton = null;
    [SerializeField] private GameObject _leftIndex = null;
    [SerializeField] private GameObject _rightIndex = null;
    [SerializeField] private Transform _selection = null;

    private PlayScreenBase _currentScreen = null;
    private RectTransform _selectionRectTransform = null;
    private byte _currentMenuFocus = 1;

    public static GeneralMenuButtons Instance
    {
        get;
        private set;
    }

    public byte CurrentLeftIndex
    {
        get;
        private set;
    }

    public byte CurrentRightIndex
    {
        get;
        private set;
    }

    public bool IsRightButtonAvailable
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 해당 오브젝트 활성화 여부
    /// </summary>
    public void EnableThis(bool enable)
    {
        gameObject.SetActive(enable);
    }


    /// <summary>
    /// 메뉴 화면 참조 가져오기.
    /// </summary>
    public PlayScreenBase GetScreen()
    {
        switch (_currentMenuFocus)
        {
            case 0:
                return _screens[(int)(PlayScreenIndex.Main + CurrentLeftIndex)];
            case 1:
                return _screens[(int)(PlayScreenIndex.Main)];
            case 2:
                return _screens[(int)(PlayScreenIndex.LeftEnd + CurrentRightIndex)];
            default:
                Debug.LogError("잘못된 메뉴 화면");
                return null;
        }
        
    }


    /// <summary>
    /// 현재 메뉴 화면 변경, 커서 위치 변경
    /// </summary>
    public void SetCurrentScreen(PlayScreenBase current, Transform cursorPosition)
    {
        // 현재 화면 참조 변경
        _currentScreen = current;

        // 커서 위치
        _selection.SetParent(cursorPosition, false);
    }


    /// <summary>
    /// 좌우 버튼 동작
    /// </summary>
    /// <param name="isLeft"></param>
    public void BtnLeftRight(bool isLeft)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 좌우 버튼에 의한 동작
        if (isLeft)
        {
            --_currentMenuFocus;
        }
        else
        {
            ++_currentMenuFocus;
        }

        // 좌우 버튼 활성화 여부
        switch (_currentMenuFocus)
        {
            case 0:
                SetLeftRightButtonsActive(false, true);
                AnimationManager.Instance.SetPlanetImagePosition(PlanetImagePosition.Right);
                _selectionRectTransform.anchorMax = new Vector2(1.2f, 0.75f);
                _selectionRectTransform.anchorMin = new Vector2(-1.2f, 0.25f);
                break;
            case 1:
                SetLeftRightButtonsActive(true, IsRightButtonAvailable);
                AnimationManager.Instance.SetPlanetImagePosition(PlanetImagePosition.Middle);
                break;
            case 2:
                SetLeftRightButtonsActive(true, false);
                AnimationManager.Instance.SetPlanetImagePosition(PlanetImagePosition.Left);
                _selectionRectTransform.anchorMax = new Vector2(2.2f, 0.75f);
                _selectionRectTransform.anchorMin = new Vector2(-0.2f, 0.25f);
                break;
            default:
                Debug.LogError("잘못된 메뉴 화면");
                break;
        }

        // 화면 상태 변경
        _currentScreen.ChangeState();

        // 노이즈 효과 시작
        AnimationManager.Instance.NoiseEffect();
    }


    /// <summary>
    /// 메뉴 목록 동작
    /// </summary>
    public void BtnScreenIndex(int index)
    {
        // 화면 전환 변경 적용. 변경 안 됐으면 바로 함수 종료.
        switch (_currentMenuFocus)
        {
            case 0:
                if (CurrentLeftIndex == index)
                {
                    return;
                }
                CurrentLeftIndex = (byte)index;
                break;
            case 2:
                if (CurrentRightIndex == index)
                {
                    return;
                }
                CurrentRightIndex = (byte)index;
                break;
            default:
                Debug.LogError("잘목된 메뉴 화면");
                return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 상태 변경
        _currentScreen.ChangeState();

        // 노이즈 효과 시작
        AnimationManager.Instance.NoiseEffect();
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 좌우 버튼 활성화 여부
    /// </summary>
    private void SetLeftRightButtonsActive(bool left, bool right)
    {
        _leftButton.SetActive(left);
        _leftIndex.SetActive(!left);
        if (right)
        {
            _rightButton.SetActive(true);
            _rightIndex.SetActive(false);
        }
        else
        {
            _rightButton.SetActive(false);
            _rightIndex.SetActive(IsRightButtonAvailable);
        }
    }


    private void Awake()
    {
        // 유니티 식 싱글턴패턴
        Instance = this;

        // 우측 메뉴 사용 가능 여부
        IsRightButtonAvailable = (0 < PlayManager.Instance[VariableUshort.CityNum]);

        // 처음 시작 시 메뉴 화면은 정해져 있다.
        _currentScreen = _screens[(int)PlayScreenIndex.Main];
        _leftButton.SetActive(true);
        _rightButton.SetActive(IsRightButtonAvailable);
        _leftIndex.SetActive(false);
        _rightIndex.SetActive(false);
        CurrentLeftIndex = 1;
        CurrentRightIndex = 0;

        // 참조
        _selectionRectTransform = _selection.GetComponent<RectTransform>();
    }
}
