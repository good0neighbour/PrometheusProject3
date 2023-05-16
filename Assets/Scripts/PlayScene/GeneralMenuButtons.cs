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
    /// �ش� ������Ʈ Ȱ��ȭ ����
    /// </summary>
    public void EnableThis(bool enable)
    {
        gameObject.SetActive(enable);
    }


    /// <summary>
    /// �޴� ȭ�� ���� ��������.
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
                Debug.LogError("�߸��� �޴� ȭ��");
                return null;
        }
        
    }


    /// <summary>
    /// ���� �޴� ȭ�� ����, Ŀ�� ��ġ ����
    /// </summary>
    public void SetCurrentScreen(PlayScreenBase current, Transform cursorPosition)
    {
        // ���� ȭ�� ���� ����
        _currentScreen = current;

        // Ŀ�� ��ġ
        _selection.SetParent(cursorPosition, false);
    }


    /// <summary>
    /// �¿� ��ư ����
    /// </summary>
    /// <param name="isLeft"></param>
    public void BtnLeftRight(bool isLeft)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �¿� ��ư�� ���� ����
        if (isLeft)
        {
            --_currentMenuFocus;
        }
        else
        {
            ++_currentMenuFocus;
        }

        // �¿� ��ư Ȱ��ȭ ����
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
                Debug.LogError("�߸��� �޴� ȭ��");
                break;
        }

        // ȭ�� ���� ����
        _currentScreen.ChangeState();

        // ������ ȿ�� ����
        AnimationManager.Instance.NoiseEffect();
    }


    /// <summary>
    /// �޴� ��� ����
    /// </summary>
    public void BtnScreenIndex(int index)
    {
        // ȭ�� ��ȯ ���� ����. ���� �� ������ �ٷ� �Լ� ����.
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
                Debug.LogError("�߸�� �޴� ȭ��");
                return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ���� ����
        _currentScreen.ChangeState();

        // ������ ȿ�� ����
        AnimationManager.Instance.NoiseEffect();
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// �¿� ��ư Ȱ��ȭ ����
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
        // ����Ƽ �� �̱�������
        Instance = this;

        // ���� �޴� ��� ���� ����
        IsRightButtonAvailable = (0 < PlayManager.Instance[VariableUshort.CityNum]);

        // ó�� ���� �� �޴� ȭ���� ������ �ִ�.
        _currentScreen = _screens[(int)PlayScreenIndex.Main];
        _leftButton.SetActive(true);
        _rightButton.SetActive(IsRightButtonAvailable);
        _leftIndex.SetActive(false);
        _rightIndex.SetActive(false);
        CurrentLeftIndex = 1;
        CurrentRightIndex = 0;

        // ����
        _selectionRectTransform = _selection.GetComponent<RectTransform>();
    }
}
