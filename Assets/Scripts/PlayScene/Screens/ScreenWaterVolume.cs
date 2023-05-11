using TMPro;
using UnityEngine;

public class ScreenWaterVolume : PlayScreenBase, IUpDownAdjust
{
    /* ==================== Variables ==================== */

    [Header("비용")]
    [SerializeField] private ushort _buildingInfraCost = 0;

    [Header("참조")]
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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        if (isUp)
        {
            // 물 체적 변화량 증가
            ++PlayManager.Instance[VariableShort.WaterMovement];

            // 아래 버튼 활성화
            if (!_downButton.activeSelf)
            {
                _downButton.SetActive(true);
            }

            // 변화량이 인프라 수와 같을 때
            if (PlayManager.Instance[VariableByte.WaterInfra] <= PlayManager.Instance[VariableShort.WaterMovement])
            {
                _upButton.SetActive(false);
            }
        }
        else
        {
            // 물 체적 변화량 감소
            --PlayManager.Instance[VariableShort.WaterMovement];

            // 위 버튼 활성화
            if (!_upButton.activeSelf)
            {
                _upButton.SetActive(true);
            }

            // 변화량이 인프라 수와 같을 때
            if (PlayManager.Instance[VariableByte.WaterInfra] <= -PlayManager.Instance[VariableShort.WaterMovement])
            {
                _downButton.SetActive(false);
            }
        }

        // 월간 변화량 표시
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.WaterMovement] * Constants.WATER_VOLUME_MOVEMENT).ToString()}PL/{Language.Instance["월"]}";

        // 월간 비용 표시
        _montlyCostText.text = $"{Language.Instance["비용"]} {PlayManager.Instance[VariableShort.WaterMovement].ToString()}/{Language.Instance["월"]}";
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
        ++PlayManager.Instance[VariableByte.WaterInfra];

        // 지출
        PlayManager.Instance[VariableLong.Funds] -= _buildingInfraCost;

        // 건설된 인프라 수 표시
        _numOfInfra.text = PlayManager.Instance[VariableByte.WaterInfra].ToString();

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
    private string GetTotalWaterVolume()
    {
        // 값이 바뀌었거나 문자열을 생성한 적 없을 때
        if (_totalWaterVolume != PlayManager.Instance[VariableFloat.TotalWater_PL] || null == _totalWaterVolumeString)
        {
            // 현재 값 저장
            _totalWaterVolume = PlayManager.Instance[VariableFloat.TotalWater_PL];

            // 단위 표시
            _totalWaterVolumeString = $"{(_totalWaterVolume * Constants.E_3).ToString("F2")}EL";
        }

        // 반환
        return _totalWaterVolumeString;
    }


    private void Awake()
    {
        // 건설된 인프라 수 표시
        _numOfInfra.text = PlayManager.Instance[VariableByte.WaterInfra].ToString();

        // 위,아래 버튼 활성화 여부
        if (PlayManager.Instance[VariableByte.WaterInfra] <= PlayManager.Instance[VariableShort.WaterMovement])
        {
            _upButton.SetActive(false);
        }
        if (PlayManager.Instance[VariableByte.WaterInfra] <= -PlayManager.Instance[VariableShort.WaterMovement])
        {
            _downButton.SetActive(false);
        }

        // 월간 변화량 표시
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.WaterMovement] * Constants.WATER_VOLUME_MOVEMENT).ToString()}PL/{Language.Instance["월"]}";

        // 월간 비용 표시
        _montlyCostText.text = $"{Language.Instance["비용"]} {PlayManager.Instance[VariableShort.WaterMovement].ToString()}/{Language.Instance["월"]}";
    }


    private void Update()
    {
        // 정보 표시
        _totalWaterVolumeText.text = GetTotalWaterVolume();
        _waterGasVolumeText.text = UIString.Instance[VariableFloat.WaterGas_PL];
        _waterLiquidVolumeText.text = UIString.Instance[VariableFloat.WaterLiquid_PL];
        _waterSolidVolumeText.text = UIString.Instance[VariableFloat.WaterSolid_PL];

        // 인프라 건설 가능 여부
        if (_infraBuildavailable)
        {
            // 건설 비용이 없거나 255개가 됐을 때
            if (_buildingInfraCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.WaterInfra] >= byte.MaxValue)
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
