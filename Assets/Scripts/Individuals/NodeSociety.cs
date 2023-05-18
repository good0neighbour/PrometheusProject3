using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("기본")]
    [SerializeField] private string _nodeName = null;
    [SerializeField] private TechTrees.SubNode[] _requirements = null;

    [Header("참조")]
    [SerializeField] private TMP_Text _titleText = null;

    private Dictionary<string, byte> _indexDictionary = null;
    private PopUpViewSociety _popUpView = null;
    private byte _nodeNum = 0;



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        _popUpView.NodeSelect(_nodeNum);
    }


    /// <summary>
    /// 노드 초기화
    /// </summary>
    public virtual void SetNode(byte nodeNum, TechTreeType techTreeType, PopUpViewSociety popUpView)
    {
        // 노드 번호 설정
        _nodeNum = nodeNum;

        // 테크트리 화면 설정
        _popUpView = popUpView;

        // 참조
        TechTrees techTreeData = PlayManager.Instance.GetTechTreeData();
        _indexDictionary = techTreeData.GetIndexDictionary();

        // 요구조건에 해당하는 노드에 다음 노드를 업데이트
        for (int i = 0; i < _requirements.Length; i++)
        {
            techTreeData.AddNextNode(_requirements[i].Type, _indexDictionary[_requirements[i].NodeName], _nodeName, techTreeType);
        }

        // 텍스트 업데이트
        OnLanguageChange();

        // 대리자 등록
        Language.OnLanguageChange += OnLanguageChange;
    }



    /* ==================== Private Methods ==================== */

    private void OnLanguageChange()
    {
        // 노드 이름 설정
        _titleText.text = _nodeName;
    }
}
