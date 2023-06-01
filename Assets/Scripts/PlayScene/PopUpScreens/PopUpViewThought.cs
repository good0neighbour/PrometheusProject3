using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class PopUpViewThought : TechTreeViewBase, IActivateFirst
{
    /* ==================== Variables ==================== */

    private TMP_Text[] _titleTexts = null;
    private TMP_Text[] _remainTexts = null;
    private Image[] _progreesionImages = null;
    private List<byte> _onProgress = new List<byte>();



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
        AdoptAnimation(PlayManager.Instance[VariableFloat.ResearchSupportRate]);
    }


    public void Activate()
    {
        // 노드 정보 가져오기
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Thought);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Thought);

        // 배열 생성
        byte length = (byte)NodeData.Length;
        _titleTexts = new TMP_Text[length];
        _remainTexts = new TMP_Text[length];
        _progreesionImages = new Image[length];

        // UIString 사용 가능 상태로 만든다.
        UIString.Instance.ThoughtInitialize(length);

        BasicInitialize(length);

        for (byte i = 0; i < length; ++i)
        {
            // 참조
            Transform node = NodeBtnObjects[i].transform;
            _titleTexts[i] = node.Find("TextTitle").GetComponent<TMP_Text>();
            _remainTexts[i] = node.transform.Find("TextRemain").GetComponent<TMP_Text>();
            _progreesionImages[i] = node.transform.Find("ImageProgressionBackground").Find("ImageProgression").GetComponent<Image>();

            // 연구 진행 중이었던 것 가변 배열에 추가
            if (0.0f < Adopted[(int)TechTreeType.Thought][i] && 1.0f > Adopted[(int)TechTreeType.Thought][i])
            {
                _onProgress.Add(i);
            }

            // 비활성화할 것
            TechTrees.SubNode[] requiredNodes = NodeData[i].Requirments;
            for (byte j = 0; j < requiredNodes.Length; ++j)
            {
                // 한 개라도 완료됐으면 활성화
                if (1.0f <= Adopted[(int)requiredNodes[j].Type][NodeIndex[requiredNodes[j].NodeName]])
                {
                    NodeBtnObjects[i].SetActive(true);
                    break;
                }
                else
                {
                    // 일단 비활성화
                    NodeBtnObjects[i].SetActive(false);
                }
            }

            // 여기서 ProgressionValue는 연구 속도로 쓴다.
            if (0.0f >= NodeData[i].ProgressionValue)
            {
                // 0이면 0.3로 바꾼다.
                NodeData[i].ProgressionValue = 0.3f;
            }
        }

        // 텍스트 업데이트
        OnLanguageChange();

        // 대리자 등록
        Language.OnLanguageChange += OnLanguageChange;
        PlayManager.OnPlayUpdate += TechResearchProgress;
    }


    /// <summary>
    /// 연구 진행 중 목록 가져온다.
    /// </summary>
    public List<byte> GetProgressionList()
    {
        return _onProgress;
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // 활성화 정보 전달
        Adopted[(int)TechTreeType.Thought][CurrentNode] = 0.0001f;

        // 지지율 상승
        PlayManager.Instance[VariableFloat.ResearchSupportRate] += Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (100.0f < PlayManager.Instance[VariableFloat.ResearchSupportRate])
        {
            PlayManager.Instance[VariableFloat.ResearchSupportRate] = 100.0f;
        }

        // 지지율 상승 이내메이션
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.ResearchSupport);

        // 연구 진행중인 리스트에 추가
        _onProgress.Add(CurrentNode);
    }


    protected override void OnFail()
    {
        // 지지율 감소
        PlayManager.Instance[VariableFloat.ResearchSupportRate] -= Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (0.0f > PlayManager.Instance[VariableFloat.ResearchSupportRate])
        {
            PlayManager.Instance[VariableFloat.ResearchSupportRate] = 0.0f;
        }

        // 지지율 애니메이션
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.ResearchSupport);
    }


    protected override bool IsUnadopted()
    {
        bool result = (0.0f < Adopted[(int)TechTreeType.Thought][NodeIndex[NodeData[CurrentNode].NodeName]]);
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

        // 다음 잠금 해제
        List<TechTrees.SubNode> requirments = NextNodes[CurrentNode];
        if (0 < requirments.Count)
        {
            for (byte i = 0; i < requirments.Count; ++i)
            {
                switch (requirments[i].Type)
                {
                    case TechTreeType.Facility:
                        result.Append($"{Language.Instance["시설 사용 가능"]} - {Language.Instance[requirments[i].NodeName]}\n");
                        break;
                    case TechTreeType.Tech:
                        result.Append($"{Language.Instance["상용화 연구 가능"]} - {Language.Instance[requirments[i].NodeName]}\n");
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
    /// 노드 표시 업데이트
    /// </summary>
    private void NodeUpdate(byte index)
    {
        // 남은 개월
        _remainTexts[index].text = UIString.Instance.GetRemainString(TechTreeType.Thought, index, NodeData);

        // 진행 정도 이미지
        _progreesionImages[index].fillAmount = Adopted[(int)TechTreeType.Thought][index];
    }


    /// <summary>
    /// 연구 진행
    /// </summary>
    private void TechResearchProgress()
    {
        for (byte i = 0; i < _onProgress.Count; ++i)
        {
            byte nodeNum = _onProgress[i];

            // 연구 진행
            Adopted[(int)TechTreeType.Thought][nodeNum] += NodeData[nodeNum].ProgressionValue * Constants.MONTHLY_MULTIPLY * PlayManager.Instance.GameSpeed * Time.deltaTime;

            // 연구 완료
            if (1.0f <= Adopted[(int)TechTreeType.Thought][nodeNum])
            {
                // 다음 노드 활성화
                List<TechTrees.SubNode> nextNodes = NextNodes[nodeNum];
                for (byte j = 0; j < nextNodes.Count; ++j)
                {
                    switch (nextNodes[j].Type)
                    {
                        case TechTreeType.Thought:
                            // 다음 사상은 연구 완료 시점에서 조건 없이 전부 활성화한다.
                            byte index = NodeIndex[nextNodes[j].NodeName];
                            NodeBtnObjects[index].SetActive(true);
                            NodeUpdate(index);
                            break;
                        case TechTreeType.Society:
                            // 사회 테크트리에는 행동하지 않는다.
                            break;
                        default:
                            // 다른 테크트리는 활성화 여부를 해당 테크트리가 스스로 결정한다.
                            break;
                    }
                }

                // 진행 중인 연구 리스트에서 제외
                _onProgress.Remove(nodeNum);

                // 메세지
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{기술} 연구가 완료됐습니다."
                    ], Language.Instance[NodeData[nodeNum].NodeName]);

                // 수정된 가변 배열 인덱스에 맞춘다.
                --i;
            }
        }
    }


    private void OnLanguageChange()
    {
        for (byte i = 0; i < _titleTexts.Length; ++i)
        {
            // 노드 이름. 
            _titleTexts[i].text = Language.Instance[NodeData[i].NodeName];
        }
    }


    private void OnEnable()
    {
        // 활성화, 비활성화
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // 이미 활성화 된 것
            if (NodeBtnObjects[i].activeSelf)
            {
                NodeUpdate(i);
            }
            // 비활성화인 것
            else
            {
                TechTrees.SubNode[] requiredNode = NodeData[i].Requirments;
                bool enable = true;

                for (byte j = 0; j < requiredNode.Length; ++j)
                {
                    if (1.0f > Adopted[(int)requiredNode[j].Type][NodeIndex[requiredNode[j].NodeName]])
                    {
                        // 사용 불가
                        enable = false;
                    }

                    if (!enable)
                    {
                        break;
                    }
                }

                // 사용 가능한 경우
                if (enable)
                {
                    NodeBtnObjects[i].SetActive(true);
                    NodeUpdate(i);
                }
            }
        }
    }
}
