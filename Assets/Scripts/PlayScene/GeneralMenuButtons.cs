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
    [SerializeField] private GameObject _selection = null;

    private PlayScreenBase _currentScreen = null;
    private Transform _selectionTransform = null;
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
    public void SetCurrentScreen(PlayScreenBase current, Transform currorPosition)
    {
        // ���� ȭ�� ���� ����
        _currentScreen = current;

        // Ŀ�� ��ġ
        if (null == currorPosition)
        {
            _selection.SetActive(false);
        }
        else
        {
            _selectionTransform.SetParent(currorPosition, false);
            if (!_selection.activeSelf)
            {
                _selection.SetActive(true);
            }
        }
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
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

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
        _rightButton.SetActive(right);
        _rightIndex.SetActive(!right);
    }


    private void Awake()
    {
        // ����Ƽ �� �̱�������
        Instance = this;

        // ó�� ���� �� �޴� ȭ���� ������ �ִ�.
        _currentScreen = _screens[(int)PlayScreenIndex.Main];
        _leftButton.SetActive(true);
        _rightButton.SetActive(true);
        _leftIndex.SetActive(false);
        _rightIndex.SetActive(false);
        CurrentLeftIndex = 1;
        CurrentRightIndex = 1;

        // ����
        _selectionTransform = _selection.transform;
    }
}
