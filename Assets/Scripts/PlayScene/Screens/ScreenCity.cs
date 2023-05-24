using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenCity : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("����")]
    [SerializeField] private TMP_Text _cityNameText = null;
    [SerializeField] private TMP_Text _capacityText = null;
    [SerializeField] private TMP_Text _populationText = null;
    [SerializeField] private TMP_Text _populationMovementText = null;
    [SerializeField] private TMP_Text _annualFundText = null;
    [SerializeField] private TMP_Text _annualResearchText = null;
    [SerializeField] private TMP_Text _crimeText = null;
    [SerializeField] private TMP_Text _diseaseText = null;
    [SerializeField] private TMP_Text _injureText = null;
    [SerializeField] private TMP_Text _stabilityText = null;
    [SerializeField] private Image _cityImage = null;
    [SerializeField] private Image _supportRateImage = null;
    [SerializeField] private PopUpScreenTechTree _popUpTechTreeScreen = null;
    [SerializeField] private Sprite[] _citySprites = new Sprite[4];

    private City _currentCity = null;

    public static ScreenCity Instance
    {
        get;
        private set;
    }

    public SlotCity CurrentSlot
    {
        get;
        set;
    }

    public City CurrentCity
    {
        get
        {
            return _currentCity;
        }
        set
        {
            // ���� ����
            _currentCity = value;

            // ���� ���� �� ����
            _cityNameText.text = value.CityName;
            _capacityText.text = _currentCity.Capacity.ToString();
            CityImageUpdate();
            OnMonthChange();
        }
    }



    /* ==================== Public Methods ==================== */

    public void BtnFacility()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // â ����.
        _popUpTechTreeScreen.ActiveThis(TechTreeType.Facility);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;

        // �� â �ݴ´�.
        gameObject.SetActive(false);
    }


    /// <summary>
    /// ���� �̹��� ������Ʈ
    /// </summary>
    public void CityImageUpdate()
    {
        if (10 > _currentCity.NumOfFacility)
        {
            _cityImage.sprite = _citySprites[0];
        }
        else if (20 > _currentCity.NumOfFacility)
        {
            _cityImage.sprite = _citySprites[1];
        }
        else if (30 > _currentCity.NumOfFacility)
        {
            _cityImage.sprite = _citySprites[2];
        }
        else
        {
            _cityImage.sprite = _citySprites[3];
        }
    }



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        // Ȱ��ȭ ���°� �ƴϸ� ��ȯ
        if (!gameObject.activeSelf)
        {
            return;
        }

        _populationText.text = _currentCity.Population.ToString("0");
        _populationMovementText.text = (_currentCity.PopulationMovement * 12.0f).ToString("0");
        _annualFundText.text = _currentCity.AnnualFund.ToString();
        _annualResearchText.text = _currentCity.AnnualResearch.ToString();
        _crimeText.text = $"{_currentCity.Crime.ToString("F2")}%";
        _diseaseText.text = $"{_currentCity.Disease.ToString("F2")}%";
        _injureText.text = $"{_currentCity.Injure.ToString("F2")}%";
        _stabilityText.text = _currentCity.Stability.ToString("0");
    }


    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // �ʱ� ���� ȭ��
        CurrentCity = PlayManager.Instance.GetCity(0);

        // �븮�� ���
        PlayManager.OnMonthChange += OnMonthChange;
    }


    private void OnEnable()
    {
        CityImageUpdate();
        OnMonthChange();
    }


    private void Update()
    {
        _supportRateImage.fillAmount = PlayManager.Instance[VariableFloat.FacilitySupportRate] * 0.01f;

        // ����Ű ����
#if PLATFORM_STANDALONE_WIN
        // Ű���� ����
        if (Input.GetKeyUp(KeyCode.S))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex + 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
#endif
        // PC, ����� ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
    }
}
