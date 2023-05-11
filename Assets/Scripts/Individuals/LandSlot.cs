using UnityEngine;

public class LandSlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    private ushort _slotNum = 0;



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 현재 슬롯 번호 설정
        ScreenExplore.Instance.CurrentSlot = _slotNum;

        // 토지 화면 활성화
        ScreenExplore.Instance.OpenLandScreen(true);
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        _slotNum = PlayManager.Instance[VariableUshort.LandNum];
    }
}
