using UnityEngine;
using TMPro;

public class ScreenCity : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _cityNameText = null;
    [SerializeField] private GameObject _popUpFacilityScreen = null;

    private City _currentCity = null;

    public static ScreenCity Instance
    {
        get;
        private set;
    }

    public CitySlot CurrentSlot
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
            // ���� ����
            _currentCity = value;

            // ���� ���� �� ����
            _cityNameText.text = value.CityName;
        }
    }



    /* ==================== Public Methods ==================== */

    public void BtnFacility()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // â ����
        _popUpFacilityScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // �ʱ� ���� ȭ��
        CurrentCity = PlayManager.Instance.GetCity(0);
    }
}
