using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpViewThought : TechTreeBase, IActivateFirst
{
    /* ==================== Variables ==================== */

    private TMP_Text[] _titleTexts = null;
    private TMP_Text[] _remainTexts = null;
    private Image[] _progreesionImages = null;
    private List<byte> _onProgress = new List<byte>();
    private float _researchSpeedmult = 1.0f / Constants.MONTH_TIMER;



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
            if (0.0f < Adopted[(int)TechTreeType.Thought][i])
            {
                _onProgress.Add(i);
            }
        }

        // 비활성화할 것
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // 요구 조건 확인
            TechTrees.Node.SubNode[] requiredNodes = NodeData[i].Requirments;
            for (byte j = 0; j < requiredNodes.Length; ++j)
            {
                // 한 개라도 승인됐으면 활성화
                if (0.0f < Adopted[(int)requiredNodes[j].Type][NodeIndex[requiredNodes[j].NodeName]])
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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 활성화 정보 전달
        Adopted[(int)TechTreeType.Thought][CurrentNode] = 0.0001f;

        // 연구 진행중인 리스트에 추가
        _onProgress.Add(CurrentNode);
    }


    protected override void OnFail()
    {

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
            // 연구 진행
            Adopted[(int)TechTreeType.Thought][_onProgress[i]] += NodeData[_onProgress[i]].ProgressionPerMonth * _researchSpeedmult * PlayManager.Instance.GameSpeed * Time.deltaTime;

            // 연구 완료
            if (1.0f <= Adopted[(int)TechTreeType.Thought][_onProgress[i]])
            {
                List<TechTrees.Node.SubNode> nextNodes = NextNodes[_onProgress[i]];
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
                        default:
                            // 다른 테크트리는 활성화 여부를 해당 테크트리가 스스로 결정한다.
                            break;
                    }
                }

                // 진행 중인 연구 리스트에서 제외
                _onProgress.Remove(_onProgress[i]);

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
            _titleTexts[i].text = NodeData[i].NodeName;
        }
    }


    private void OnEnable()
    {
        // 활성화, 비활성화
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // 이미 활성화 된 것만
            if (NodeBtnObjects[i].activeSelf)
            {
                NodeUpdate(i);
            }
        }
    }
}
