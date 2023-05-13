using UnityEngine;
using TMPro;

public class PopUpScreenFacility : TechTreeBase
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    public override void BtnAdopt()
    {
        // 사용 불가
        if (!IsAdoptAvailable)
        {
            return;
        }

        // 부모 클래스의 동작 수행
        base.BtnAdopt();

        // 승인 애니메이션
        //AdoptAnimation(PlayManager.Instance[VariableFloat.FacilitySupportRate]);
        AdoptAnimation(50.0f);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 노드 아이콘 변경
        NodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;

        // 상태 메세지
        StatusText.color = Constants.WHITE;
        StatusText.text = Language.Instance["승인 완료"];

        // 승인 버튼 사용 불가
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;
    }


    protected override void OnFail()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Failed);

        // 상태 메세지
        StatusText.color = Constants.FAIL_TEXT;
        StatusText.text = Language.Instance["승인 실패"];

        // 승인 버튼 사용 가능
        IsAdoptAvailable = true;
    }



    /* ==================== Private Methods ==================== */
}
