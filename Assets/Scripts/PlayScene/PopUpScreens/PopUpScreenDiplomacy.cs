using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScreenDiplomacy : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private CoolTimeBtnDiplomacySemiBase[] _buttons = null;
    [SerializeField] private GameObject[] _categories = null;
    [SerializeField] private Transform[] _categoryBtnAreas = null;
    [SerializeField] private Image[] _slotImages = null;
    [SerializeField] private Image[] _connectionImages = null;
    [SerializeField] private TMP_Text _forceNameText = null;
    [SerializeField] private TMP_Text _dexcriptionText = null;
    [SerializeField] private TMP_Text _adoptBtnText = null;
    [SerializeField] private TMP_Text _statusText = null;
    [SerializeField] private TMP_Text _backBtnText = null;
    [SerializeField] private TMP_Text _friendlyText = null;
    [SerializeField] private Image _playerSoftpowerImage = null;
    [SerializeField] private Image _forceFriendlyImage = null;
    [SerializeField] private Image _forceHostileImage = null;
    [SerializeField] private Image _progressionImage = null;
    [SerializeField] private GameObject _previousScreen = null;
    [SerializeField] private Transform _cursor = null;

    private TMP_Text[] _slotTexts = null;
    private byte _currentBtn = 255;
    private byte _currentCategory = 0;
    private byte _currentSlot = 0;
    private byte _slotLength = 0;
    private float _supportRate = 0.0f;
    private float _adoptTimer = 0.0f;
    private float _connectionTimer = 0.0f;
    private bool _isAdoptAvailable = false;
    private bool _isBackBtnAvailable = true;
    private bool _adoptAnimationProceed = false;
    private bool _connectionAnimationProceed = false;

    public static PopUpScreenDiplomacy Instance
    {
        get;
        private set;
    }

    public float PlayerSoftPower
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        // ��� �Ұ�
        if (!_isBackBtnAvailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ��ȯ
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;
    }


    public void BtnAdopt()
    {
        // ��� �Ұ�
        if (!_isAdoptAvailable)
        {
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ��ư ��� �Ұ�
        _isAdoptAvailable = false;

        // �ڷΰ��� ����
        _isBackBtnAvailable = false;
        _backBtnText.color = Constants.TEXT_BUTTON_DISABLE;

        // �ִϸ��̼� ����
        AdoptAnimation(PlayManager.Instance[VariableFloat.DiplomacySupportRate]);
    }


    public void BtnTouch(int index)
    {
        // ��� �Ұ�
        if (_adoptAnimationProceed)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        _currentBtn = (byte)index;
        _dexcriptionText.text = _buttons[_currentBtn].GetDescription();

        if (_buttons[_currentBtn].IsCoolTimeRunning)
        {
            _adoptBtnText.text = Language.Instance["��� ��"];
            SetAdoptAvailable(false);
        }
        else
        {
            _adoptBtnText.text = Language.Instance["����"];
            SetAdoptAvailable(_buttons[_currentBtn].IsAvailable && ScreenDiplomacy.CurrentForce.IsDiplomacySlotAvailable());
        }

        _statusText.text = null;
    }


    public void BtnCategory(int index)
    {
        // �̹� ���� ī�װ��� ���
        if (_currentCategory == index)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ī�װ� ��ȯ
        _categories[_currentCategory].SetActive(false);
        _currentCategory = (byte)index;
        _categories[_currentCategory].SetActive(true);

        // Ŀ�� �̵�
        _cursor.SetParent(_categoryBtnAreas[index], false);
    }


    /// <summary>
    /// ���Կ� �̸� �߰�
    /// </summary>
    public void FillSlot(string name, out Force force, out byte index)
    {
        // �� ���� ã�´�.
        byte i;
        for (i = 0; i < _slotLength; ++i)
        {
            if (string.IsNullOrEmpty(ScreenDiplomacy.CurrentForce.DiplomacySlots[i]))
            {
                break;
            }
        }
        _slotTexts[i].text = Language.Instance[name];
        _slotTexts[i].color = Constants.WHITE;
        _slotImages[i].color = Constants.SLOT_ENABLED;
        ScreenDiplomacy.CurrentForce.DiplomacySlots[i] = name;

        index = i;
        force = ScreenDiplomacy.CurrentForce;
        _currentSlot = i;
        _connectionAnimationProceed = true;
        ++ScreenDiplomacy.CurrentForce.DiplomacySlotUsage;
    }


    /// <summary>
    /// ���� ����.
    /// </summary>
    public void EmptySlot(Force force, byte index)
    {
        force.DiplomacySlots[index] = null;
        --force.DiplomacySlotUsage;
        _connectionImages[index].fillAmount = 0.0f;
    }


    /// <summary>
    /// ���� �ؽ�Ʈ ������Ʈ
    /// </summary>
    public void SetStatusText(string text, Color colour)
    {
        _statusText.text = text;
        _statusText.color = colour;
    }



    /* ==================== Private Methods ==================== */

    private void AdoptAnimation(float supportRate)
    {
        _adoptAnimationProceed = true;
        _supportRate = supportRate;
    }


    private void SetAdoptAvailable(bool available)
    {
        _isAdoptAvailable = available;
        if (_isAdoptAvailable)
        {
            _adoptBtnText.color = Constants.WHITE;
        }
        else
        {
            _adoptBtnText.color = Constants.TEXT_BUTTON_DISABLE;
        }
    }


    /// <summary>
    /// ���� �ִϸ��̼�
    /// </summary>
    private void AdoptAnimationProceed()
    {
        _adoptTimer += Time.deltaTime;
        _progressionImage.fillAmount = _adoptTimer;
        if (1.0f <= _adoptTimer)
        {
            if (_supportRate >= Random.Range(0.0f, Constants.MAX_SUPPORT_RATE_ADOPTION))
            {
                // ���� ���� �� ����
                _buttons[_currentBtn].BtnAdopt();

                // ������ ���
                PlayManager.Instance[VariableFloat.DiplomacySupportRate] += Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
                if (100.0f < PlayManager.Instance[VariableFloat.DiplomacySupportRate])
                {
                    PlayManager.Instance[VariableFloat.DiplomacySupportRate] = 100.0f;
                }

                // ������ ��� �̳����̼�
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.DiplomacySupport);

                _adoptBtnText.text = Language.Instance["��� ��"];
                SetAdoptAvailable(false);
            }
            else
            {
                //�Ҹ� ���
                AudioManager.Instance.PlayAuido(AudioType.Failed);

                // ������ ����
                PlayManager.Instance[VariableFloat.DiplomacySupportRate] -= Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
                if (0.0f > PlayManager.Instance[VariableFloat.DiplomacySupportRate])
                {
                    PlayManager.Instance[VariableFloat.DiplomacySupportRate] = 0.0f;
                }

                // ������ ���� �̳����̼�
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.DiplomacySupport);

                // ���� ����
                _statusText.text = Language.Instance["��å ����"];
                _statusText.color = Constants.FAIL_TEXT;

                // ��� Ȯ�� �� ���� ��ư Ȱ��ȭ
                SetAdoptAvailable(_buttons[_currentBtn].IsAvailable);
            }

            // ��ȣ�� �׷��� ������Ʈ
            FriendlyImageUpdate();

            // �ڷΰ��� ����
            _isBackBtnAvailable = true;
            _backBtnText.color = Constants.WHITE;

            // �ִϸ��̼� ��
            _adoptAnimationProceed = false;
            _progressionImage.fillAmount = 0.0f;
            _adoptTimer = 0.0f;
        }
    }


    /// <summary>
    /// ���� ���� �ִϸ��̼�
    /// </summary>
    private void ConnectionAnimantionProceed()
    {
        _connectionTimer += Time.deltaTime;
        _connectionImages[_currentSlot].fillAmount = _connectionTimer;
        if (1.0f <= _connectionTimer)
        {
            _connectionAnimationProceed = false;
            _connectionTimer = 0.0f;
        }
    }


    /// <summary>
    /// ��ȣ�� �׷��� ������Ʈ
    /// </summary>
    private void FriendlyImageUpdate()
    {
        _friendlyText.text = $"{(ScreenDiplomacy.CurrentForce.Friendly * 100.0f).ToString("0")}%";
        _forceFriendlyImage.fillAmount = ScreenDiplomacy.CurrentForce.Friendly;
        _forceHostileImage.fillAmount = ScreenDiplomacy.CurrentForce.Hostile;
    }


    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // ����
        _slotLength = (byte)_slotImages.Length;
        _slotTexts = new TMP_Text[_slotLength];
        for (byte i = 0; i < _slotLength; ++i)
        {
            _slotTexts[i] = _slotImages[i].GetComponentInChildren<TMP_Text>();
            _slotTexts[i].text = Language.Instance["��� ����"];
        }
    }


    private void OnEnable()
    {
        // ���� ����
        _forceNameText.text = ScreenDiplomacy.CurrentForce.ForceName;
        PlayerSoftPower = (float)PlayManager.Instance[VariableUint.Culture] / (PlayManager.Instance[VariableUint.Culture] + ScreenDiplomacy.CurrentForce.Culture);
        _playerSoftpowerImage.fillAmount = PlayerSoftPower;
        FriendlyImageUpdate();

        // ����
        for (byte i = 0; i < 5; ++i)
        {
            if (string.IsNullOrEmpty(ScreenDiplomacy.CurrentForce.DiplomacySlots[i]))
            {
                _slotImages[i].color = Constants.SLOT_DISABLED;
                _slotTexts[i].text = Language.Instance["��� ����"];
                _slotTexts[i].color = Constants.TEXT_BUTTON_DISABLE;
                _connectionImages[i].fillAmount = 0.0f;
            }
            else
            {
                _slotImages[i].color = Constants.SLOT_ENABLED;
                _slotTexts[i].text = Language.Instance[ScreenDiplomacy.CurrentForce.DiplomacySlots[i]];
                _slotTexts[i].color = Constants.WHITE;
                _connectionImages[i].fillAmount = 1.0f;
            }
        }

        // ó�� ���·�
        _adoptBtnText.text = Language.Instance["����"];
        SetAdoptAvailable(false);
        _dexcriptionText.text = null;
        _statusText.text = null;
        _connectionAnimationProceed = false;
        _connectionTimer = 0.0f;
    }


    private void Update()
    {
        if (_adoptAnimationProceed)
        {
            AdoptAnimationProceed();
        }

        if (_connectionAnimationProceed)
        {
            ConnectionAnimantionProceed();
        }

        // ����Ű ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BtnBack();
        }
    }
}
