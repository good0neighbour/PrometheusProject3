using UnityEngine;
using UnityEngine.EventSystems;

public class GeneralMenuButtons : MonoBehaviour
{
    private enum PlayScreenIndex
    {
        Main,
        AirPressure,
        Temperature,
        WaterVolume,
        Carbon,
        Photo,
        Breath,
        LeftEnd,
        RightEnd
    }



    /* ==================== Variables ==================== */

    [SerializeField] private PlayScreenBase[] _screens = null;
    [SerializeField] private GameObject _leftButton = null;
    [SerializeField] private GameObject _rightButton = null;
    [SerializeField] private GameObject _leftIndex = null;
    [SerializeField] private GameObject _rightIndex = null;
    [SerializeField] private Transform _leftSelection = null;
    [SerializeField] private Transform _rightSelection = null;

    private PlayScreenBase _currentScreen = null;
    private byte _currentMenuFocus = 1;
    private byte _currentLeftIndex = 1;
    private byte _currentRightIndex = 1;

    public static GeneralMenuButtons Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 메뉴 화면 참조 가져오기.
    /// </summary>
    public PlayScreenBase GetScreen()
    {
        switch (_currentMenuFocus)
        {
            case 0:
                return _screens[(int)(PlayScreenIndex.Main + _currentLeftIndex)];
            case 1:
                return _screens[(int)(PlayScreenIndex.Main)];
            case 2:
                return _screens[(int)(PlayScreenIndex.LeftEnd + _currentRightIndex)];
            default:
                Debug.LogError("잘못된 메뉴 화면");
                return null;
        }
        
    }


    /// <summary>
    /// 현재 메뉴 화면 변경
    /// </summary>
    public void SetCurrentScreen(PlayScreenBase current)
    {
        // 현재 화면 참조 변경
        _currentScreen = current;
    }


    /// <summary>
    /// 좌우 버튼 동작
    /// </summary>
    /// <param name="isLeft"></param>
    public void BtnLeftRight(bool isLeft)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido();

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
                break;
            case 1:
                SetLeftRightButtonsActive(true, true);
                AnimationManager.Instance.SetPlanetImagePosition(PlanetImagePosition.Middle);
                break;
            case 2:
                SetLeftRightButtonsActive(true, false);
                AnimationManager.Instance.SetPlanetImagePosition(PlanetImagePosition.Left);
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
        // 소리 재생
        AudioManager.Instance.PlayAuido();

        // 화면 전환 변경 적용. 변경 안 됐으면 바로 함수 종료.
        switch (_currentMenuFocus)
        {
            case 0:
                if (_currentLeftIndex == index)
                {
                    return;
                }
                _currentLeftIndex = (byte)index;
                _leftSelection.SetParent(EventSystem.current.currentSelectedGameObject.transform, false);
                break;
            case 2:
                if (_currentRightIndex == index)
                {
                    return;
                }
                _currentRightIndex = (byte)index;
                _rightSelection.SetParent(EventSystem.current.currentSelectedGameObject.transform, false);
                break;
            default:
                Debug.LogError("잘목된 메뉴 화면");
                return;
        }

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
        _rightButton.SetActive(right);
        _rightIndex.SetActive(!right);
    }


    private void Awake()
    {
        // 유니티 식 싱글턴패턴
        Instance = this;

        // 처음 시작 시 메뉴 화면은 정해져 있다.
        _currentScreen = _screens[(int)PlayScreenIndex.Main];
        _leftButton.SetActive(true);
        _rightButton.SetActive(true);
        _leftIndex.SetActive(false);
        _rightIndex.SetActive(false);
    }
}
