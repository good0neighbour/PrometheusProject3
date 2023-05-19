using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("�⺻")]
    [SerializeField] private string _nodeName = null;
    [SerializeField] private string _description = null;
    [SerializeField] private byte _eraNum = 1;
    [SerializeField] private TechTrees.SubNode[] _requirements = null;
    [SerializeField] private NodeElementSociety[] _elements = null;

    [Header("����")]
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
    /// ��� �ʱ�ȭ
    /// </summary>
    public void SetNode(byte nodeNum, List<NodeElementSociety> elementList, TechTrees techTreeData, Dictionary<string, byte> nodeIndex)
    {
        // ��� ��ȣ ����
        _nodeNum = nodeNum;

        // �䱸���ǿ� �ش��ϴ� ��忡 ���� ��带 ������Ʈ
        for (byte i = 0; i < _requirements.Length; ++i)
        {
            techTreeData.AddNextNode(_requirements[i].Type, nodeIndex[_requirements[i].NodeName], _nodeName, TechTreeType.Society);
        }

        // ���� �迭�� ���� ��� �߰�
        for (byte i = 0; i < _elements.Length; ++i)
        {
            _elements[i].SetElement((byte)elementList.Count);
            elementList.Add(_elements[i]);
        }

        // �ؽ�Ʈ ������Ʈ
        OnLanguageChange();

        // �븮�� ���
        Language.OnLanguageChange += OnLanguageChange;
    }


    /// <summary>
    /// Ȱ��ȭ ���� Ȯ��
    /// </summary>
    public void CheckEnable(float[][] adopted, Dictionary<string, byte> nodeIndex)
    {
        // �̹� Ȱ��ȭ ���¸� �������� �ʴ´�.
        if (_isEnable)
        {
            return;
        }

        // �䱸���� Ȯ��
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

        // ���� ���� �� Ȱ��ȭ
        if (_isEnable)
        {
            gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// ��� ���� ���� ����
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
        // ��� �̸� ����
        _titleText.text = _nodeName;
    }
}
