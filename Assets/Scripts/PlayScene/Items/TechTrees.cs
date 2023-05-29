using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TechTrees", menuName = "PrometheusProject/TechTrees")]
public class TechTrees : ScriptableObject
{
    [Serializable]
    public class Node
    {
        public string NodeName;
        public TechTreeType Type;
        public Vector2 NodePosition;
        public string Description;
        public float ProgressionValue;
        [Header("���")]
        public ushort FundCost;
        public ushort ResearchCost;
        public ushort CultureCost;
        public ushort Maintenance;
        public ushort Injure;
        public byte IronCost;
        public byte NukeCost;
        [Header("����")]
        public ushort AnnualFund;
        public ushort AnnualResearch;
        public ushort AnnualCulture;
        public byte Stability;
        public float PopulationMovement;
        public float Police;
        public float Health;
        public float Safety;
        [Header("�䱸����")]
        public SubNode[] Requirments;
    }

    [Serializable]
    public struct SubNode
    {
        public string NodeName;
        public TechTreeType Type;

        public SubNode(string nodeName, TechTreeType type)
        {
            NodeName = nodeName;
            Type = type;
        }
    }



    /* ==================== Variables ==================== */

    [SerializeField] private Node[] _facilityNodes = null;
    [SerializeField] private Node[] _techNodes = null;
    [SerializeField] private Node[] _thoughtNodes = null;
    private Dictionary<string, byte> _indexDictionary = new Dictionary<string, byte>();
    private List<SubNode>[][] _nextNodes = new List<SubNode>[(int)TechTreeType.TechTreeEnd][];



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ��ũƮ�� ��带 �ؽ����̺�, ���� �迭�� ����Ѵ�.
    /// </summary>
    public void GetReady()
    {
        if (GameManager.Instance.IsTechTreeInitialized)
        {
            return;
        }

        // ��� ���
        AddDictionary(_facilityNodes, TechTreeType.Facility);
        AddDictionary(_techNodes, TechTreeType.Tech);
        AddDictionary(_thoughtNodes, TechTreeType.Thought);

        // ���� ��� ���
        SetNextNodes(_facilityNodes);
        SetNextNodes(_techNodes);
        SetNextNodes(_thoughtNodes);

        GameManager.Instance.IsTechTreeInitialized = true;
    }


    /// <summary>
    /// ��� �迭 ��ȯ.
    /// </summary>
    public Node[] GetNodes(TechTreeType type)
    {
        switch (type)
        {
            case TechTreeType.Facility:
                return _facilityNodes;
            case TechTreeType.Tech:
                return _techNodes;
            case TechTreeType.Thought:
                return _thoughtNodes;
            default:
                Debug.LogError("TechTrees - �߸��� ��ũƮ�� ����");
                return null;
        }
    }


    /// <summary>
    /// ���� ��� ��ȯ
    /// </summary>
    public List<SubNode>[] GetNextNodes(TechTreeType type)
    {
        return _nextNodes[(int)type];
    }


    /// <summary>
    /// ��� �ε��� �ؽ����̺� ��ȯ.
    /// </summary>
    public Dictionary<string, byte> GetIndexDictionary()
    {
        return _indexDictionary;
    }


    /// <summary>
    /// ���� ��� ���� �迭�� ��� �߰�
    /// </summary>
    public void AddNextNode(TechTreeType targetType, byte targetIndex, string nodeName, TechTreeType nodeType)
    {
        // ���� �迭 ������ �� ������ ���� ����
        if (null == _nextNodes[(int)targetType][targetIndex])
        {
            _nextNodes[(int)targetType][targetIndex] = new List<SubNode>();
        }

        // ���� ���� ���
        _nextNodes[(int)targetType][targetIndex].Add(new SubNode(nodeName, nodeType));
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ��� ���
    /// </summary>
    private void AddDictionary(Node[] techTreeNodes, TechTreeType techTreeType)
    {
        // �ؽ����̺� ��� �ε��� ����
        for (byte i = 0; i < techTreeNodes.Length; ++i)
        {
            _indexDictionary.Add(techTreeNodes[i].NodeName, i);
        }

        // ���� ��� ������ ���� �迭 ����
        byte length = (byte)techTreeNodes.Length;
        _nextNodes[(int)techTreeType] = new List<SubNode>[length];
    }


    /// <summary>
    /// ���� ��� ���
    /// </summary>
    private void SetNextNodes(Node[] techTreeNodes)
    {
        // ��� ���
        for (byte i = 0; i < techTreeNodes.Length; ++i)
        {
            Node node = techTreeNodes[i];

            for (byte j = 0; j < node.Requirments.Length; ++j)
            {
                SubNode subNode = node.Requirments[j];
                byte type = (byte)subNode.Type;
                byte index = _indexDictionary[subNode.NodeName];

                // ���� �迭 ������ �� ������ ���� ����
                if (null == _nextNodes[type][index])
                {
                    _nextNodes[type][index] = new List<SubNode>();
                }

                // ���� ���� ���
                _nextNodes[type][index].Add(new SubNode(node.NodeName, node.Type));
            }
        }
    }
}