using UnityEngine;
using TMPro;

public class ScreenCity : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("참조")]
    [SerializeField] private TMP_Text _cityNameText = null;
    [SerializeField] private PopUpScreenTechTree _popUpTechTreeScreen = null;

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
        }
    }



    /* ==================== Public Methods ==================== */

    public void BtnFacility()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 창 열기
        _popUpTechTreeScreen.ActiveThis(TechTreeType.Facility);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        // 초기 도시 화면
        CurrentCity = PlayManager.Instance.GetCity(0);
    }
}
