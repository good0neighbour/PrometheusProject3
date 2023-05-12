using UnityEngine;
using TMPro;
using System.Text;

public class LandSlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _resourcesField = null;

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
        // 토지 구분 저장
        SlotNum = PlayManager.Instance[VariableUshort.LandNum];

        // 텍스트 관련 업데이트
        OnLanguageChange();

        // 대리자에 등록
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

        // 자원 목록 업데이트를 위한 준비
        StringBuilder resourceString = new StringBuilder(null);
        Land land = PlayManager.Instance.GetLand(SlotNum);

        // 각 자원은 0보다 클 때만 표시한다.
        // 언어 변경은 플레이 중 잘 일어나지 않을 것이기 때문에 자원 값은 멤버변수로 저장하지 않고 그때그때 참조해온다.
        if (0 < land.Resources.Iron)
        {
            resourceString.Append($"{Language.Instance["철"]} {land.Resources.Iron.ToString()}");
        }
        if (0 < land.Resources.Nuke)
        {
            // 이전에 표시한 자원 정보가 없으면 띄어쓰기할 필요 없다.
            if (string.IsNullOrEmpty(resourceString.ToString()))
            {
                resourceString.Append("\n");
            }
            resourceString.Append($"{Language.Instance["핵물질"]} {land.Resources.Nuke.ToString()}");
        }

        // 자원 정보 표시
        _resourcesField.text = resourceString.ToString();
    }
}
