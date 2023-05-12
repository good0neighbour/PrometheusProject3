using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScreenLand : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [Header("소도시")]
    [SerializeField] private uint _smallCityCapacity = 0;
    [SerializeField] private ushort _smallCityCost = 0;

    [Header("중형도시")]
    [SerializeField] private uint _middleCityCapacity = 0;
    [SerializeField] private ushort _middleCityCost = 0;

    [Header("대도시")]
    [SerializeField] private uint _bigCityCapacity = 0;
    [SerializeField] private ushort _bigCityCost = 0;

    [Header("참조")]
    [SerializeField] private GameObject _buildCityBtn = null;
    [SerializeField] private GameObject _cityName = null;
    [SerializeField] private GameObject _cityBuildScreen = null;
    [SerializeField] private Image _smallCityButton = null;
    [SerializeField] private Image _middleCityButton = null;
    [SerializeField] private Image _bigCityButton = null;
    [SerializeField] private TMP_Text _popUpBuildCityText = null;
    [SerializeField] private TMP_Text _smallCityTitleText = null;
    [SerializeField] private TMP_Text _smallCityCapacityText = null;
    [SerializeField] private TMP_Text _smallCityCostText = null;
    [SerializeField] private TMP_Text _middleCityTitleText = null;
    [SerializeField] private TMP_Text _middleCityCapacityText = null;
    [SerializeField] private TMP_Text _middleCityCostText = null;
    [SerializeField] private TMP_Text _bigCityTitleText = null;
    [SerializeField] private TMP_Text _bigCityCapacityText = null;
    [SerializeField] private TMP_Text _bigCityCostText = null;
    [SerializeField] private TMP_InputField _cityNameInputField = null;

    private LandSlot _currentSlot = null;
    private Land _currentLand = null;
    private TMP_Text _cityNameText = null;
    private bool _isBuildAvailable = false;
    private ushort _currentCost = 0;
    private uint _currentCapacity = 0;



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 현재 창 닫기
        ScreenExplore.Instance.OpenLandScreen(false);
    }


    public void BtnBuildCity()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 활성화할 때마다 초기화
        _smallCityButton.color = Constants.BUTTON_UNSELECTED;
        _middleCityButton.color = Constants.BUTTON_UNSELECTED;
        _bigCityButton.color = Constants.BUTTON_UNSELECTED;
        _currentCost = 0;

        // 도시 건설 창 열기
        _cityBuildScreen.SetActive(true);
    }


    public void BtnPopUpBack()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 도시 건설 창 닫기
        _cityBuildScreen.SetActive(false);
    }


    public void BtnPopUpBuildCity()
    {
        // 사용 불가
        if (!_isBuildAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 비용 지출
        PlayManager.Instance[VariableLong.Funds] -= _currentCost;

        // 도시 이름
        string cityName = _cityNameInputField.text;

        // 도시 이름 업데이트
        _currentLand.CityName = cityName;

        // 슬롯 이름 업데이트
        _currentSlot.SlotNameUpdate(cityName);

        // 이전 화면 업데이트
        BuildCityBtnDisplay(false);

        // 도시 건설 창 닫기
        _cityBuildScreen.SetActive(false);
    }


    public void BtnCityList(int whatCity)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        switch (whatCity)
        {
            case 0:
                _smallCityButton.color = Constants.BUTTON_SELECTED;
                _middleCityButton.color = Constants.BUTTON_UNSELECTED;
                _bigCityButton.color = Constants.BUTTON_UNSELECTED;
                _currentCost = _smallCityCost;
                return;
            case 1:
                _smallCityButton.color = Constants.BUTTON_UNSELECTED;
                _middleCityButton.color = Constants.BUTTON_SELECTED;
                _bigCityButton.color = Constants.BUTTON_UNSELECTED;
                _currentCost = _middleCityCost;
                return;
            case 2:
                _smallCityButton.color = Constants.BUTTON_UNSELECTED;
                _middleCityButton.color = Constants.BUTTON_UNSELECTED;
                _bigCityButton.color = Constants.BUTTON_SELECTED;
                _currentCost = _bigCityCost;
                return;
        }
    }



    /* ==================== Private Methods ==================== */

    private void BuildCityBtnDisplay(bool display)
    {
        if (display)
        {
            _buildCityBtn.SetActive(true);
            _cityName.SetActive(false);
        }
        else
        {
            _cityNameText.text = _currentLand.CityName;
            _buildCityBtn.SetActive(false);
            _cityName.SetActive(true);
        }
    }


    private void Awake()
    {
        // 참조
        _cityNameText = _cityName.GetComponent<TMP_Text>();

        // 텍스트 지정
        _smallCityCapacityText.text = $"{Language.Instance["수용량"]}\n{_smallCityCapacity.ToString()}";
        _smallCityCostText.text = $"{Language.Instance["비용"]} {_smallCityCost.ToString()}";
        _middleCityCapacityText.text = $"{Language.Instance["수용량"]}\n{_middleCityCapacity.ToString()}";
        _middleCityCostText.text = $"{Language.Instance["비용"]} {_middleCityCost.ToString()}";
        _bigCityCapacityText.text = $"{Language.Instance["수용량"]}\n{_bigCityCapacity.ToString()}";
        _bigCityCostText.text = $"{Language.Instance["비용"]} {_bigCityCost.ToString()}";
    }


    private void OnEnable()
    {
        // 슬롯 정보를 가져온다.
        _currentSlot = ScreenExplore.Instance.CurrentSlot;

        // 토지 정보 가져온다.
        _currentLand = PlayManager.Instance.GetLand(_currentSlot.SlotNum);

        // 정보 표시
        BuildCityBtnDisplay(null == _currentLand.CityName);

        // 입력칸 비운다.
        _cityNameInputField.text = null;

        #region 비용 계산
        if (_smallCityCost > PlayManager.Instance[VariableLong.Funds])
        {
            _smallCityTitleText.color = Constants.TEXT_BUTTON_DISABLE;
            _smallCityCapacityText.color = Constants.TEXT_BUTTON_DISABLE;
            _smallCityCostText.color = Constants.TEXT_BUTTON_DISABLE;
        }
        else
        {
            _smallCityTitleText.color = Constants.WHITE;
            _smallCityCapacityText.color = Constants.WHITE;
            _smallCityCostText.color = Constants.WHITE;
        }
        if (_middleCityCost > PlayManager.Instance[VariableLong.Funds])
        {
            _middleCityTitleText.color = Constants.TEXT_BUTTON_DISABLE;
            _middleCityCapacityText.color = Constants.TEXT_BUTTON_DISABLE;
            _middleCityCostText.color = Constants.TEXT_BUTTON_DISABLE;
        }
        else
        {
            _middleCityTitleText.color = Constants.WHITE;
            _middleCityCapacityText.color = Constants.WHITE;
            _middleCityCostText.color = Constants.WHITE;
        }
        if (_bigCityCost > PlayManager.Instance[VariableLong.Funds])
        {
            _bigCityTitleText.color = Constants.TEXT_BUTTON_DISABLE;
            _bigCityCapacityText.color = Constants.TEXT_BUTTON_DISABLE;
            _bigCityCostText.color = Constants.TEXT_BUTTON_DISABLE;
        }
        else
        {
            _bigCityTitleText.color = Constants.WHITE;
            _bigCityCapacityText.color = Constants.WHITE;
            _bigCityCostText.color = Constants.WHITE;
        }
        #endregion
    }


    private void Update()
    {
        // 건설 가능 여부
        if (_currentCost == 0 || _cityNameInputField.text == string.Empty || _currentCost > PlayManager.Instance[VariableLong.Funds])
        {
            _isBuildAvailable = false;
            _popUpBuildCityText.color = Constants.TEXT_BUTTON_DISABLE;
        }
        else
        {
            _isBuildAvailable = true;
            _popUpBuildCityText.color = Constants.WHITE;
        }

        // 단축키 동작
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ScreenExplore.Instance.OpenLandScreen(false);
        }
    }
}
