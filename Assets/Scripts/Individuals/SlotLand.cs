using UnityEngine;
using TMPro;
using System.Text;

public class SlotLand : MonoBehaviour
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
    public void SlotInitialize(ushort landNum)
    {
        // 토지 구분 저장
        SlotNum = landNum;

        // 도시 건설 여부 확인. 저장된 게임을 불러온 경우 필요.
        _isCityExists = (null != PlayManager.Instance.GetLand(SlotNum).CityName);

        // 텍스트 관련 업데이트
        OnLanguageChange();

        // 대리자에 등록
        Language.OnLanguageChange += OnLanguageChange;
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
        // 도시 있으면 도시 이름으로 업데이트
        else
        {
            _name.text = PlayManager.Instance.GetLand(SlotNum).CityName;
        }

        // 자원 목록 업데이트를 위한 준비
        StringBuilder resourceString = new StringBuilder(null);
        byte[] land = PlayManager.Instance.GetLand(SlotNum).Resources;

        // 언어 변경은 플레이 중 잘 일어나지 않을 것이기 때문에 자원 값은 멤버변수로 저장하지 않고 그때그때 참조해온다.
        for (ResourceType i = 0; i < ResourceType.End; ++i)
        {
            // 문자열 참조 가져오기
            string text;
            switch (i)
            {
                case ResourceType.Iron:
                    text = Language.Instance["철"];
                    break;
                case ResourceType.Nuke:
                    text = Language.Instance["핵물질"];
                    break;
                case ResourceType.Jewel:
                    text = Language.Instance["보석"];
                    break;
                default:
                    Debug.LogError($"LandSlot - ResourceType 수정 요망. OnLanguageChange()");
                    return;
            }

            // 각 자원은 0보다 클 때만 표시한다.
            if (0 < land[(int)i])
            {
                // 이전에 표시한 자원 정보가 없으면 띄어쓰기할 필요 없다.
                if (!string.IsNullOrEmpty(resourceString.ToString()))
                {
                    resourceString.Append("\n");
                }
                resourceString.Append($"{text} {land[(int)i].ToString()}");
            }
        }

        // 자원 정보 표시
        _resourcesField.text = resourceString.ToString();
    }
}
