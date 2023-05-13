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
        public ushort FundCost;
        public TechTag[] Requirments;
        public FaciityTag[] NextNodes;
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
