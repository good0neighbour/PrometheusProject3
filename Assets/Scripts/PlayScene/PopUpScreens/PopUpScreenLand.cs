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
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� �Ǽ�
        _currentLand.CityName = "�Ǽ� ��.";
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
        // ���� ���� �����´�.
        _currentLand = PlayManager.Instance.GetLand(ScreenExplore.Instance.CurrentSlot);

        // ���� ǥ��
        BuildCityBtnDisplay(null == _currentLand.CityName);
    }


    private void Update()
    {
        // ����Ű ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ScreenExplore.Instance.OpenLandScreen(false);
        }
    }
}
