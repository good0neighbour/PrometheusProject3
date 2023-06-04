using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("기본")]
    [SerializeField] private string _nodeName = null;
    [SerializeField] private byte _eraNum = 1;
    [SerializeField] private TechTrees.SubNode[] _requirements = null;
    [SerializeField] private NodeElementSociety[] _elements = null;

    [Header("비용")]
    [SerializeField] private ushort _cultureCost = 1;

    [Header("참조")]
    [SerializeField] private TMP_Text _titleText = null;

    private byte _nodeNum = 0;
    private ushort _descriptionNum = 0;
    private bool _isEnable = false;

    public bool IsAvailable
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        PopUpViewSociety.Instance.NodeSelect(_nodeNum, -1, Language.Instance[_nodeName], Language.Instance[_descriptionNum], IsAvailable);
    }


    /// <summary>
    /// 승인 버튼 클릭 시
    /// </summary>
    public void BtnAdopt()
    {
        PlayManager.Instance[VariableUint.Culture] -= _cultureCost;
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Culture);
    }


    /// <summary>
    /// 승인 시 동작
    /// </summary>
    public void OnAdopt(float[] adopted, NodeSociety[] nodes)
    {
        // 다음 시대
        PlayManager.Instance[VariableByte.Era] = (byte)(_eraNum + 1);

        // 사회 채택
        adopted[_nodeNum] = 1.0f;

        // 용도는 다른 함수이나 같은 동작을 할 것이라서 호출했다.
        AlreadyAdopted(nodes);
    }


    /// <summary>
    /// 저장된 데이타를 불러왔을 때 이미 승인 된 경우
    /// </summary>
    public void AlreadyAdopted(NodeSociety[] nodes)
    {
        // 하위 요소 사용 가능
        for (byte i = 0; i < _elements.Length; ++i)
        {
            _elements[i].IsAvailable = true;
        }

        // 같은 시대 사회 사용 불가
        for (byte i = 0; i < nodes.Length; ++i)
        {
            if (_nodeNum != i)
            {
                nodes[i].SetAvaiable(_eraNum, false);
            }
        }

        // 최신 사회 이름으로 등록
        GameManager.Instance.LatestSocietyName = _nodeName;
    }


    /// <summary>
    /// 노드 초기화
    /// </summary>
    public void SetNode(byte nodeNum, List<NodeElementSociety> elementList, TechTrees techTreeData, Dictionary<string, byte> nodeIndex)
    {
        // 노드 번호 설정
        _nodeNum = nodeNum;

        // 노드는 기본적으로 사용 가능 상태로 시작
        IsAvailable = true;

        // 요구조건에 해당하는 노드에 다음 노드를 업데이트
        if (!GameManager.Instance.IsTechTreeInitialized)
        {
            for (byte i = 0; i < _requirements.Length; ++i)
            {
                techTreeData.AddNextNode(_requirements[i].Type, nodeIndex[_requirements[i].NodeName], _nodeName, TechTreeType.Society);
            }
        }

        // 가변 배열에 하위 요소 추가
        for (byte i = 0; i < _elements.Length; ++i)
        {
            _elements[i].SetElement((byte)elementList.Count);
            elementList.Add(_elements[i]);
        }

        // 설명 글 인덱스
        _descriptionNum = (ushort)(Language.Instance.GetLanguageIndex(_nodeName) + 1);

        // 텍스트 업데이트
        OnLanguageChange();

        // 대리자 등록
        Language.OnLanguageChange += OnLanguageChange;
    }


    /// <summary>
    /// 활성화 여부 확인
    /// </summary>
    public void CheckEnable(float[][] adopted, Dictionary<string, byte> nodeIndex)
    {
        // 이미 활성화 상태면 동작하지 않는다.
        if (_isEnable)
        {
            return;
        }

        // 요구조건 확인
        _isEnable = true;
        for (byte i = 0; i < _requirements.Length; ++i)
        {
            if (1.0f > adopted[(int)_requirements[i].Type][nodeIndex[_requirements[i].NodeName]])
            {
                _isEnable = false;
            }

            if (!_isEnable)
            {
                break;
            }
        }

        // 조건 충족 시 활성화
        if (_isEnable)
        {
            gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// 사용 가능 여부 설정
    /// </summary>
    public void SetAvaiable(byte eraNum, bool avaiable)
    {
        if (_eraNum == eraNum)
        {
            IsAvailable = avaiable;
            if (IsAvailable)
            {
                _titleText.color = Constants.WHITE;
            }
            else
            {
                _titleText.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
    }


    /// <summary>
    /// 비용 확인
    /// </summary>
    public bool CostAvailable()
    {
        return _cultureCost <= PlayManager.Instance[VariableUint.Culture];
    }



    /* ==================== Private Methods ==================== */

    private void OnLanguageChange()
    {
        // 노드 이름 설정
        _titleText.text = Language.Instance[_nodeName];
    }
}
