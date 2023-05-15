using System.Collections.Generic;
using UnityEngine;

public class PopUpScreenTech : TechTreeBase
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
        //AdoptAnimation(PlayManager.Instance[VariableFloat.ResearchSupportRate]);
        AdoptAnimation(75.0f);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 활성화 정보 전달

        // 노드 아이콘 변경
        NodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;

        // 다음 노드 활성화
    }


    protected override void OnFail()
    {
        
    }


    protected override bool IsUnadopted()
    {
        bool result = (0.0f >= Adopted[(int)TechTreeType.Tech][NodeIndex[NodeData[CurrentNode].NodeName]]);
        if (result)
        {
            AdoptBtn.text = Language.Instance["승인 완료"];
        }
        else
        {
            AdoptBtn.text = Language.Instance["승인"];
        }
        return !result;
    }



    /* ==================== Private Methods ==================== */

    protected void Awake()
    {
        // 노드 정보 가져오기
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Tech);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Tech);

        BasicInitialize();
    }
}
