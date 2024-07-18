using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStarNode
{
    private int _id;
    private int _column;
    private int _row;
    
    private List<AStarNode> _neighbours;
    private List<float> _neighbourDistances;
    private bool _isBlocked;
    private int _neighbourCount;

    public bool IsBlocked => _isBlocked;
    public int ID => _id;
    public int Column => _column;
    public int Row => _row;
    public AStarNode Parent = null;
    
    public float FScore = Mathf.Infinity;
    public float GScore = Mathf.Infinity;
    public float HScore = 0;
    
    public AStarNode(int id)
    {
        _id = id;
        _neighbours = new List<AStarNode>();
        _neighbourDistances = new List<float>();
    }

    public void SetColumnRow(int column, int row)
    {
        _column = column;
        _row = row;
    }
    
    public void AddNeighbour(AStarNode node, float distance = 1)
    {
        if (!_neighbours.Contains(node))
        {
            _neighbours.Add(node);
            _neighbourDistances.Add(distance);   
        }

        _neighbourCount = _neighbours.Count;
    }

    public List<AStarNode> Neighbours => _neighbours;
    
    public float GetNodeDistance(int nodeIndex)
    {
        return _neighbourDistances[nodeIndex];
    }

    public void SetBlocked(bool blocked)
    {
        _isBlocked = blocked;
    }

    public void ResetScores()
    {
        FScore = Mathf.Infinity;
        GScore = Mathf.Infinity;
        HScore = 0;
    }
}
