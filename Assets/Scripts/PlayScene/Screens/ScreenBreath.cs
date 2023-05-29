using TMPro;
using UnityEngine;

public class ScreenBreath : PlayScreenBase, IRequest
{
    /* ==================== Variables ==================== */

    [Header("���")]
    [SerializeField] private ushort _requestCost = 0;

    [Header("����")]
    [SerializeField] private TMP_Text _requestButton = null;
    [SerializeField] private TMP_Text _costText = null;
    [SerializeField] private TMP_Text _lifePosibilityText = null;
    [SerializeField] private TMP_Text _lifeStabilityText = null;
    [SerializeField] private TMP_Text _airPressureText = null;
    [SerializeField] private TMP_Text _temperatureText = null;
    [SerializeField] private TMP_Text _waterText = null;
    [SerializeField] private TMP_Text _oxygenRatioText = null;

    private float _airPressure = 0.0f;
    private string _airPressureString = null;
    private float _temperature = 0.0f;
    private string _temperatureString = null;
    private float _posibility = -1.0f;
    private string _posibilityString = null;
    private float _stability = 0.0f;
    private string _stabilityString = null;
    private float _water = 0.0f;
    private string _waterString = null;
    private bool _requestAvailable = true;



    /* ==================== Public Methods ==================== */

    public void BtnRequestSeed()
    {
        // ��� �Ұ�
        if (_requestAvailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ����
        PlayManager.Instance[VariableLong.Funds] -= _requestCost;

        // ���� �ִϸ��̼�
        BottomBarLeft.Instance.SpendAnimation(BottomBarLeft.Displays.Fund);

        // ������ 1��ŭ ����
        ++PlayManager.Instance[VariableFloat.BreathLifeStability];
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// �ٸ� ������ ǥ���ϱ� ���� �������.
    /// </summary>
    private string GetAirPressure()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_airPressure != PlayManager.Instance[VariableFloat.TotalAirPressure_hPa] || string.IsNullOrEmpty(_airPressureString))
        {
            // ���� �� ����
            _airPressure = PlayManager.Instance[VariableFloat.TotalAirPressure_hPa];

            // ���� ǥ��
            _airPressureString = $"{((1.0f - Mathf.Abs(_airPressure / Constants.EARTH_AIR_PRESSURE - 1.0f)) * 100.0f).ToString("0")}%";
        }

        // ��ȯ
        return _airPressureString;
    }


    private string GetTemperature()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_temperature != PlayManager.Instance[VariableFloat.TotalTemperature_C] || string.IsNullOrEmpty(_temperatureString))
        {
            // ���� �� ����
            _temperature = PlayManager.Instance[VariableFloat.TotalTemperature_C];

            // ���� ǥ��
            _temperatureString = $"{((1.0f - Mathf.Abs(_temperature / Constants.EARTH_TEMPERATURE - 1.0f)) * 100.0f).ToString("0")}%";
        }

        // ��ȯ
        return _temperatureString;
    }


    private string GetPosibility()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_posibility != PlayManager.Instance[VariableFloat.BreathLifePosibility] || string.IsNullOrEmpty(_posibilityString))
        {
            if (0.0f < PlayManager.Instance[VariableFloat.BreathLifePosibility])
            {
                // ���� �� ����
                _posibility = PlayManager.Instance[VariableFloat.BreathLifePosibility];

                // ���� ǥ��
                _posibilityString = $"{_posibility.ToString("0")}%";
            }
            else if (0.0f != _posibility)
            {
                _posibility = 0.0f;
                _posibilityString = "0%";
            }
        }

        // ��ȯ
        return _posibilityString;
    }


    private string GetStability()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_stability != PlayManager.Instance[VariableFloat.BreathLifeStability] || string.IsNullOrEmpty(_stabilityString))
        {
            // ���� �� ����
            _stability = PlayManager.Instance[VariableFloat.BreathLifeStability];

            // ���� ǥ��
            _stabilityString = $"{_stability.ToString("0")}";
        }

        // ��ȯ
        return _stabilityString;
    }


    private string GetWater()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_water != PlayManager.Instance[VariableFloat.WaterLiquid_PL] || string.IsNullOrEmpty(_waterString))
        {
            // ���� �� ����
            _water = PlayManager.Instance[VariableFloat.WaterLiquid_PL];

            // ���� ǥ��
            _waterString = $"{(_water * Constants.E_3).ToString("F2")}EL";
        }

        // ��ȯ
        return _waterString;
    }


    private void OnLanguageChange()
    {
        // ��� ǥ��
        _costText.text = $"{Language.Instance["���"]} {_requestCost.ToString()}";
    }


    private void Awake()
    {
        // �ؽ�Ʈ �� �� ������Ʈ
        OnLanguageChange();

        // �븮��
        Language.OnLanguageChange += OnLanguageChange;
    }


    private void Update()
    {
        // ���� ǥ��
        _lifePosibilityText.text = GetPosibility();
        _lifeStabilityText.text = GetStability();
        _airPressureText.text = GetAirPressure();
        _temperatureText.text = GetTemperature();
        _waterText.text = GetWater();
        _oxygenRatioText.text = UIString.Instance[VariableFloat.OxygenRatio];

        // ��û ���� ����
        if (_requestAvailable)
        {
            // ��û ����� ���� ��
            if (_requestCost > PlayManager.Instance[VariableLong.Funds])
            {
                _requestAvailable = false;
                _requestButton.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
        // ��û ����� ������ ��
        else if (_requestCost < PlayManager.Instance[VariableLong.Funds])
        {
            _requestAvailable = true;
            _requestButton.color = Constants.WHITE;
        }

        // ����Ű ����
#if PLATFORM_STANDALONE_WIN
        // Ű���� ����
        if (Input.GetKeyUp(KeyCode.W))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentLeftIndex - 1);
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
