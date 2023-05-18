using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("�⺻")]
    [SerializeField] private string _nodeName = null;
    [SerializeField] private TechTrees.SubNode[] _requirements = null;

    [Header("����")]
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
    /// ��� �ʱ�ȭ
    /// </summary>
    public virtual void SetNode(byte nodeNum, TechTreeType techTreeType, PopUpViewSociety popUpView)
    {
        // ��� ��ȣ ����
        _nodeNum = nodeNum;

        // ��ũƮ�� ȭ�� ����
        _popUpView = popUpView;

        // ����
        TechTrees techTreeData = PlayManager.Instance.GetTechTreeData();
        _indexDictionary = techTreeData.GetIndexDictionary();

        // �䱸���ǿ� �ش��ϴ� ��忡 ���� ��带 ������Ʈ
        for (int i = 0; i < _requirements.Length; i++)
        {
            techTreeData.AddNextNode(_requirements[i].Type, _indexDictionary[_requirements[i].NodeName], _nodeName, techTreeType);
        }

        // �ؽ�Ʈ ������Ʈ
        OnLanguageChange();

        // �븮�� ���
        Language.OnLanguageChange += OnLanguageChange;
    }



    /* ==================== Private Methods ==================== */

    private void OnLanguageChange()
    {
        // ��� �̸� ����
        _titleText.text = _nodeName;
    }
}
