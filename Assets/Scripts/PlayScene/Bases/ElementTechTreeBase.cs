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
        [Header("���")]
        public ushort FundCost;
        [Header("���� ���")]
        public Node[] Elements;
    }



    /* ==================== Variables ==================== */

    [SerializeField] private Node[] _societyNodes = null;
    private Dictionary<string, byte> _indexDictionary = new Dictionary<string, byte>();



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ��� �迭 ��ȯ.
    /// </summary>
    public Node[] GetNodes(ElementTechTreeType type)
    {
        switch (type)
        {
            case ElementTechTreeType.Society:
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



    /* ==================== Private Methods ==================== */
}
