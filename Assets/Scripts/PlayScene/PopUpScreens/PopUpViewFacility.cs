using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class PopUpViewFacility : TechTreeViewBase
{
    /* ==================== Variables ==================== */

    private TMP_Text[] _nodeIcons = null;
    private bool[] _enabled = null;
    private bool[] _adopted = null;
    private City _currentCity = null;



    /* ==================== Public Methods ==================== */

    public override void BtnAdopt()
    {
        // 사용 불가
        if (!IsAdoptAvailable)
        {
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // 부모 클래스의 동작 수행
        base.BtnAdopt();

        // 승인 애니메이션
        AdoptAnimation(PlayManager.Instance[VariableFloat.FacilitySupportRate]);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // 활성화 정보 전달
        _adopted[CurrentNode] = true;

        // 지지율 상승
        PlayManager.Instance[VariableFloat.FacilitySupportRate] += Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (100.0f < PlayManager.Instance[VariableFloat.FacilitySupportRate])
        {
            PlayManager.Instance[VariableFloat.FacilitySupportRate] = 100.0f;
        }

        // 지지율 상승 이내메이션
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.FacilitySupport);

        // 시설 수 증가
        ++_currentCity.NumOfFacility;

        // 시설 수익
        FacilityGains();

        // 노드 아이콘 변경
        _nodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // 다음 노드 활성화
        foreach (TechTrees.SubNode nextNodes in NextNodes[CurrentNode])
        {
            switch (nextNodes.Type)
            {
                case TechTreeType.Facility:
                    if (EnableCheck(nextNodes))
                    {
                        byte index = NodeIndex[nextNodes.NodeName];
                        _enabled[index] = true;
                        SetIcon(index);
                        NodeBtnObjects[index].SetActive(true);
                    }
                    break;
                case TechTreeType.Society:
                    // 사회 테크트리에는 행동하지 않는다.
                    break;
                default:
                    break;
            }
        }
    }


    protected override void OnFail()
    {
        // 지지율 감소
        PlayManager.Instance[VariableFloat.FacilitySupportRate] -= Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (0.0f > PlayManager.Instance[VariableFloat.FacilitySupportRate])
        {
            PlayManager.Instance[VariableFloat.FacilitySupportRate] = 0.0f;
        }

        // 지지율 애니메이션
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.FacilitySupport);
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


    protected override bool EnableCheck(TechTrees.SubNode nextNode)
    {
        // 이전 노드로 설정된 것
        TechTrees.SubNode[] requiredNodes = NodeData[NodeIndex[nextNode.NodeName]].Requirments;
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


    protected override string GetGainText()
    {
        StringBuilder result = new StringBuilder();
        result.Append($"[{Language.Instance["수익"]}]\n");

        TechTrees.Node node = NodeData[CurrentNode];
        if (0 < node.AnnualFund)
        {
            result.Append($"{Language.Instance["연간 자금"]} {node.AnnualFund.ToString()}\n");
        }
        if (0 < node.AnnualResearch)
        {
            result.Append($"{Language.Instance["연간 연구"]} {node.AnnualResearch.ToString()}\n");
        }
        if (0 < node.AnnualCulture)
        {
            result.Append($"{Language.Instance["연간 문화"]} {node.AnnualCulture.ToString()}\n");
        }
        if (0 < node.Stability)
        {
            result.Append($"{Language.Instance["안정도 증가"]} {node.AnnualCulture.ToString()}\n");
        }
        if (0 < node.PopulationMovement)
        {
            result.Append($"{Language.Instance["인구 증가"]}\n");
        }
        if (0 < node.Police)
        {
            result.Append($"{Language.Instance["범죄율 감소"]}\n");
        }
        if (0 < node.Health)
        {
            result.Append($"{Language.Instance["질병 감소"]}\n");
        }
        if (0 < node.Safety)
        {
            result.Append($"{Language.Instance["부상 감소"]}\n");
        }

        // 다음 잠금 해제
        List<TechTrees.SubNode> requirments = NextNodes[CurrentNode];
        if (0 < requirments.Count)
        {
            for (byte i = 0; i < requirments.Count; ++i)
            {
                switch (requirments[i].Type)
                {
                    case TechTreeType.Tech:
                        result.Append($"{Language.Instance["상용화 연구 가능"]} - {Language.Instance[requirments[i].NodeName]}\n");
                        break;
                    case TechTreeType.Thought:
                        result.Append($"{Language.Instance["사상 연구 가능"]} - {Language.Instance[requirments[i].NodeName]}\n");
                        break;
                    case TechTreeType.Society:
                        result.Append($"{Language.Instance["사회 채택 가능"]} - {Language.Instance[requirments[i].NodeName]}\n");
                        break;
                    default:
                        // 나머지는 표시하지 않는다.
                        break;
                }
            }
        }

        // 반환. 마지막 \n은 제거한다.
        return result.Remove(result.Length - 1, 1).ToString();
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 노드 승인 완료, 미완료 표시
    /// </summary>
    private void SetIcon(byte index)
    {
        if (_adopted[index])
        {
            _nodeIcons[index].text = Constants.FACILITY_ADOPTED;
        }
        else
        {
            _nodeIcons[index].text = Constants.FACILITY_UNADOPTED;
        }
    }


    /// <summary>
    /// 시설 수익
    /// </summary>
    private void FacilityGains()
    {
        _currentCity.PopulationMovementMultiply += NodeData[CurrentNode].PopulationMovement;
        _currentCity.AnnualFund += (short)(NodeData[CurrentNode].AnnualFund - NodeData[CurrentNode].Maintenance);
        _currentCity.AnnualResearch += NodeData[CurrentNode].AnnualResearch;
        _currentCity.InjurePosibility += NodeData[CurrentNode].Injure;
        _currentCity.Police += NodeData[CurrentNode].Police;
        _currentCity.Health += NodeData[CurrentNode].Health;
        _currentCity.Safety += NodeData[CurrentNode].Safety;
        _currentCity.StabilityAdd += NodeData[CurrentNode].Stability;
    }


    private void Awake()
    {
        // 노드 정보 가져오기
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Facility);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Facility);

        // 배열 생성
        byte length = (byte)NodeData.Length;
        _nodeIcons = new TMP_Text[length];
        
        BasicInitialize(length);

        // 참조
        for (byte i = 0; i < length; ++i)
        {
            _nodeIcons[i] = NodeBtnObjects[i].GetComponentInChildren<TMP_Text>();
        }
    }


    private void OnEnable()
    {
        // 현재 도시 정보
        _currentCity = ScreenCity.Instance.CurrentCity;

        // 배열 참조
        _enabled = _currentCity.FacilityEnabled;
        _adopted = _currentCity.FacilityAdopted;

        // 활성화, 비활성화
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // 사용 가능 여부
            if (_enabled[i])
            {
                // 사용 가능
                NodeBtnObjects[i].SetActive(true);

                // 승인 완료, 미완료
                SetIcon(i);
            }
            else
            {
                TechTrees.SubNode[] requiredNode = NodeData[i].Requirments;
                bool enable = true;

                for (byte j = 0; j < requiredNode.Length; ++j)
                {
                    switch (requiredNode[j].Type)
                    {
                        case TechTreeType.Facility:
                            if (!_adopted[NodeIndex[requiredNode[j].NodeName]])
                            {
                                // 사용 불가
                                enable = false;
                            }
                            break;
                        default:
                            if (1.0f > Adopted[(int)requiredNode[j].Type][NodeIndex[requiredNode[j].NodeName]])
                            {
                                // 사용 불가
                                enable = false;
                            }
                            break;
                    }

                    if (!enable)
                    {
                        break;
                    }
                }

                if (enable)
                {
                    // 승인 완료, 미완료
                    SetIcon(i);

                    // 활성화
                    NodeBtnObjects[i].SetActive(true);
                }
                else
                {
                    //  비활성화
                    NodeBtnObjects[i].SetActive(false);
                }
            }
        }
    }
}
