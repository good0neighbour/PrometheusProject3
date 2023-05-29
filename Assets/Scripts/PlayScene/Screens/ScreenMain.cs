using UnityEngine;
using TMPro;

public class ScreenMain : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("����")]
    [SerializeField] private GameObject _popUpMenuScreen = null;
    [SerializeField] private GameObject _popMessageLogScreen = null;
    [SerializeField] private TMP_Text _pauseBtnText = null;
    [SerializeField] private TMP_Text _speedBtnText = null;
    [SerializeField] private TMP_Text _dateText = null;
    [SerializeField] private TMP_Text _eraText = null;
    [SerializeField] private TMP_Text _airPressure = null;
    [SerializeField] private TMP_Text _airMass = null;
    [SerializeField] private TMP_Text _temperature = null;
    [SerializeField] private TMP_Text _waterVolume = null;
    [SerializeField] private TMP_Text _carbonRatio = null;
    [SerializeField] private TMP_Text _oxygenRatio = null;
    [SerializeField] private TMP_Text _photo = null;
    [SerializeField] private TMP_Text _breath = null;
    [SerializeField] private TMP_Text _fund = null;
    [SerializeField] private TMP_Text _maintenance = null;
    [SerializeField] private TMP_Text _annualFund = null;
    [SerializeField] private TMP_Text _research = null;
    [SerializeField] private TMP_Text _annualResearch = null;
    [SerializeField] private TMP_Text _culture = null;
    [SerializeField] private TMP_Text _annualCulture = null;
    [SerializeField] private TMP_Text _population = null;
    [SerializeField] private TMP_Text _supportRate = null;

    private string _totalWaterVolumeString = null;
    private string _photoStabilityString = null;
    private string _breathStabilityString = null;
    private float _totalWaterVolume = 0.0f;
    private float _photoStability = 0.0f;
    private float _breathStability = 0.0f;
    private bool _isMessageLogOpened = false;



    /* ==================== Public Methods ==================== */

    public void BtnGamePause()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� �Ͻ� ����, �簳
        switch (PlayManager.Instance.GameResume)
        {
            case Constants.GAME_RESUME:
                PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
                _pauseBtnText.color = Constants.WHITE;
                _speedBtnText.color = Constants.TEXT_BUTTON_DISABLE;
                return;

            case Constants.GAME_PAUSE:
                PlayManager.Instance.GameResume = Constants.GAME_RESUME;
                _pauseBtnText.color = Constants.TEXT_BUTTON_DISABLE;
                _speedBtnText.color = Constants.WHITE;
                return;
        }
    }


    public void BtnGameSpeed()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �Ͻ� ���� ������ ��
        if (Constants.GAME_PAUSE == PlayManager.Instance.GameResume)
        {
            PlayManager.Instance.GameResume = Constants.GAME_RESUME;
            _pauseBtnText.color = Constants.TEXT_BUTTON_DISABLE;
            _speedBtnText.color = Constants.WHITE;
            return;
        }

        // ���� �ӵ� ����
        switch (PlayManager.Instance.GameSpeed)
        {
            case 1.0f:
                PlayManager.Instance.GameSpeed = 2.0f;
                _speedBtnText.text = Constants.GAME_SPEED_PHASE_2;
                return;

            case 2.0f:
                PlayManager.Instance.GameSpeed = 4.0f;
                _speedBtnText.text = Constants.GAME_SPEED_PHASE_3;
                return;

            case 4.0f:
                PlayManager.Instance.GameSpeed = 8.0f;
                _speedBtnText.text = Constants.GAME_SPEED_PHASE_4;
                return;

            case 8.0f:
                PlayManager.Instance.GameSpeed = 1.0f;
                _speedBtnText.text = Constants.GAME_SPEED_PHASE_1;
                return;
        }
    }


    public void BtnPopUpMenu()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ����
        _popUpMenuScreen.SetActive(true);
        gameObject.SetActive(false);
        GeneralMenuButtons.Instance.EnableThis(false);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    public void BtnMessageLog(bool openPopUpScreen)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ����
        if (openPopUpScreen)
        {
            _popMessageLogScreen.SetActive(true);
            gameObject.SetActive(false);
            GeneralMenuButtons.Instance.EnableThis(false);
            PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
        }
        else
        {
            _popMessageLogScreen.SetActive(false);
            gameObject.SetActive(true);
            GeneralMenuButtons.Instance.EnableThis(true);
            PlayManager.Instance.GameResume = Constants.GAME_RESUME;
        }

        _isMessageLogOpened = openPopUpScreen;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// �ٸ� ������ ǥ���ϱ� ���� ���� �Լ�.
    /// </summary>
    private string GetTotalWaterVolume()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_totalWaterVolume != PlayManager.Instance[VariableFloat.TotalWater_PL] || string.IsNullOrEmpty(_totalWaterVolumeString))
        {
            // ���� �� ����
            _totalWaterVolume = PlayManager.Instance[VariableFloat.TotalWater_PL];

            // ���� ǥ��
            _totalWaterVolumeString = $"{(_totalWaterVolume * Constants.E_3).ToString("F2")}EL";
        }

        // ��ȯ
        return _totalWaterVolumeString;
    }


    private string GetPhotoStability()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_photoStability != PlayManager.Instance[VariableFloat.PhotoLifeStability] || string.IsNullOrEmpty(_photoStabilityString))
        {
            // ���� �� ����
            _photoStability = PlayManager.Instance[VariableFloat.PhotoLifeStability];

            // ���� ǥ��
            _photoStabilityString = $"{_photoStability.ToString("0")}";
        }

        // ��ȯ
        return _photoStabilityString;
    }


    private string GetBreathStability()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_breathStability != PlayManager.Instance[VariableFloat.BreathLifeStability] || string.IsNullOrEmpty(_breathStabilityString))
        {
            // ���� �� ����
            _breathStability = PlayManager.Instance[VariableFloat.BreathLifeStability];

            // ���� ǥ��
            _breathStabilityString = $"{_breathStability.ToString("0")}";
        }

        // ��ȯ
        return _breathStabilityString;
    }


    private void OnMonthChange()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        _dateText.text = UIString.Instance.GetDateString();
        _eraText.text = UIString.Instance.GetEraString();
        _airPressure.text = UIString.Instance[VariableFloat.TotalAirPressure_hPa];
        _airMass.text = UIString.Instance[VariableFloat.TotalAirMass_Tt];
        _temperature.text = UIString.Instance[VariableFloat.TotalTemperature_C];
        _waterVolume.text = GetTotalWaterVolume();
        _carbonRatio.text = UIString.Instance[VariableFloat.TotalCarbonRatio_ppm];
        _oxygenRatio.text = UIString.Instance[VariableFloat.OxygenRatio];
        _photo.text = GetPhotoStability();
        _breath.text = GetBreathStability();
        _fund.text = UIString.Instance[VariableLong.Funds];
        _maintenance.text = UIString.Instance[VariableUint.Maintenance];
        _annualFund.text = UIString.Instance[VariableInt.AnnualFund];
        _research.text = UIString.Instance[VariableUint.Research];
        _annualResearch.text = UIString.Instance[VariableUshort.AnnualResearch];
        _culture.text = UIString.Instance[VariableUint.Culture];
        _annualCulture.text = UIString.Instance[VariableUshort.AnnualCulture];

        double totalPopulation = 0.0;
        for (ushort i = 0; i < PlayManager.Instance[VariableUshort.CityNum]; ++i)
        {
            totalPopulation += PlayManager.Instance.GetCity(i).Population;
        }
        _population.text = totalPopulation.ToString("0");

        _supportRate.text = $"{((PlayManager.Instance[VariableFloat.FacilitySupportRate] + PlayManager.Instance[VariableFloat.ResearchSupportRate] + PlayManager.Instance[VariableFloat.SocietySupportRate] + PlayManager.Instance[VariableFloat.DiplomacySupportRate]) * 0.25).ToString("F2")}%";
    }


    private void Awake()
    {
        // �븮�� ���
        PlayManager.OnMonthChange += OnMonthChange;
    }


    private void OnEnable()
    {
        OnMonthChange();
    }


    private void Update()
    {
        // ����Ű ����
        // �޼��� �α� ������ ��
        if (_isMessageLogOpened)
        {
            // ���� ����
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                BtnMessageLog(false);
            }
        }
        // �߾� ȭ���� ��
        else
        {
#if PLATFORM_STANDALONE_WIN
            // Ű���� ����
            if (Input.GetKeyUp(KeyCode.A))
            {
                GeneralMenuButtons.Instance.BtnLeftRight(true);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                GeneralMenuButtons.Instance.BtnLeftRight(false);
            }
#endif
            // ���� ����
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                BtnPopUpMenu();
            }
        }
    }
}
