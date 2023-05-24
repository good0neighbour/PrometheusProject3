using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenCity : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("참조")]
    [SerializeField] private TMP_Text _cityNameText = null;
    [SerializeField] private TMP_Text _capacityText = null;
    [SerializeField] private TMP_Text _populationText = null;
    [SerializeField] private TMP_Text _populationMovementText = null;
    [SerializeField] private TMP_Text _annualFundText = null;
    [SerializeField] private TMP_Text _annualResearchText = null;
    [SerializeField] private TMP_Text _crimeText = null;
    [SerializeField] private TMP_Text _diseaseText = null;
    [SerializeField] private TMP_Text _injureText = null;
    [SerializeField] private TMP_Text _stabilityText = null;
    [SerializeField] private Image _cityImage = null;
    [SerializeField] private Image _supportRateImage = null;
    [SerializeField] private PopUpScreenTechTree _popUpTechTreeScreen = null;
    [SerializeField] private Sprite[] _citySprites = new Sprite[4];

    private City _currentCity = null;

    public static ScreenCity Instance
    {
        get;
        private set;
    }

    public SlotCity CurrentSlot
    {
        get;
        set;
    }

    public City CurrentCity
    {
        get
        {
            return _currentCity;
        }
        set
        {
            // 도시 변경
            _currentCity = value;

            // 도시 변경 시 동작
            _cityNameText.text = value.CityName;
            _capacityText.text = _currentCity.Capacity.ToString();
            CityImageUpdate();
            OnMonthChange();
        }
    }



    /* ==================== Public Methods ==================== */

    public void BtnFacility()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 창 연다.
        _popUpTechTreeScreen.ActiveThis(TechTreeType.Facility);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;

        // 이 창 닫는다.
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 도시 이미지 업데이트
    /// </summary>
    public void CityImageUpdate()
    {
        if (10 > _currentCity.NumOfFacility)
        {
            _cityImage.sprite = _citySprites[0];
        }
        else if (20 > _currentCity.NumOfFacility)
        {
            _cityImage.sprite = _citySprites[1];
        }
        else if (30 > _currentCity.NumOfFacility)
        {
            _cityImage.sprite = _citySprites[2];
        }
        else
        {
            _cityImage.sprite = _citySprites[3];
        }
    }



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        // 활성화 상태가 아니면 반환
        if (!gameObject.activeSelf)
        {
            return;
        }

        _populationText.text = _currentCity.Population.ToString("0");
        _populationMovementText.text = (_currentCity.PopulationMovement * 12.0f).ToString("0");
        _annualFundText.text = _currentCity.AnnualFund.ToString();
        _annualResearchText.text = _currentCity.AnnualResearch.ToString();
        _crimeText.text = $"{_currentCity.Crime.ToString("F2")}%";
        _diseaseText.text = $"{_currentCity.Disease.ToString("F2")}%";
        _injureText.text = $"{_currentCity.Injure.ToString("F2")}%";
        _stabilityText.text = _currentCity.Stability.ToString("0");
    }


    private void Awake()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        // 초기 도시 화면
        CurrentCity = PlayManager.Instance.GetCity(0);

        // 대리자 등록
        PlayManager.OnMonthChange += OnMonthChange;
    }


    private void OnEnable()
    {
        CityImageUpdate();
        OnMonthChange();
    }


    private void Update()
    {
        _supportRateImage.fillAmount = PlayManager.Instance[VariableFloat.FacilitySupportRate] * 0.01f;

        // 단축키 동작
#if PLATFORM_STANDALONE_WIN
        // 키보드 동작
        if (Input.GetKeyUp(KeyCode.S))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex + 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
#endif
        // PC, 모바일 공용
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
    }
}
