using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("기본")]
    [SerializeField] private string _nodeName = null;
    [SerializeField] private string _description = null;
    [SerializeField] private byte _eraNum = 1;
    [SerializeField] private TechTrees.SubNode[] _requirements = null;
    [SerializeField] private NodeElementSociety[] _elements = null;

    [Header("참조")]
    [SerializeField] private TMP_Text _titleText = null;

    private byte _nodeNum = 0;
    private bool _isAvailable = false;
    private bool _isEnable = false;



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        PopUpViewSociety.Instance.NodeSelect(_nodeNum, -1, _description, _isAvailable);
    }


    /// <summary>
    /// 노드 초기화
    /// </summary>
    public void SetNode(byte nodeNum, List<NodeElementSociety> elementList, TechTrees techTreeData, Dictionary<string, byte> nodeIndex)
    {
        // 노드 번호 설정
        _nodeNum = nodeNum;

        // 요구조건에 해당하는 노드에 다음 노드를 업데이트
        for (byte i = 0; i < _requirements.Length; ++i)
        {
            techTreeData.AddNextNode(_requirements[i].Type, nodeIndex[_requirements[i].NodeName], _nodeName, TechTreeType.Society);
        }

        // 가변 배열에 하위 요소 추가
        for (byte i = 0; i < _elements.Length; ++i)
        {
            _elements[i].SetElement((byte)elementList.Count);
            elementList.Add(_elements[i]);
        }

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
            _isAvailable = avaiable;
        }
    }



    /* ==================== Private Methods ==================== */

    private void OnLanguageChange()
    {
        // 노드 이름 설정
        _titleText.text = _nodeName;
    }
}
