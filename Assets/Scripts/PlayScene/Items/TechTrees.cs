using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TechTrees", menuName = "PrometheusProject/TechTrees")]
public class TechTrees : ScriptableObject
{
    /* ==================== Facilities ==================== */

    [Serializable]
    public struct Node
    {
        public string NodeName;
        public FaciityTag Tag;
        public Vector2 NodePosition;
        public string Description;
        [Header("비용")]
        public ushort FundCost;
        public byte IronCost;
        public byte NukeCost;
        [Header("요구사항")]
        public TechTag[] Requirments;
        public FaciityTag[] PreviousNodes;
    }

    [SerializeField] private Node[] _facilityNodes = null;
    [SerializeField] private Node[] _texhNodes = null;
    [SerializeField] private Node[] _thoughtNodes = null;
    [SerializeField] private Node[] _societyNodes = null;

    public Node[] GetFacilityNodes()
    {
        return _facilityNodes;
    }

    public Node[] GetTechNodes()
    {
        return _texhNodes;
    }

    public Node[] GetThoughtNodes()
    {
        return _thoughtNodes;
    }

    public Node[] GetSocietyNodes()
    {
        return _societyNodes;
    }
}
