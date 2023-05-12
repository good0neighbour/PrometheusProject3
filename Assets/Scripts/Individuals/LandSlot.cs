using UnityEngine;
using TMPro;

public class LandSlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;

    private bool _isCityExists = false;

    public ushort SlotNum
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 현재 슬롯 번호 설정
        ScreenExplore.Instance.CurrentSlot = this;

        // 토지 화면 활성화
        ScreenExplore.Instance.OpenLandScreen(true);
    }


    /// <summary>
    /// 슬롯 초기화
    /// </summary>
    public void SlotInitialize()
    {
        SlotNum = PlayManager.Instance[VariableUshort.LandNum];
        _name.text = $"{Language.Instance["토지"]}{(SlotNum + 1).ToString()}";

        Language.OLC += OnLanguageChange;
    }


    /// <summary>
    /// 슬롯 이름 업데이트
    /// </summary>
    public void SlotNameUpdate(string cityName)
    {
        _name.text = cityName;
        _isCityExists = true;
    }



    /* ==================== Private Methods ==================== */

    private void OnLanguageChange()
    {
        // 도시가 없을 때만
        if (!_isCityExists)
        {
            _name.text = $"{Language.Instance["토지"]}{(SlotNum + 1).ToString()}";
        }
    }
}
