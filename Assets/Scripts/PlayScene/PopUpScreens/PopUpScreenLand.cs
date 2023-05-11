using UnityEngine;
using TMPro;

public class PopUpScreenLand : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _buildCityBtn = null;
    [SerializeField] private GameObject _cityName = null;

    private Land _currentLand = null;
    private TMP_Text _cityNameText = null;



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        ScreenExplore.Instance.OpenLandScreen(false);
    }


    public void BtnBuildCity()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 도시 건설
        _currentLand.CityName = "건설 됨.";
        BuildCityBtnDisplay(false);
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
        _cityNameText = _cityName.GetComponent<TMP_Text>();
    }


    private void OnEnable()
    {
        // 토지 정보 가져온다.
        _currentLand = PlayManager.Instance.GetLand(ScreenExplore.Instance.CurrentSlot);

        // 정보 표시
        BuildCityBtnDisplay(null == _currentLand.CityName);
    }


    private void Update()
    {
        // 단축키 동작
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ScreenExplore.Instance.OpenLandScreen(false);
        }
    }
}
