using TMPro;
using UnityEngine;

public class ScreenWaterVolume : PlayScreenBase, IUpDownAdjust
{
    /* ==================== Variables ==================== */

    [Header("���")]
    [SerializeField] private ushort _buildingInfraCost = 0;

    [Header("����")]
    [SerializeField] private GameObject _upButton = null;
    [SerializeField] private GameObject _downButton = null;
    [SerializeField] private TMP_Text _buildInfraButton = null;
    [SerializeField] private TMP_Text _numOfInfra = null;
    [SerializeField] private TMP_Text _montlyMovementText = null;
    [SerializeField] private TMP_Text _montlyCostText = null;
    [SerializeField] private TMP_Text _totalWaterVolumeText = null;
    [SerializeField] private TMP_Text _waterGasVolumeText = null;
    [SerializeField] private TMP_Text _waterLiquidVolumeText = null;
    [SerializeField] private TMP_Text _waterSolidVolumeText = null;

    private float _totalWaterVolume = 0.0f;
    private string _totalWaterVolumeString = null;
    private bool _infraBuildavailable = true;



    /* ==================== Public Methods ==================== */

    public void BtnUpDown(bool isUp)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        if (isUp)
        {
            // �� ü�� ��ȭ�� ����
            ++PlayManager.Instance[VariableShort.WaterMovement];

            // �Ʒ� ��ư Ȱ��ȭ
            if (!_downButton.activeSelf)
            {
                _downButton.SetActive(true);
            }

            // ��ȭ���� ������ ���� ���� ��
            if (PlayManager.Instance[VariableByte.WaterInfra] <= PlayManager.Instance[VariableShort.WaterMovement])
            {
                _upButton.SetActive(false);
            }
        }
        else
        {
            // �� ü�� ��ȭ�� ����
            --PlayManager.Instance[VariableShort.WaterMovement];

            // �� ��ư Ȱ��ȭ
            if (!_upButton.activeSelf)
            {
                _upButton.SetActive(true);
            }

            // ��ȭ���� ������ ���� ���� ��
            if (PlayManager.Instance[VariableByte.WaterInfra] <= -PlayManager.Instance[VariableShort.WaterMovement])
            {
                _downButton.SetActive(false);
            }
        }

        // ���� ��ȭ�� ǥ��
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.WaterMovement] * Constants.WATER_VOLUME_MOVEMENT).ToString()}PL/{Language.Instance["��"]}";

        // ���� ��� ǥ��
        _montlyCostText.text = $"{Language.Instance["���"]} {PlayManager.Instance[VariableShort.WaterMovement].ToString()}/{Language.Instance["��"]}";
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
        ++PlayManager.Instance[VariableByte.WaterInfra];

        // ����
        PlayManager.Instance[VariableLong.Funds] -= _buildingInfraCost;

        // �Ǽ��� ������ �� ǥ��
        _numOfInfra.text = PlayManager.Instance[VariableByte.WaterInfra].ToString();

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

    /// <summary>
    /// �ٸ� ������ ǥ���ϱ� ���� ���� �Լ�.
    /// </summary>
    private string GetTotalWaterVolume()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_totalWaterVolume != PlayManager.Instance[VariableFloat.TotalWater_PL] || null == _totalWaterVolumeString)
        {
            // ���� �� ����
            _totalWaterVolume = PlayManager.Instance[VariableFloat.TotalWater_PL];

            // ���� ǥ��
            _totalWaterVolumeString = $"{(_totalWaterVolume * Constants.E_3).ToString("F2")}EL";
        }

        // ��ȯ
        return _totalWaterVolumeString;
    }


    private void Awake()
    {
        // �Ǽ��� ������ �� ǥ��
        _numOfInfra.text = PlayManager.Instance[VariableByte.WaterInfra].ToString();

        // ��,�Ʒ� ��ư Ȱ��ȭ ����
        if (PlayManager.Instance[VariableByte.WaterInfra] <= PlayManager.Instance[VariableShort.WaterMovement])
        {
            _upButton.SetActive(false);
        }
        if (PlayManager.Instance[VariableByte.WaterInfra] <= -PlayManager.Instance[VariableShort.WaterMovement])
        {
            _downButton.SetActive(false);
        }

        // ���� ��ȭ�� ǥ��
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.WaterMovement] * Constants.WATER_VOLUME_MOVEMENT).ToString()}PL/{Language.Instance["��"]}";

        // ���� ��� ǥ��
        _montlyCostText.text = $"{Language.Instance["���"]} {PlayManager.Instance[VariableShort.WaterMovement].ToString()}/{Language.Instance["��"]}";
    }


    private void Update()
    {
        // ���� ǥ��
        _totalWaterVolumeText.text = GetTotalWaterVolume();
        _waterGasVolumeText.text = UIString.Instance[VariableFloat.WaterGas_PL];
        _waterLiquidVolumeText.text = UIString.Instance[VariableFloat.WaterLiquid_PL];
        _waterSolidVolumeText.text = UIString.Instance[VariableFloat.WaterSolid_PL];

        // ������ �Ǽ� ���� ����
        if (_infraBuildavailable)
        {
            // �Ǽ� ����� ���ų� 255���� ���� ��
            if (_buildingInfraCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.WaterInfra] >= byte.MaxValue)
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
