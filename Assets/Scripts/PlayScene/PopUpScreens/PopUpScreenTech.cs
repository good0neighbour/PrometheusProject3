using System.Collections.Generic;
using UnityEngine;

public class PopUpScreenTech : TechTreeBase
{
    /* ==================== Variables ==================== */

    private List<byte>[] _nextNodes = null;
    private bool[] _unlocked = null;



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
        _unlocked[CurrentNode] = true;
        

        // 상태 메세지
        StatusText.color = Constants.WHITE;
        StatusText.text = Language.Instance["승인 완료"];

        // 승인 버튼 사용 불가
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;
    }

    protected override void OnFail()
    {
        throw new System.NotImplementedException();
    }



    /* ==================== Private Methods ==================== */

    protected void Awake()
    {
        // 노드 정보 가져오기
        NodeData = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Tech);

        // 다음 노드 가변 배열 생성
        byte length = (byte)NodeData.Length;
        _nextNodes = new List<byte>[length];

        // 부모의 함수 호출
        BasicInitialize(length);

        // 다음 노드 등록
        for (byte i = 0; i < length; ++i)
        {
            // 이전 노드 정보
            TechTrees.Node.RequirmentNode[] requiredNodes = NodeData[i].Requirments;

            // 다음 노드로 등록
            for (byte j = 0; j < requiredNodes.Length; ++j)
            {
                // 요구 조건이 기술일 때만 수행할 것
                switch (requiredNodes[j].Type)
                {
                    case TechTreeType.Tech:
                        byte index = NodeIndex[requiredNodes[j].NodeName];

                        // 가변배열 생성한 적 없으면 생성
                        if (null == _nextNodes[index])
                        {
                            _nextNodes[index] = new List<byte>();
                        }

                        // 이전 노드로 설정된 것에 현재 노드를 다음 것으로 등록
                        _nextNodes[index].Add(i);

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
