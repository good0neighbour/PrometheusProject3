using TMPro;
using UnityEngine;

public class ScreenCarbon : PlayScreenBase, IUpDownAdjust
{
    /* ==================== Variables ==================== */

    [Header("비용")]
    [SerializeField] private ushort _buildingInfraCost = 0;

    [Header("참조")]
    [SerializeField] private GameObject _upButton = null;
    [SerializeField] private GameObject _downButton = null;
    [SerializeField] private TMP_Text _buildInfraButton = null;
    [SerializeField] private TMP_Text _infraCost = null;
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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        if (isUp)
        {
            // 탄소 농도 변화량 증가
            ++PlayManager.Instance[VariableShort.CarbonMovement];

            // 아래 버튼 활성화
            if (!_downButton.activeSelf)
            {
                _downButton.SetActive(true);
            }

            // 변화량이 인프라 수와 같을 때
            if (PlayManager.Instance[VariableByte.CarbonInfra] <= PlayManager.Instance[VariableShort.CarbonMovement])
            {
                _upButton.SetActive(false);
            }
        }
        else
        {
            // 탄소 농도 변화량 감소
            --PlayManager.Instance[VariableShort.CarbonMovement];

            // 위 버튼 활성화
            if (!_upButton.activeSelf)
            {
                _upButton.SetActive(true);
            }

            // 변화량이 인프라 수와 같을 때
            if (PlayManager.Instance[VariableByte.CarbonInfra] <= -PlayManager.Instance[VariableShort.CarbonMovement])
            {
                _downButton.SetActive(false);
            }
        }

        // 텍스트 업데이트
        OnLanguageChange();
    }


    public void BtnBuildInfra()
    {
        // 사용 불가
        if (!_infraBuildavailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 건설
        ++PlayManager.Instance[VariableByte.CarbonInfra];

        // 지출
        PlayManager.Instance[VariableLong.Funds] -= _buildingInfraCost;

        // 지출 애니메이션
        BottomBarLeft.Instance.SpendAnimation(BottomBarLeft.Displays.Fund);

        // 건설된 인프라 수 표시
        _numOfInfra.text = PlayManager.Instance[VariableByte.CarbonInfra].ToString();

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

    private void OnLanguageChange()
    {
        // 인프라 건설 비용
        _infraCost.text = $"{Language.Instance["인프라 건설 비용"]} {_buildingInfraCost.ToString()}";

        // 월간 변화량 표시
        _montlyMovementText.text = $"{(PlayManager.Instance[VariableShort.CarbonMovement] * Constants.CARBON_RATIO_MOVEMENT).ToString()}ppm/{Language.Instance["월"]}";

        // 월간 비용 표시
        _montlyCostText.text = $"{Language.Instance["비용"]} {PlayManager.Instance[VariableShort.CarbonMovement].ToString()}/{Language.Instance["월"]}";
    }


    private void Awake()
    {
        // 건설된 인프라 수 표시
        _numOfInfra.text = PlayManager.Instance[VariableByte.CarbonInfra].ToString();

        // 위,아래 버튼 활성화 여부
        if (PlayManager.Instance[VariableByte.CarbonInfra] <= PlayManager.Instance[VariableShort.CarbonMovement])
        {
            _upButton.SetActive(false);
        }
        if (PlayManager.Instance[VariableByte.CarbonInfra] <= -PlayManager.Instance[VariableShort.CarbonMovement])
        {
            _downButton.SetActive(false);
        }

        // 텍스트 한 번 업데이트
        OnLanguageChange();

        // 대리자 등록
        Language.OnLanguageChange += OnLanguageChange;
    }


    private void Update()
    {
        // 정보 표시
        _totalCarbonRatioText.text = UIString.Instance[VariableFloat.TotalCarbonRatio_ppm];
        _atmosphereText.text = UIString.Instance[VariableFloat.CarbonGasMass_Tt];
        _hydrosphereText.text = UIString.Instance[VariableFloat.CarbonLiquidMass_Tt];
        _lithosphereText.text = UIString.Instance[VariableFloat.CarbonSolidMass_Tt];
        _biosphereText.text = UIString.Instance[VariableFloat.CarbonLifeMass_Tt];

        // 인프라 건설 가능 여부
        if (_infraBuildavailable)
        {
            // 건설 비용이 없거나 255개가 됐을 때
            if (_buildingInfraCost > PlayManager.Instance[VariableLong.Funds] || PlayManager.Instance[VariableByte.CarbonInfra] >= byte.MaxValue)
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
