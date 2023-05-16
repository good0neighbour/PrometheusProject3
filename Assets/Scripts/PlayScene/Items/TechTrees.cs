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
        [Header("비용")]
        public ushort FundCost;
        public byte IronCost;
        public byte NukeCost;
        [Header("요구사항")]
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
    /// 테크트리 노드를 해쉬테이블, 가변 배열에 등록한다.
    /// </summary>
    public void GetReady()
    {
        BuildNodeSettings(_facilityNodes, TechTreeType.Facility);
        BuildNodeSettings(_techNodes, TechTreeType.Tech);
        BuildNodeSettings(_thoughtNodes, TechTreeType.Thought);
        BuildNodeSettings(_societyNodes, TechTreeType.Society);
    }

    /// <summary>
    /// 노드 배열 반환.
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
                Debug.LogError("TechTrees - 잘못된 테크트리 종류");
                return null;
        }
    }

    /// <summary>
    /// 다음 노드 반환
    /// </summary>
    public List<Node.SubNode>[] GetNextNodes(TechTreeType type)
    {
        return _nextNodes[(int)type];
    }

    /// <summary>
    /// 노드 인덱스 해쉬테이블 반환.
    /// </summary>
    public Dictionary<string, byte> GetIndexDictionary()
    {
        return _indexDictionary;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 다음 노드 등록
    /// </summary>
    private void BuildNodeSettings(Node[] techTreeNodes, TechTreeType techTreeType)
    {
        // 배열 생성
        byte length = (byte)techTreeNodes.Length;
        _nextNodes[(int)techTreeType] = new List<Node.SubNode>[length];

        // 노드 등록
        for (byte i = 0; i < length; ++i)
        {
            Node node = techTreeNodes[i];
            _indexDictionary.Add(node.NodeName, i);

            for (int j = 0; j < node.Requirments.Length; j++)
            {
                Node.SubNode subNode = node.Requirments[j];
                byte type = (byte)subNode.Type;
                byte index = _indexDictionary[subNode.NodeName];

                // 가변 배열 생성한 적 없으면 새로 생성
                if (null == _nextNodes[type][index])
                {
                    _nextNodes[type][index] = new List<Node.SubNode>();
                }

                // 다음 노드로 등록
                _nextNodes[type][index].Add(new Node.SubNode(node.NodeName, node.Type));
            }
        }
    }
}