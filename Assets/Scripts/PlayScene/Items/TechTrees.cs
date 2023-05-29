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
        [Header("비용")]
        public ushort FundCost;
        public ushort ResearchCost;
        public ushort CultureCost;
        public ushort Maintenance;
        public ushort Injure;
        public byte IronCost;
        public byte NukeCost;
        [Header("수익")]
        public ushort AnnualFund;
        public ushort AnnualResearch;
        public ushort AnnualCulture;
        public byte Stability;
        public float PopulationMovement;
        public float Police;
        public float Health;
        public float Safety;
        [Header("요구사항")]
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
    /// 테크트리 노드를 해쉬테이블, 가변 배열에 등록한다.
    /// </summary>
    public void GetReady()
    {
        if (GameManager.Instance.IsTechTreeInitialized)
        {
            return;
        }

        // 노드 등록
        AddDictionary(_facilityNodes, TechTreeType.Facility);
        AddDictionary(_techNodes, TechTreeType.Tech);
        AddDictionary(_thoughtNodes, TechTreeType.Thought);

        // 다음 노드 등록
        SetNextNodes(_facilityNodes);
        SetNextNodes(_techNodes);
        SetNextNodes(_thoughtNodes);

        GameManager.Instance.IsTechTreeInitialized = true;
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
            default:
                Debug.LogError("TechTrees - 잘못된 테크트리 종류");
                return null;
        }
    }


    /// <summary>
    /// 다음 노드 반환
    /// </summary>
    public List<SubNode>[] GetNextNodes(TechTreeType type)
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


    /// <summary>
    /// 다음 노드 가변 배열에 노드 추가
    /// </summary>
    public void AddNextNode(TechTreeType targetType, byte targetIndex, string nodeName, TechTreeType nodeType)
    {
        // 가변 배열 생성한 적 없으면 새로 생성
        if (null == _nextNodes[(int)targetType][targetIndex])
        {
            _nextNodes[(int)targetType][targetIndex] = new List<SubNode>();
        }

        // 다음 노드로 등록
        _nextNodes[(int)targetType][targetIndex].Add(new SubNode(nodeName, nodeType));
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 노드 등록
    /// </summary>
    private void AddDictionary(Node[] techTreeNodes, TechTreeType techTreeType)
    {
        // 해쉬테이블에 노드 인덱스 저장
        for (byte i = 0; i < techTreeNodes.Length; ++i)
        {
            _indexDictionary.Add(techTreeNodes[i].NodeName, i);
        }

        // 다음 노드 저장을 위한 배열 생성
        byte length = (byte)techTreeNodes.Length;
        _nextNodes[(int)techTreeType] = new List<SubNode>[length];
    }


    /// <summary>
    /// 다음 노드 등록
    /// </summary>
    private void SetNextNodes(Node[] techTreeNodes)
    {
        // 노드 등록
        for (byte i = 0; i < techTreeNodes.Length; ++i)
        {
            Node node = techTreeNodes[i];

            for (byte j = 0; j < node.Requirments.Length; ++j)
            {
                SubNode subNode = node.Requirments[j];
                byte type = (byte)subNode.Type;
                byte index = _indexDictionary[subNode.NodeName];

                // 가변 배열 생성한 적 없으면 새로 생성
                if (null == _nextNodes[type][index])
                {
                    _nextNodes[type][index] = new List<SubNode>();
                }

                // 다음 노드로 등록
                _nextNodes[type][index].Add(new SubNode(node.NodeName, node.Type));
            }
        }
    }
}