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
        [Range(0f, 1f)]
        public float ProgressionPerMonth;
        [Header("���")]
        public ushort FundCost;
        public byte IronCost;
        public byte NukeCost;
        [Header("�䱸����")]
        public SubNode[] Requirments;

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
    }



    /* ==================== Variables ==================== */

    [SerializeField] private Node[] _facilityNodes = null;
    [SerializeField] private Node[] _techNodes = null;
    [SerializeField] private Node[] _thoughtNodes = null;
    [SerializeField] private Node[] _societyNodes = null;
    private Dictionary<string, byte> _indexDictionary = new Dictionary<string, byte>();
    private List<Node.SubNode>[][] _nextNodes = new List<Node.SubNode>[(int)TechTreeType.TechTreeTypeEnd][];



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ��ũƮ�� ��带 �ؽ����̺�, ���� �迭�� ����Ѵ�.
    /// </summary>
    public void GetReady()
    {
        BuildNodeSettings(_facilityNodes, TechTreeType.Facility);
        BuildNodeSettings(_techNodes, TechTreeType.Tech);
        BuildNodeSettings(_thoughtNodes, TechTreeType.Thought);
        BuildNodeSettings(_societyNodes, TechTreeType.Society);
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
            case TechTreeType.Society:
                return _societyNodes;
            default:
                Debug.LogError("TechTrees - �߸��� ��ũƮ�� ����");
                return null;
        }
    }

    /// <summary>
    /// ���� ��� ��ȯ
    /// </summary>
    public List<Node.SubNode>[] GetNextNodes(TechTreeType type)
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



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ���� ��� ���
    /// </summary>
    private void BuildNodeSettings(Node[] techTreeNodes, TechTreeType techTreeType)
    {
        // �迭 ����
        byte length = (byte)techTreeNodes.Length;
        _nextNodes[(int)techTreeType] = new List<Node.SubNode>[length];

        // ��� ���
        for (byte i = 0; i < length; ++i)
        {
            Node node = techTreeNodes[i];
            _indexDictionary.Add(node.NodeName, i);

            for (int j = 0; j < node.Requirments.Length; j++)
            {
                Node.SubNode subNode = node.Requirments[j];
                byte type = (byte)subNode.Type;
                byte index = _indexDictionary[subNode.NodeName];

                // ���� �迭 ������ �� ������ ���� ����
                if (null == _nextNodes[type][index])
                {
                    _nextNodes[type][index] = new List<Node.SubNode>();
                }

                // ���� ���� ���
                _nextNodes[type][index].Add(new Node.SubNode(node.NodeName, node.Type));
            }
        }
    }
}