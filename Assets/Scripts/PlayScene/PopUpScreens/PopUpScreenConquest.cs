using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScreenConquest : MonoBehaviour, IPopUpScreen
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
    [SerializeField] private TMP_Text _conquestText = null;
    [SerializeField] private Image _friendlyImage = null;
    [SerializeField] private Image _hostileImage = null;
    [SerializeField] private Image _conquestImage = null;
    [SerializeField] private Image _progressionImage = null;
    [SerializeField] private GameObject _previousScreen = null;
    [SerializeField] private Transform _cursor = null;

    private TMP_Text[] _slotTexts = null;
    private byte _currentBtn = 255;
    private byte _currentCategory = 0;
    private byte _currentSlot = 0;
    private byte _slotLength = 0;
    private float _supportRate = 0.0f;
    private float _timer = 0.0f;
    private bool _isAdoptAvailable = false;
    private bool _isBackBtnAvailable = true;
    private bool _adoptAnimationProceed = false;
    private bool _connectionAnimationProceed = false;

    public static PopUpScreenConquest Instance
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
            SetAdoptAvailable(_buttons[_currentBtn].IsAvailable && ScreenDiplomacy.CurrentForce.IsDiplomacySlotAvailable);
        }

        _statusText.text = null;
    }


    public void BtnCategory(int index)
    {
        // �̹� ���� ī�װ����� ���
        if (_currentCategory == index)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ī�װ��� ��ȯ
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
            if (string.IsNullOrEmpty(ScreenDiplomacy.CurrentForce.ConquestSlotText(i)))
            {
                break;
            }
        }
        _slotTexts[i].text = name;
        _slotTexts[i].color = Constants.WHITE;
        _slotImages[i].color = Constants.SLOT_ENABLED;
        ScreenDiplomacy.CurrentForce.ConquestSlotText(i, name);

        index = i;
        force = ScreenDiplomacy.CurrentForce;
        _currentSlot = i;
        _connectionAnimationProceed = true;
        if (i == _slotLength - 1)
        {
            ScreenDiplomacy.CurrentForce.IsConquestSlotAvailable = true;
        }
    }


    /// <summary>
    /// ���� ����.
    /// </summary>
    public void EmptySlot(Force force, byte index)
    {
        force.ConquestSlotText(index, null);
        force.IsConquestSlotAvailable = true;
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
        _timer += Time.deltaTime;
        _progressionImage.fillAmount = _timer;
        if (1.0f <= _timer)
        {
            if (_supportRate >= Random.Range(0.0f, Constants.MAX_SUPPORT_RATE_ADOPTION))
            {
                _buttons[_currentBtn].BtnAdopt();

                _adoptBtnText.text = Language.Instance["��� ��"];
                SetAdoptAvailable(false);
            }
            else
            {
                // ���� ����
                _statusText.text = Language.Instance["��å ����"];
                _statusText.color = Constants.FAIL_TEXT;

                // ��� Ȯ�� �� ���� ��ư Ȱ��ȭ
                SetAdoptAvailable(_buttons[_currentBtn].IsAvailable);
            }

            // �ӱ�ȭ �׷��� ������Ʈ
            ConquestImageUpdate();

            // �ڷΰ��� ����
            _isBackBtnAvailable = true;
            _backBtnText.color = Constants.WHITE;

            // �ִϸ��̼� ��
            _adoptAnimationProceed = false;
            _progressionImage.fillAmount = 0.0f;
            _timer = 0.0f;
        }
    }


    /// <summary>
    /// ���� ���� �ִϸ��̼�
    /// </summary>
    private void ConnectionAnimantionProceed()
    {
        _timer += Time.deltaTime;
        _connectionImages[_currentSlot].fillAmount = _timer;
        if (1.0f <= _timer)
        {
            _connectionAnimationProceed = false;
            _timer = 0.0f;
        }
    }


    /// <summary>
    /// �ӱ�ȭ �׷��� ������Ʈ
    /// </summary>
    private void ConquestImageUpdate()
    {
        _conquestText.text = $"{(ScreenDiplomacy.CurrentForce.Conquest * 100.0f).ToString("0")}%";
        _conquestImage.fillAmount = ScreenDiplomacy.CurrentForce.Conquest;
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
        _friendlyImage.fillAmount = ScreenDiplomacy.CurrentForce.Friendly;
        _hostileImage.fillAmount = ScreenDiplomacy.CurrentForce.Hostile;
        ConquestImageUpdate();

        // ����
        for (byte i = 0; i < 5; ++i)
        {
            if (string.IsNullOrEmpty(ScreenDiplomacy.CurrentForce.ConquestSlotText(i)))
            {
                _slotImages[i].color = Constants.SLOT_DISABLED;
                _slotTexts[i].text = Language.Instance["��� ����"];
                _slotTexts[i].color = Constants.TEXT_BUTTON_DISABLE;
                _connectionImages[i].fillAmount = 0.0f;
            }
            else
            {
                _slotImages[i].color = Constants.TEXT_BUTTON_DISABLE;
                _slotTexts[i].text = Language.Instance[ScreenDiplomacy.CurrentForce.ConquestSlotText(i)];
                _slotTexts[i].color = Constants.WHITE;
                _connectionImages[i].fillAmount = 1.0f;
            }
        }

        // ó�� ���·�
        _adoptBtnText.text = Language.Instance["����"];
        SetAdoptAvailable(false);
        _dexcriptionText.text = null;
        _statusText.text = null;
    }


    private void Update()
    {
        if (_adoptAnimationProceed)
        {
            AdoptAnimationProceed();
        }
        else if (_connectionAnimationProceed)
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