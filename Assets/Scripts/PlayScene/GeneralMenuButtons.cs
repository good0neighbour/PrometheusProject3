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
    /// �޴� ȭ�� ���� ��������.
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
                Debug.LogError("�߸��� �޴� ȭ��");
                return null;
        }
        
    }


    /// <summary>
    /// ���� �޴� ȭ�� ����
    /// </summary>
    public void SetCurrentScreen(PlayScreenBase current)
    {
        // ���� ȭ�� ���� ����
        _currentScreen = current;
    }


    /// <summary>
    /// �¿� ��ư ����
    /// </summary>
    /// <param name="isLeft"></param>
    public void BtnLeftRight(bool isLeft)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido();

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
        AudioManager.Instance.PlayAuido();

        // ȭ�� ��ȯ ���� ����. ���� �� ������ �ٷ� �Լ� ����.
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
    }
}
