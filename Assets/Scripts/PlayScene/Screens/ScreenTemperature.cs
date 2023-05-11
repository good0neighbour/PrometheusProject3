using TMPro;
using UnityEngine;

public class ScreenTemperature : PlayScreenBase, IUpDownAdjust
{
    /* ==================== Variables ==================== */

    [Header("���")]
    [SerializeField] private ushort _buildingInfraCost = 0;

    [Header("����")]
    [SerializeField] private GameObject _upButton = null;
    [SerializeField] private GameObject _downButton = null;
    [SerializeField] private RectTransform _meterCursor = null;
    [SerializeField] private TMP_Text _buildInfraButton = null;
    [SerializeField] private TMP_Text _numOfInfra = null;
    [SerializeField] private TMP_Text _montlyMovementText = null;
    [SerializeField] private TMP_Text _montlyCostText = null;
    [SerializeField] private TMP_Text _totalTemperatureText = null;
    [SerializeField] private TMP_Text _totalReflectionText = null;
    [SerializeField] private TMP_Text _waterGreenHouseText = null;
    [SerializeField] private TMP_Text _carbonGreenHouseText = null;
    [SerializeField] private TMP_Text _etcGreenHouseText = null;
    [SerializeField] private TMP_Text _planetDistanceText = null;

    private bool _infraBuildavailable = true;
    private float _meterMultiply = 1.0f / 288.0f * Constants.HALF_METAIMAGE_WIDTH;



    /* ==================== Public Methods ==================== */

    public void BtnUpDown(bool isUp)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        if (isUp)
        {
            // �µ� ��ȭ�� ����
            ++PlayManager.Instance[VariableShort.TemperatureMovement];

            // �Ʒ� ��ư Ȱ��ȭ
            if (!_downButton.activeSelf)
            {
                _downButton.SetActive(true);
            }

            // ��ȭ���� ������ ���� ���� ��
            if (PlayManager.Instance[VariableByte.TemperatureInfra] <= PlayManager.Instance[VariableShort.TemperatureMovement])
            {
                _upButton.SetActive(false);
            }
        }
        else
        {
            // �µ� ��ȭ�� ����
            --PlayManager.Instance[VariableShort.TemperatureMovement];

            // �� ��ư Ȱ��ȭ
            if (!_upButton.activeSelf)
            {
                _upButton.SetActive(true);
            }

            // ��ȭ���� ������ ���� ���� ��
            if (PlayManager.Instance[VariableByte.TemperatureInfra] <= -PlayManager.Instance[VariableShort.TemperatureMovement])
            {
                _downButton.SetActive(false);
            }
        }

        // ���� ��ȭ�� ǥ��
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.TemperatureMovement] * Constants.TEMPERATURE_MOVEMENT).ToString()}��";

        // ���� ��� ǥ��
        _montlyCostText.text = $"{Language.Instance["���"]} {PlayManager.Instance[VariableShort.TemperatureMovement].ToString()}/{Language.Instance["��"]}";
    }


    public void BtnBuildInfra()
    {
        // ��� �Ұ�
        if (!_infraBuildavailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �Ǽ�
        ++PlayManager.Instance[VariableByte.TemperatureInfra];

        // ����
        PlayManager.Instance[VariableLong.Funds] -= _buildingInfraCost;

        // �Ǽ��� ������ �� ǥ��
        _numOfInfra.text = PlayManager.Instance[VariableByte.TemperatureInfra].ToString();

        // ��, �Ʒ� ��ư Ȱ��ȭ
        if (!_upButton.activeSelf)
        {
            _upButton.SetActive(true);
        }
        if (!_downButton.activeSelf)
        {
            _downButton.SetActive(true);
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ���� ��
        _planetDistanceText.text = UIString.Instance[VariableFloat.PlanetDistance_AU];

        // �Ǽ��� ������ �� ǥ��
        _numOfInfra.text = PlayManager.Instance[VariableByte.TemperatureInfra].ToString();

        // ��,�Ʒ� ��ư Ȱ��ȭ ����
        if (PlayManager.Instance[VariableByte.TemperatureInfra] <= PlayManager.Instance[VariableShort.TemperatureMovement])
        {
            _upButton.SetActive(false);
        }
        if (PlayManager.Instance[VariableByte.TemperatureInfra] <= -PlayManager.Instance[VariableShort.TemperatureMovement])
        {
            _downButton.SetActive(false);
        }

        // ���� ��ȭ�� ǥ��
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.TemperatureMovement] * Constants.TEMPERATURE_MOVEMENT).ToString()}��";

        // ���� ��� ǥ��
        _montlyCostText.text = $"{Language.Instance["���"]} {PlayManager.Instance[VariableShort.TemperatureMovement].ToString()}/{Language.Instance["��"]}";
    }


    private void Update()
    {
        // ���� ǥ��
        _totalTemperatureText.text = UIString.Instance[VariableFloat.TotalTemperature_C];
        _totalReflectionText.text = UIString.Instance[VariableFloat.TotalReflection];
        _waterGreenHouseText.text = UIString.Instance[VariableFloat.WaterGreenHouse_C];
        _carbonGreenHouseText.text = UIString.Instance[VariableFloat.CarbonGreenHouse_C];
        _etcGreenHouseText.text = UIString.Instance[VariableFloat.EtcGreenHouse_C];

        // �ð��� ���� ǥ��
        float meterX = (PlayManager.Instance[VariableFloat.TotalTemperature_C] - Constants.EARTH_TEMPERATURE) * _meterMultiply;
        if (meterX > Constants.HALF_METAIMAGE_WIDTH)
        {
            meterX = Constants.HALF_METAIMAGE_WIDTH;
        }
        _meterCursor.localPosition = new Vector3(meterX, 0.0f, 0.0f);

        // ������ �Ǽ� ���� ����
        if (_infraBuildavailable)
        {
            // �Ǽ� ����� ���ų� 255���� ���� ��
            if (_buildingInfraCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.TemperatureInfra] >= 255)
            {
                _infraBuildavailable = false;
                _buildInfraButton.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
        // �Ǽ� ����� ������ ��
        else if (_buildingInfraCost < PlayManager.Instance[VariableLong.Funds])
        {
            _infraBuildavailable = true;
            _buildInfraButton.color = Constants.WHITE;
        }

        // ����Ű ����
#if PLATFORM_STANDALONE_WIN
        // Ű���� ����
        if (Input.GetKeyUp(KeyCode.W))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentLeftIndex - 1);
        }
        else if (Input.GetKeyUp(KeyCode.S))
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
