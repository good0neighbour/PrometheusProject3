using System.Collections.Generic;

public class PopUpScreenFacility : TechTreeBase
{
    /* ==================== Variables ==================== */

    private City _currentCity = null;
    private bool[] _enabled = null;
    private bool[] _adopted = null;



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
        AdoptAnimation(75.0f);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // 활성화 정보 전달
        _adopted[CurrentNode] = true;

        // 노드 아이콘 변경
        NodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;

        // 다음 노드 활성화
        List<TechTrees.Node.SubNode> nextNodes = NextNodes[CurrentNode];
        for (byte i = 0; i < nextNodes.Count; ++i)
        {
            switch (nextNodes[i].Type)
            {
                case TechTreeType.Facility:
                    if (EnableCheck(nextNodes[i]))
                    {
                        byte index = NodeIndex[nextNodes[i].NodeName];
                        _enabled[index] = true;
                        NodeBtnObjects[index].SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }


    protected override void OnFail()
    {

    }


    protected override bool IsUnadopted()
    {
        bool result = _adopted[NodeIndex[NodeData[CurrentNode].NodeName]];
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

    protected override bool EnableCheck(TechTrees.Node.SubNode nextNode)
    {
        // 이전 노드로 설정된 것
        TechTrees.Node.SubNode[] requiredNodes = NodeData[NodeIndex[nextNode.NodeName]].Requirments;
        for (int i = 0; i < requiredNodes.Length; i++)
        {
            // 모두 승인된 것이 아니면 거짓 반환
            switch (requiredNodes[i].Type)
            {
                case TechTreeType.Facility:
                    if (!_adopted[NodeIndex[requiredNodes[i].NodeName]])
                    {
                        return false;
                    }
                    break;
                default:
                    if (1.0f > Adopted[(int)requiredNodes[i].Type][NodeIndex[requiredNodes[i].NodeName]])
                    {
                        return false;
                    }
                    break;
            }
        }

        // 모두 승인 됐으면 참 반환
        return true;
    }


    private void Awake()
    {
        // 노드 정보 가져오기
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Facility);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Facility);

        BasicInitialize();
    }


    private void OnEnable()
    {
        // 현재 도시 정보
        _currentCity = ScreenCity.Instance.CurrentCity;

        // 배열 참조
        _enabled = _currentCity.GetFacilityEnabled();
        _adopted = _currentCity.GetFacilityAdopted();

        // 활성화, 비활성화
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // 사용 가능 여부
            NodeBtnObjects[i].SetActive(_enabled[i]);

            // 승인 완료, 미완료
            if (_adopted[i])
            {
                NodeIcons[i].text = Constants.FACILITY_ADOPTED;
            }
            else
            {
                NodeIcons[i].text = Constants.FACILITY_UNADOPTED;
            }
        }
    }
}
