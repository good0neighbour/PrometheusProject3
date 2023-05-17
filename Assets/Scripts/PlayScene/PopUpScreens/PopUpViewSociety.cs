using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpViewSociety : TechTreeBase
{
    /* ==================== Variables ==================== */

    private TMP_Text[] _titleTexts = null;
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
        //AdoptAnimation(PlayManager.Instance[VariableFloat.SocietySupportRate]);
        AdoptAnimation(75.0f);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 활성화 정보 전달
        Adopted[(int)TechTreeType.Society][CurrentNode] = 0.0001f;

        // 연구 진행중인 리스트에 추가
        _onProgress.Add(CurrentNode);

        // 다음 노드 활성화
        List<TechTrees.Node.SubNode> nextNodes = NextNodes[CurrentNode];
        for (byte i = 0; i < nextNodes.Count; ++i)
        {
            switch (nextNodes[i].Type)
            {
                case TechTreeType.Society:
                    // 다음 기술은 연구 시작하는 시점에서 활성화한다.
                    if (EnableCheck(nextNodes[i]))
                    {
                        byte index = NodeIndex[nextNodes[i].NodeName];
                        NodeBtnObjects[index].SetActive(true);
                        NodeUpdate(index);
                    }
                    break;
                default:
                    // 다른 테크트리는 활성화 여부를 해당 테크트리가 스스로 결정한다.
                    break;
            }
        }
    }


    protected override void OnFail()
    {

    }


    protected override bool IsUnadopted()
    {
        bool result = (1.0f <= Adopted[(int)TechTreeType.Society][NodeIndex[NodeData[CurrentNode].NodeName]]);
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
        // 진행 정도 이미지
        _progreesionImages[index].fillAmount = Adopted[(int)TechTreeType.Society][index];
    }


    private void OnLanguageChange()
    {
        for (byte i = 0; i < _titleTexts.Length; ++i)
        {
            // 노드 이름. 
            _titleTexts[i].text = NodeData[i].NodeName;
        }
    }


    private void Awake()
    {
        // 노드 정보 가져오기
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Society);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Society);

        // 배열 생성
        byte length = (byte)NodeData.Length;
        _titleTexts = new TMP_Text[length];
        _progreesionImages = new Image[length];

        // UIString 사용 가능 상태로 만든다.
        UIString.Instance.TechInitialize(length);

        BasicInitialize(length);

        for (byte i = 0; i < length; ++i)
        {
            // 참조
            Transform node = NodeBtnObjects[i].transform;
            _titleTexts[i] = node.Find("TextTitle").GetComponent<TMP_Text>();
            _progreesionImages[i] = node.transform.Find("ImageProgressionBackground").Find("ImageProgression").GetComponent<Image>();

            // 연구 진행 중이었던 것 가변 배열에 추가
            if (0.0f < Adopted[(int)TechTreeType.Society][i])
            {
                _onProgress.Add(i);
            }
        }

        // 비활성화할 것
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            TechTrees.Node.SubNode[] requiredNodes = NodeData[i].Requirments;
            for (byte j = 0; j < requiredNodes.Length; ++j)
            {
                // 모두 승인된 것이 아니면 비활성화
                if (0.0f >= Adopted[(int)requiredNodes[j].Type][NodeIndex[requiredNodes[j].NodeName]])
                {
                    NodeBtnObjects[i].SetActive(false);
                    break;
                }
            }
        }

        // 텍스트 업데이트
        OnLanguageChange();

        // 대리자 등록
        Language.OnLanguageChange += OnLanguageChange;
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
