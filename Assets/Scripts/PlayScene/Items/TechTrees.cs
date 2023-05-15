using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TechTrees", menuName = "PrometheusProject/TechTrees")]
public class TechTrees : ScriptableObject
{
    /* ==================== Facilities ==================== */

    [Serializable]
    public struct Node
    {
        public string NodeName;
        public TechTreeType Type;
        public Vector2 NodePosition;
        public string Description;
        [Header("비용")]
        public ushort FundCost;
        public byte IronCost;
        public byte NukeCost;
        [Header("요구사항")]
        public RequirmentNode[] Requirments;

        [Serializable]
        public struct RequirmentNode
        {
            public string NodeName;
            public TechTreeType Type;
        }
    }

    [SerializeField] private Node[] _facilityNodes = null;
    [SerializeField] private Node[] _techNodes = null;
    [SerializeField] private Node[] _thoughtNodes = null;
    [SerializeField] private Node[] _societyNodes = null;
    private Dictionary<string, byte> _indexDictionary = new Dictionary<string, byte>();

    /// <summary>
    /// 테크트리 노드를 해쉬테이블에 등록한다.
    /// </summary>
    public void GetReady()
    {
        byte i;

        // 모든 노드 등록
        for (i = 0; i < _facilityNodes.Length; ++i)
        {
            _indexDictionary.Add(_facilityNodes[i].NodeName, i);
        }
        for (i = 0; i < _techNodes.Length; ++i)
        {
            _indexDictionary.Add(_techNodes[i].NodeName, i);
        }
        for (i = 0; i < _thoughtNodes.Length; ++i)
        {
            _indexDictionary.Add(_thoughtNodes[i].NodeName, i);
        }
        for (i = 0; i < _societyNodes.Length; ++i)
        {
            _indexDictionary.Add(_societyNodes[i].NodeName, i);
        }
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
    /// 노드 인덱스 해쉬테이블 반환.
    /// </summary>
    public Dictionary<string, byte> GetIndexDictionary()
    {
        return _indexDictionary;
    }
}