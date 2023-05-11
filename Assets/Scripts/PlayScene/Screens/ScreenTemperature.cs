using TMPro;
using UnityEngine;

public class ScreenTemperature : PlayScreenBase, IUpDownAdjust
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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        if (isUp)
        {
            // 온도 변화량 증가
            ++PlayManager.Instance[VariableShort.TemperatureMovement];

            // 아래 버튼 활성화
            if (!_downButton.activeSelf)
            {
                _downButton.SetActive(true);
            }

            // 변화량이 인프라 수와 같을 때
            if (PlayManager.Instance[VariableByte.TemperatureInfra] <= PlayManager.Instance[VariableShort.TemperatureMovement])
            {
                _upButton.SetActive(false);
            }
        }
        else
        {
            // 온도 변화량 감소
            --PlayManager.Instance[VariableShort.TemperatureMovement];

            // 위 버튼 활성화
            if (!_upButton.activeSelf)
            {
                _upButton.SetActive(true);
            }

            // 변화량이 인프라 수와 같을 때
            if (PlayManager.Instance[VariableByte.TemperatureInfra] <= -PlayManager.Instance[VariableShort.TemperatureMovement])
            {
                _downButton.SetActive(false);
            }
        }

        // 월간 변화량 표시
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.TemperatureMovement] * Constants.TEMPERATURE_MOVEMENT).ToString()}℃";

        // 월간 비용 표시
        _montlyCostText.text = $"{Language.Instance["비용"]} {PlayManager.Instance[VariableShort.TemperatureMovement].ToString()}/{Language.Instance["월"]}";
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
        ++PlayManager.Instance[VariableByte.TemperatureInfra];

        // 지출
        PlayManager.Instance[VariableLong.Funds] -= _buildingInfraCost;

        // 건설된 인프라 수 표시
        _numOfInfra.text = PlayManager.Instance[VariableByte.TemperatureInfra].ToString();

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

    private void Awake()
    {
        // 고정 값
        _planetDistanceText.text = UIString.Instance[VariableFloat.PlanetDistance_AU];

        // 건설된 인프라 수 표시
        _numOfInfra.text = PlayManager.Instance[VariableByte.TemperatureInfra].ToString();

        // 위,아래 버튼 활성화 여부
        if (PlayManager.Instance[VariableByte.TemperatureInfra] <= PlayManager.Instance[VariableShort.TemperatureMovement])
        {
            _upButton.SetActive(false);
        }
        if (PlayManager.Instance[VariableByte.TemperatureInfra] <= -PlayManager.Instance[VariableShort.TemperatureMovement])
        {
            _downButton.SetActive(false);
        }

        // 월간 변화량 표시
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.TemperatureMovement] * Constants.TEMPERATURE_MOVEMENT).ToString()}℃";

        // 월간 비용 표시
        _montlyCostText.text = $"{Language.Instance["비용"]} {PlayManager.Instance[VariableShort.TemperatureMovement].ToString()}/{Language.Instance["월"]}";
    }


    private void Update()
    {
        // 정보 표시
        _totalTemperatureText.text = UIString.Instance[VariableFloat.TotalTemperature_C];
        _totalReflectionText.text = UIString.Instance[VariableFloat.TotalReflection];
        _waterGreenHouseText.text = UIString.Instance[VariableFloat.WaterGreenHouse_C];
        _carbonGreenHouseText.text = UIString.Instance[VariableFloat.CarbonGreenHouse_C];
        _etcGreenHouseText.text = UIString.Instance[VariableFloat.EtcGreenHouse_C];

        // 시각적 정보 표시
        float meterX = (PlayManager.Instance[VariableFloat.TotalTemperature_C] - Constants.EARTH_TEMPERATURE) * _meterMultiply;
        if (meterX > Constants.HALF_METAIMAGE_WIDTH)
        {
            meterX = Constants.HALF_METAIMAGE_WIDTH;
        }
        _meterCursor.localPosition = new Vector3(meterX, 0.0f, 0.0f);

        // 인프라 건설 가능 여부
        if (_infraBuildavailable)
        {
            // 건설 비용이 없거나 255개가 됐을 때
            if (_buildingInfraCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.TemperatureInfra] >= 255)
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
