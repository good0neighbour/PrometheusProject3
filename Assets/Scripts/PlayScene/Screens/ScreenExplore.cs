using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenExplore : PlayScreenBase
{
    /* ==================== Variables ==================== */

    // ���� ��ư ������ �̱��������� �ʿ��ϴ�.
    public static ScreenExplore Instance = null;

    [Header("���")]
    [SerializeField] private ushort _deviceCost = 0;

    [Header("���� ���")]
    [SerializeField] private Vector2 _anchorMaxCollapsed = new Vector2(0.7f, 0.55f);
    [SerializeField] private Vector2 _anchorMaxExpanded = new Vector2(0.7f, 0.8f);

    [Header("����")]
    [SerializeField] private Image _progressImage = null;
    [SerializeField] private TMP_Text _addDeviceBtn = null;
    [SerializeField] private TMP_Text _deviceNum = null;
    [SerializeField] private TMP_Text _listExpandBtn = null;
    [SerializeField] private RectTransform _list = null;
    [SerializeField] private GameObject _landScreen = null;

    private bool _addDeviceAvailable = true;
    private bool _isExpended = false;

    public LandSlot CurrentSlot
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ���� ��ư Ŭ�� �� ȣ���Ѵ�.
    /// </summary>
    public void OpenLandScreen(bool open)
    {
        // Ȱ��ȭ, ��Ȱ��ȭ
        _landScreen.SetActive(open);
        gameObject.SetActive(!open);
        GeneralMenuButtons.Instance.EnableThis(!open);

        // ���� �Ͻ�����, �簳
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
        // ��� �Ұ�
        if (!_addDeviceAvailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ����
        PlayManager.Instance[VariableLong.Funds] -= _deviceCost;

        // ��� ���� �߰�
        ++PlayManager.Instance[VariableByte.ExploreDevice];

        // ��� ���� ǥ��
        _deviceNum.text = PlayManager.Instance[VariableByte.ExploreDevice].ToString();
    }


    public void BtnListExpand()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��� ���
        if (_isExpended)
        {
            _list.anchorMax = _anchorMaxCollapsed;
            _listExpandBtn.text = Constants.ON_COLLAPSED;
            _isExpended = false;
        }
        // ��� Ȯ��
        else
        {
            _list.anchorMax = _anchorMaxExpanded;
            _listExpandBtn.text = Constants.ON_EXPANDED;
            _isExpended = true;
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // ��� ���� ǥ��
        _deviceNum.text = PlayManager.Instance[VariableByte.ExploreDevice].ToString();
    }


    private void Update()
    {
        // �ð� ���� ǥ��
        _progressImage.fillAmount = PlayManager.Instance[VariableFloat.ExploreProgress] / PlayManager.Instance[VariableFloat.ExploreGoal];

        // ��� �߰� ���� ����
        if (_addDeviceAvailable)
        {
            // �Ǽ� ����� ���ų� 255���� ���� ��
            if (_deviceCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.ExploreDevice] >= byte.MaxValue)
            {
                _addDeviceAvailable = false;
                _addDeviceBtn.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
        // ����� ������ ��
        else if (_deviceCost < PlayManager.Instance[VariableLong.Funds])
        {
            _addDeviceAvailable = true;
            _addDeviceBtn.color = Constants.WHITE;
        }

        // ����Ű ����
#if PLATFORM_STANDALONE_WIN
        // Ű���� ����
        if (Input.GetKeyUp(KeyCode.S))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentLeftIndex + 1);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(false);
        }
#endif
        // PC, ����� ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(false);
        }
    }
}
