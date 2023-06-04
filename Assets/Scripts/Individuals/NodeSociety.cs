using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("�⺻")]
    [SerializeField] private string _nodeName = null;
    [SerializeField] private byte _eraNum = 1;
    [SerializeField] private TechTrees.SubNode[] _requirements = null;
    [SerializeField] private NodeElementSociety[] _elements = null;

    [Header("���")]
    [SerializeField] private ushort _cultureCost = 1;

    [Header("����")]
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
    /// ���� ��ư Ŭ�� ��
    /// </summary>
    public void BtnAdopt()
    {
        PlayManager.Instance[VariableUint.Culture] -= _cultureCost;
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Culture);
    }


    /// <summary>
    /// ���� �� ����
    /// </summary>
    public void OnAdopt(float[] adopted, NodeSociety[] nodes)
    {
        // ���� �ô�
        PlayManager.Instance[VariableByte.Era] = (byte)(_eraNum + 1);

        // ��ȸ ä��
        adopted[_nodeNum] = 1.0f;

        // �뵵�� �ٸ� �Լ��̳� ���� ������ �� ���̶� ȣ���ߴ�.
        AlreadyAdopted(nodes);
    }


    /// <summary>
    /// ����� ����Ÿ�� �ҷ����� �� �̹� ���� �� ���
    /// </summary>
    public void AlreadyAdopted(NodeSociety[] nodes)
    {
        // ���� ��� ��� ����
        for (byte i = 0; i < _elements.Length; ++i)
        {
            _elements[i].IsAvailable = true;
        }

        // ���� �ô� ��ȸ ��� �Ұ�
        for (byte i = 0; i < nodes.Length; ++i)
        {
            if (_nodeNum != i)
            {
                nodes[i].SetAvaiable(_eraNum, false);
            }
        }

        // �ֽ� ��ȸ �̸����� ���
        GameManager.Instance.LatestSocietyName = _nodeName;
    }


    /// <summary>
    /// ��� �ʱ�ȭ
    /// </summary>
    public void SetNode(byte nodeNum, List<NodeElementSociety> elementList, TechTrees techTreeData, Dictionary<string, byte> nodeIndex)
    {
        // ��� ��ȣ ����
        _nodeNum = nodeNum;

        // ���� �⺻������ ��� ���� ���·� ����
        IsAvailable = true;

        // �䱸���ǿ� �ش��ϴ� ��忡 ���� ��带 ������Ʈ
        if (!GameManager.Instance.IsTechTreeInitialized)
        {
            for (byte i = 0; i < _requirements.Length; ++i)
            {
                techTreeData.AddNextNode(_requirements[i].Type, nodeIndex[_requirements[i].NodeName], _nodeName, TechTreeType.Society);
            }
        }

        // ���� �迭�� ���� ��� �߰�
        for (byte i = 0; i < _elements.Length; ++i)
        {
            _elements[i].SetElement((byte)elementList.Count);
            elementList.Add(_elements[i]);
        }

        // ���� �� �ε���
        _descriptionNum = (ushort)(Language.Instance.GetLanguageIndex(_nodeName) + 1);

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
    /// ��� Ȯ��
    /// </summary>
    public bool CostAvailable()
    {
        return _cultureCost <= PlayManager.Instance[VariableUint.Culture];
    }



    /* ==================== Private Methods ==================== */

    private void OnLanguageChange()
    {
        // ��� �̸� ����
        _titleText.text = Language.Instance[_nodeName];
    }
}
