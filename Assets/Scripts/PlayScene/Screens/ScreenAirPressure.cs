using UnityEngine;
using TMPro;

public class ScreenAirPressure : PlayScreenBase, IUpDownAdjust
{
    /* ==================== Variables ==================== */

    [Header("비용")]
    [SerializeField] private ushort _buildingInfraCost = 0;

    [Header("참조")]
    [SerializeField] private GameObject _upButton = null;
    [SerializeField] private GameObject _downButton = null;
    [SerializeField] private RectTransform _meterCursor = null;
    [SerializeField] private TMP_Text _buildInfraButton = null;
    [SerializeField] private TMP_Text _numOfInfra = null;
    [SerializeField] private TMP_Text _montlyMovementText = null;
    [SerializeField] private TMP_Text _montlyCostText = null;
    [SerializeField] private TMP_Text _totalAirPressureText = null;
    [SerializeField] private TMP_Text _airMassText = null;
    [SerializeField] private TMP_Text _waterGasMassText = null;
    [SerializeField] private TMP_Text _carbonGasMassText = null;
    [SerializeField] private TMP_Text _etcAirMassText = null;
    [SerializeField] private TMP_Text _gravityAccelationText = null;
    [SerializeField] private TMP_Text _planetAreaText = null;

    private float _waterGasMass = 0.0f;
    private string _waterGasMassString = null;
    private bool _infraBuildavailable = true;
    private float _meterMultiply = 1.0f / Constants.EARTH_AIR_PRESSURE * Constants.HALF_METAIMAGE_WIDTH;



    /* ==================== Public Methods ==================== */

    public void BtnUpDown(bool isUp)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        if (isUp)
        {
            // 대기 질량 변화량 증가
            ++PlayManager.Instance[VariableShort.AirMassMovement];

            // 아래 버튼 활성화
            if (!_downButton.activeSelf)
            {
                _downButton.SetActive(true);
            }

            // 변화량이 인프라 수와 같을 때
            if (PlayManager.Instance[VariableByte.AirPressureInfra] <= PlayManager.Instance[VariableShort.AirMassMovement])
            {
                _upButton.SetActive(false);
            }
        }
        else
        {
            // 대기 질량 변화량 감소
            --PlayManager.Instance[VariableShort.AirMassMovement];

            // 위 버튼 활성화
            if (!_upButton.activeSelf)
            {
                _upButton.SetActive(true);
            }

            // 변화량이 인프라 수와 같을 때
            if (PlayManager.Instance[VariableByte.AirPressureInfra] <= -PlayManager.Instance[VariableShort.AirMassMovement])
            {
                _downButton.SetActive(false);
            }
        }

        // 월간 변화량 표시
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.AirMassMovement] * Constants.AIRMASS_MOVEMENT).ToString()}Tt/{Language.Instance["월"]}";

        // 월간 비용 표시
        _montlyCostText.text = $"{Language.Instance["비용"]} {PlayManager.Instance[VariableShort.AirMassMovement].ToString()}/{Language.Instance["월"]}";
    }


    public void BtnBuildInfra()
    {
        // 사용 불가
        if (!_infraBuildavailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 건설
        ++PlayManager.Instance[VariableByte.AirPressureInfra];

        // 지출
        PlayManager.Instance[VariableLong.Funds] -= _buildingInfraCost;

        // 건설된 인프라 수 표시
        _numOfInfra.text = PlayManager.Instance[VariableByte.AirPressureInfra].ToString();

        // 위, 아래 버튼 활성화
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
    /// 다른 단위로 표시하기 위해 만든 함수.
    /// </summary>
    private string GetWaterGasMass()
    {
        // 값이 바뀌었거나 문자열을 생성한 적 없을 때
        if (_waterGasMass != PlayManager.Instance[VariableFloat.WaterGas_PL] || null == _waterGasMassString)
        {
            // 현재 값 저장
            _waterGasMass = PlayManager.Instance[VariableFloat.WaterGas_PL];

            // 단위 표시
            _waterGasMassString = $"{_waterGasMass.ToString("F2")}Tt";
        }

        // 반환
        return _waterGasMassString;
    }


    private void Awake()
    {
        // 고정 값
        _gravityAccelationText.text = UIString.Instance[VariableFloat.GravityAccelation_m_s2];
        _planetAreaText.text = $"{(PlayManager.Instance[VariableFloat.PlanetArea_km2] * 0.0000001f).ToString("F2")}Mm²";

        // 건설된 인프라 수 표시
        _numOfInfra.text = PlayManager.Instance[VariableByte.AirPressureInfra].ToString();

        // 위,아래 버튼 활성화 여부
        if (PlayManager.Instance[VariableByte.AirPressureInfra] <= PlayManager.Instance[VariableShort.AirMassMovement])
        {
            _upButton.SetActive(false);
        }
        if (PlayManager.Instance[VariableByte.AirPressureInfra] <= -PlayManager.Instance[VariableShort.AirMassMovement])
        {
            _downButton.SetActive(false);
        }

        // 월간 변화량 표시
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.AirMassMovement] * Constants.AIRMASS_MOVEMENT).ToString()}Tt/{Language.Instance["월"]}";

        // 월간 비용 표시
        _montlyCostText.text = $"{Language.Instance["비용"]} {PlayManager.Instance[VariableShort.AirMassMovement].ToString()}/{Language.Instance["월"]}";
    }


    private void Update()
    {
        // 정보 표시
        _totalAirPressureText.text = UIString.Instance[VariableFloat.TotalAirPressure_hPa];
        _airMassText.text = UIString.Instance[VariableFloat.TotalAirMass_Tt];
        _waterGasMassText.text = GetWaterGasMass();
        _carbonGasMassText.text = UIString.Instance[VariableFloat.CarbonGasMass_Tt];
        _etcAirMassText.text = UIString.Instance[VariableFloat.EtcAirMass_Tt];

        // 시각적 정보 표시
        float meterX = (PlayManager.Instance[VariableFloat.TotalAirPressure_hPa] - Constants.EARTH_AIR_PRESSURE) * _meterMultiply;
        if (meterX > Constants.HALF_METAIMAGE_WIDTH)
        {
            meterX = Constants.HALF_METAIMAGE_WIDTH;
        }
        _meterCursor.localPosition = new Vector3(meterX, 0.0f, 0.0f);

        // 인프라 건설 가능 여부
        if (_infraBuildavailable)
        {
            // 건설 비용이 없거나 255개가 됐을 때
            if (_buildingInfraCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.AirPressureInfra] >= byte.MaxValue)
            {
                _infraBuildavailable = false;
                _buildInfraButton.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
        // 건설 비용이 생겼을 때
        else if (_buildingInfraCost < PlayManager.Instance[VariableLong.Funds])
        {
            _infraBuildavailable = true;
            _buildInfraButton.color = Constants.WHITE;
        }

        // 단축키 동작
#if PLATFORM_STANDALONE_WIN
        // 키보드 동작
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
        // PC, 모바일 공용
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(false);
        }
    }
}
