using TMPro;
using UnityEngine;

public class ScreenCarbon : PlayScreenBase, IUpDownAdjust
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
    [SerializeField] private TMP_Text _totalCarbonRatioText = null;
    [SerializeField] private TMP_Text _atmosphereText = null;
    [SerializeField] private TMP_Text _hydrosphereText = null;
    [SerializeField] private TMP_Text _lithosphereText = null;
    [SerializeField] private TMP_Text _biosphereText = null;

    private bool _infraBuildavailable = true;



    /* ==================== Public Methods ==================== */

    public void BtnUpDown(bool isUp)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido();

        if (isUp)
        {
            // ź�� �� ��ȭ�� ����
            ++PlayManager.Instance[VariableShort.CarbonMovement];

            // �Ʒ� ��ư Ȱ��ȭ
            if (!_downButton.activeSelf)
            {
                _downButton.SetActive(true);
            }

            // ��ȭ���� ������ ���� ���� ��
            if (PlayManager.Instance[VariableByte.CarbonInfra] <= PlayManager.Instance[VariableShort.CarbonMovement])
            {
                _upButton.SetActive(false);
            }
        }
        else
        {
            // ź�� �� ��ȭ�� ����
            --PlayManager.Instance[VariableShort.CarbonMovement];

            // �� ��ư Ȱ��ȭ
            if (!_upButton.activeSelf)
            {
                _upButton.SetActive(true);
            }

            // ��ȭ���� ������ ���� ���� ��
            if (PlayManager.Instance[VariableByte.CarbonInfra] <= -PlayManager.Instance[VariableShort.CarbonMovement])
            {
                _downButton.SetActive(false);
            }
        }

        // ���� ��ȭ�� ǥ��
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.CarbonMovement] * Constants.CARBON_RATIO_MOVEMENT).ToString()}ppm/{Language.Instance["��"]}";

        // ���� ��� ǥ��
        _montlyCostText.text = $"{Language.Instance["���"]} {PlayManager.Instance[VariableShort.CarbonMovement].ToString()}/{Language.Instance["��"]}";
    }


    public void BtnBuildInfra()
    {
        // ��� �Ұ�
        if (!_infraBuildavailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido();

        // �Ǽ�
        ++PlayManager.Instance[VariableByte.CarbonInfra];

        // ����
        PlayManager.Instance[VariableLong.Funds] -= _buildingInfraCost;

        // �Ǽ��� ������ �� ǥ��
        _numOfInfra.text = PlayManager.Instance[VariableByte.CarbonInfra].ToString();

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
        // �Ǽ��� ������ �� ǥ��
        _numOfInfra.text = PlayManager.Instance[VariableByte.CarbonInfra].ToString();

        // ��,�Ʒ� ��ư Ȱ��ȭ ����
        if (PlayManager.Instance[VariableByte.CarbonInfra] <= PlayManager.Instance[VariableShort.CarbonMovement])
        {
            _upButton.SetActive(false);
        }
        if (PlayManager.Instance[VariableByte.CarbonInfra] <= -PlayManager.Instance[VariableShort.CarbonMovement])
        {
            _downButton.SetActive(false);
        }

        // ���� ��ȭ�� ǥ��
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.CarbonMovement] * Constants.CARBON_RATIO_MOVEMENT).ToString()}ppm/{Language.Instance["��"]}";

        // ���� ��� ǥ��
        _montlyCostText.text = $"{Language.Instance["���"]} {PlayManager.Instance[VariableShort.CarbonMovement].ToString()}/{Language.Instance["��"]}";
    }


    private void Update()
    {
        // ���� ǥ��
        _totalCarbonRatioText.text = UIString.Instance[VariableFloat.TotalCarbonRatio_ppm];
        _atmosphereText.text = UIString.Instance[VariableFloat.CarbonGasMass_Tt];
        _hydrosphereText.text = UIString.Instance[VariableFloat.CarbonLiquidMass_Tt];
        _lithosphereText.text = UIString.Instance[VariableFloat.CarbonSolidMass_Tt];
        _biosphereText.text = UIString.Instance[VariableFloat.CarbonLifeMass_Tt];

        // ������ �Ǽ� ���� ����
        if (_infraBuildavailable)
        {
            // �Ǽ� ����� ���ų� 255���� ���� ��
            if (_buildingInfraCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.CarbonInfra] >= 255)
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
