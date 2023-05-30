using TMPro;
using UnityEngine;

public class ScreenPhoto : PlayScreenBase, IRequest
{
    /* ==================== Variables ==================== */

    [Header("비용")]
    [SerializeField] private ushort _requestCost = 0;

    [Header("참조")]
    [SerializeField] private TMP_Text _requestButton = null;
    [SerializeField] private TMP_Text _costText = null;
    [SerializeField] private TMP_Text _lifePosibilityText = null;
    [SerializeField] private TMP_Text _lifeStabilityText = null;
    [SerializeField] private TMP_Text _airPressureText = null;
    [SerializeField] private TMP_Text _temperatureText = null;
    [SerializeField] private TMP_Text _waterText = null;

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
        // 사용 불가
        if (!_requestAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 비용 지출
        PlayManager.Instance[VariableLong.Funds] -= _requestCost;

        // 지출 애니메이션
        BottomBarLeft.Instance.SpendAnimation(BottomBarLeft.Displays.Fund);

        // 종자 도착 카운트 다운
        PlayManager.Instance[VariableByte.PhotoRequest] = Constants.REQUEST_TIME;

        // 요청 진행
        RequestCountDown();

        // 메세지
        MessageBox.Instance.EnqueueMessage(Language.Instance[
            "{생물} 종자를 요청했습니다. {0}개월 후에 지구로부터 도착 예정입니다."
            ], Language.Instance["광합성 생물"], Constants.REQUEST_TIME.ToString());
    }


    /// <summary>
    /// 요청 진행
    /// </summary>
    public void RequestCountDown()
    {
        // 대리자 등록
        PlayManager.OnMonthChange += SeedDelivery;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 다른 단위로 표시하기 위해 만들었다.
    /// </summary>
    private string GetAirPressure()
    {
        // 값이 바뀌었거나 문자열을 생성한 적 없을 때
        if (_airPressure != PlayManager.Instance[VariableFloat.TotalAirPressure_hPa] || string.IsNullOrEmpty(_airPressureString))
        {
            // 현재 값 저장
            _airPressure = PlayManager.Instance[VariableFloat.TotalAirPressure_hPa];

            // 단위 표시
            float result = ((1.0f - Mathf.Abs(_airPressure / Constants.EARTH_AIR_PRESSURE - 1.0f)) * 100.0f);
            if (0.0f > result)
            {
                result = 0.0f;
            }
            _airPressureString = $"{result.ToString("0")}%";
        }

        // 반환
        return _airPressureString;
    }


    private string GetTemperature()
    {
        // 값이 바뀌었거나 문자열을 생성한 적 없을 때
        if (_temperature != PlayManager.Instance[VariableFloat.TotalTemperature_C] || string.IsNullOrEmpty(_temperatureString))
        {
            // 현재 값 저장
            _temperature = PlayManager.Instance[VariableFloat.TotalTemperature_C];

            // 단위 표시
            float result = ((1.0f - Mathf.Abs(_temperature / Constants.EARTH_TEMPERATURE - 1.0f)) * 100.0f);
            if (0.0f > result)
            {
                result = 0.0f;
            }
            _temperatureString = $"{result.ToString("0")}%";
        }

        // 반환
        return _temperatureString;
    }


    private string GetPosibility()
    {
        // 값이 바뀌었거나 문자열을 생성한 적 없을 때
        if (_posibility != PlayManager.Instance[VariableFloat.PhotoLifePosibility] || string.IsNullOrEmpty(_posibilityString))
        {
            if (0.0f < PlayManager.Instance[VariableFloat.PhotoLifePosibility])
            {
                // 현재 값 저장
                _posibility = PlayManager.Instance[VariableFloat.PhotoLifePosibility];

                // 단위 표시
                _posibilityString = $"{_posibility.ToString("0")}%";
            }
            else if (0.0f != _posibility)
            {
                _posibility = 0.0f;
                _posibilityString = "0%";
            }
        }

        // 반환
        return _posibilityString;
    }


    private string GetStability()
    {
        // 값이 바뀌었거나 문자열을 생성한 적 없을 때
        if (_stability != PlayManager.Instance[VariableFloat.PhotoLifeStability] || string.IsNullOrEmpty(_stabilityString))
        {
            // 현재 값 저장
            _stability = PlayManager.Instance[VariableFloat.PhotoLifeStability];

            // 단위 표시
            _stabilityString = $"{_stability.ToString("0")}";
        }

        // 반환
        return _stabilityString;
    }


    private string GetWater()
    {
        // 값이 바뀌었거나 문자열을 생성한 적 없을 때
        if (_water != PlayManager.Instance[VariableFloat.WaterLiquid_PL] || string.IsNullOrEmpty(_waterString))
        {
            // 현재 값 저장
            _water = PlayManager.Instance[VariableFloat.WaterLiquid_PL];

            // 단위 표시
            _waterString = $"{(_water * Constants.E_3).ToString("F2")}EL";
        }

        // 반환
        return _waterString;
    }


    /// <summary>
    /// 종자 배달 중
    /// </summary>
    private void SeedDelivery()
    {
        --PlayManager.Instance[VariableByte.PhotoRequest];

        switch (PlayManager.Instance[VariableByte.PhotoRequest])
        {
            case 0:
                // 안정도 1만큼 증가
                ++PlayManager.Instance[VariableFloat.PhotoLifeStability];

                // 대리자에서 제거
                PlayManager.OnMonthChange -= SeedDelivery;

                // 메세지
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "지구로부터 {생물} 종자가 도착했습니다. 증식을 시작합니다."
                    ], Language.Instance["광합성 생물"]);
                return;

            default:
                return;
        }
    }


    private void OnLanguageChange()
    {
        // 비용 표시
        _costText.text = $"{Language.Instance["비용"]} {_requestCost.ToString()}";
    }


    private void Awake()
    {
        // 텍스트 한 번 업데이트
        OnLanguageChange();

        // 대리자 등록
        Language.OnLanguageChange += OnLanguageChange;
    }


    private void Update()
    {
        // 정보 표시
        _lifePosibilityText.text = GetPosibility();
        _lifeStabilityText.text = GetStability();
        _airPressureText.text = GetAirPressure();
        _temperatureText.text = GetTemperature();
        _waterText.text = GetWater();

        // 요청 가능 여부
        if (_requestAvailable)
        {
            // 요청 비용이 없됐을 때
            if (_requestCost > PlayManager.Instance[VariableLong.Funds])
            {
                _requestAvailable = false;
                _requestButton.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
        // 요청 비용이 생겼을 때
        else if (_requestCost < PlayManager.Instance[VariableLong.Funds])
        {
            _requestAvailable = true;
            _requestButton.color = Constants.WHITE;
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
