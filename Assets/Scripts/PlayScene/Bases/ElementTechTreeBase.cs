using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementTechTrees", menuName = "PrometheusProject/ElementTechTrees")]
public class ElementTechTreeBase : ScriptableObject
{
    [Serializable]
    public class Node
    {
        public string NodeName;
        public ElementTechTreeType Type;
        public byte Floor;
        public string Description;
        [Range(0.0f, 1.0f)]
        public float ProgressionPerAdoption;
        [Header("비용")]
        public ushort FundCost;
        [Header("하위 노드")]
        public Node[] Elements;
    }



    /* ==================== Variables ==================== */

    [SerializeField] private Node[] _societyNodes = null;
    private Dictionary<string, byte> _indexDictionary = new Dictionary<string, byte>();



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 노드 배열 반환.
    /// </summary>
    public Node[] GetNodes(ElementTechTreeType type)
    {
        switch (type)
        {
            case ElementTechTreeType.Society:
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



    /* ==================== Private Methods ==================== */
}
