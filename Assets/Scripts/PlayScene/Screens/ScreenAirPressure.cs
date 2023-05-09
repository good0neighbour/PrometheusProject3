using UnityEngine;
using TMPro;

public class ScreenAirPressure : PlayScreenBase, IUpDownAdjust
{
    /* ==================== Variables ==================== */

    [Header("���")]
    [SerializeField] private short _buildingInfraCost = 0;

    [Header("����")]
    [SerializeField] private GameObject _upButton = null;
    [SerializeField] private GameObject _downButton = null;
    [SerializeField] private TMP_Text _buildInfraButton = null;
    [SerializeField] private TMP_Text _numOfInfra = null;
    [SerializeField] private TMP_Text _montlyMovementText = null;
    [SerializeField] private TMP_Text _montlyCostText = null;
    [SerializeField] private TMP_Text _totalAirPressureText = null;
    [SerializeField] private TMP_Text _airMassText = null;
    [SerializeField] private TMP_Text _waterGasMassText = null;
    [SerializeField] private TMP_Text _carbonGasMassText = null;
    [SerializeField] private TMP_Text _gravityAccelationText = null;
    [SerializeField] private TMP_Text _planetAreaText = null;

    private float _waterGasMass = 0.0f;
    private string _waterGasMassString = null;
    private bool _infraBuildavailable = true;



    /* ==================== Public Methods ==================== */

    public void BtnUpDown(bool isUp)
    {
        if (isUp)
        {
            // ��� ���� ��ȭ�� ����
            ++PlayManager.Instance[VariableShort.AirMassMovement];

            // �Ʒ� ��ư Ȱ��ȭ
            if (!_downButton.activeSelf)
            {
                _downButton.SetActive(true);
            }

            // ��ȭ���� ������ ���� ���� ��
            if (PlayManager.Instance[VariableByte.AirPressureInfra] <= PlayManager.Instance[VariableShort.AirMassMovement])
            {
                _upButton.SetActive(false);
            }
        }
        else
        {
            // ��� ���� ��ȭ�� ����
            --PlayManager.Instance[VariableShort.AirMassMovement];

            // �� ��ư Ȱ��ȭ
            if (!_upButton.activeSelf)
            {
                _upButton.SetActive(true);
            }

            // ��ȭ���� ������ ���� ���� ��
            if (PlayManager.Instance[VariableByte.AirPressureInfra] <= -PlayManager.Instance[VariableShort.AirMassMovement])
            {
                _downButton.SetActive(false);
            }
        }

        // ���� ��ȭ�� ǥ��
        _montlyMovementText.text = $"{PlayManager.Instance[VariableShort.AirMassMovement] * Constants.AIRMASS_MOVEMENT}Tt/{Language.Instance["��"]}";

        // ���� ��� ǥ��
        _montlyCostText.text = $"{PlayManager.Instance[VariableShort.AirMassMovement]}Tt/{Language.Instance["��"]}";

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido();
    }


    public void BtnBuildInfra()
    {
        // ��� �Ұ�
        if (!_infraBuildavailable)
        {
            return;
        }

        // �Ǽ�
        ++PlayManager.Instance[VariableByte.AirPressureInfra];

        // �Ǽ��� ������ �� ǥ��
        _numOfInfra.text = PlayManager.Instance[VariableByte.AirPressureInfra].ToString();

        // ��, �Ʒ� ��ư Ȱ��ȭ
        if (!_upButton.activeSelf)
        {
            _upButton.SetActive(true);
        }
        if (!_downButton.activeSelf)
        {
            _downButton.SetActive(true);
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido();
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// �ٸ� ������ ǥ���ϱ� ���� ���� �Լ�.
    /// </summary>
    private string GetWaterGasMass()
    {
        // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
        if (_waterGasMass != PlayManager.Instance[VariableFloat.WaterGas_PL] || null == _waterGasMassString)
        {
            // ���� �� ����
            _waterGasMass = PlayManager.Instance[VariableFloat.WaterGas_PL];

            // ���� ǥ��
            _waterGasMassString = $"{_waterGasMass.ToString("F2")}Tt";
        }

        // ��ȯ
        return _waterGasMassString;
    }


    private void Awake()
    {
        // ���� ��
        _gravityAccelationText.text = UIString.Instance[VariableFloat.GravityAccelation_m_s2];
        _planetAreaText.text = $"{(PlayManager.Instance[VariableFloat.PlanetArea_km2] * 0.0000001f).ToString("F2")}Mm��";

        // �Ǽ��� ������ �� ǥ��
        _numOfInfra.text = PlayManager.Instance[VariableByte.AirPressureInfra].ToString();

        // ��,�Ʒ� ��ư Ȱ��ȭ ����
        if (PlayManager.Instance[VariableByte.AirPressureInfra] <= PlayManager.Instance[VariableShort.AirMassMovement])
        {
            _upButton.SetActive(false);
        }
        if (PlayManager.Instance[VariableByte.AirPressureInfra] <= -PlayManager.Instance[VariableShort.AirMassMovement])
        {
            _downButton.SetActive(false);
        }

        // ���� ��ȭ�� ǥ��
        _montlyMovementText.text = $"{PlayManager.Instance[VariableShort.AirMassMovement] * Constants.AIRMASS_MOVEMENT}Tt/{Language.Instance["��"]}";

        // ���� ��� ǥ��
        _montlyCostText.text = $"{PlayManager.Instance[VariableShort.AirMassMovement]}Tt/{Language.Instance["��"]}";
    }


    private void Update()
    {
        // ���� ǥ��
        _totalAirPressureText.text = UIString.Instance[VariableFloat.TotalAirPressure_hPa];
        _airMassText.text = UIString.Instance[VariableFloat.TotalAirMass_Tt];
        _waterGasMassText.text = GetWaterGasMass();
        _carbonGasMassText.text = UIString.Instance[VariableFloat.CarbonGasMass_Tt];

        // ������ �Ǽ� ���� ����
        if (_infraBuildavailable)
        {
            // �Ǽ� ����� ���ų� 255���� ���� ��
            if (_buildingInfraCost < PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.AirPressureInfra] >= 255)
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
    }
}
