using UnityEngine;
using TMPro;

public class ScreenCity : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("����")]
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
        _popUpTechTreeScreen.ActiveThis(TechTreeType.Facility);
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
