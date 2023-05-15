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
        [Header("���")]
        public ushort FundCost;
        public byte IronCost;
        public byte NukeCost;
        [Header("�䱸����")]
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
    /// ��ũƮ�� ��带 �ؽ����̺� ����Ѵ�.
    /// </summary>
    public void GetReady()
    {
        byte i;

        // ��� ��� ���
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
    /// ��� �ε��� �ؽ����̺� ��ȯ.
    /// </summary>
    public Dictionary<string, byte> GetIndexDictionary()
    {
        return _indexDictionary;
    }
}