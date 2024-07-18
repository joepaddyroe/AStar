using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMap
{
    private int _nodeCount;
    private int _columnCount;
    private int _rowCount;
    
    private List<AStarNode> _mapNodes;
    public List<AStarNode> MapNodes => _mapNodes;
    
    public AStarMap()
    {
        _mapNodes = new List<AStarNode>();
    }

    public void BuildMap(int columns, int rows)
    {
        _nodeCount = columns * rows;
        _columnCount = columns;
        _rowCount = rows;
        
        for (int i = 0; i < _nodeCount; i++)
        {
            AddNode(new AStarNode(i));
        }
        
        SetNodeNeighbours();
    }
    
    private void AddNode(AStarNode node)
    {
        if(!_mapNodes.Contains(node))
            _mapNodes.Add(node);
    }
    
    public void SetNodeBlocked(int nodeID, bool isBlocked)
    {
        _mapNodes[nodeID].SetBlocked(isBlocked);
    }

    public float GetHCostBetweenNodes(AStarNode node1, AStarNode node2)
    {
        // manhattan width + height of map calculation
     
        float horizontal = Mathf.Abs(node2.Column - node1.Column);
        float vertical = Mathf.Abs(node2.Row - node1.Row);
        
        return horizontal + vertical;
    }
    
    private void SetNodeNeighbours()
    {
        // each node needs to know about its neighbours in the grid
        // knowing column and row count, we can determine which nodes are neighbours
        // also we set the neighbour's neighbour (I am your neighbour, so you are mine)
        // and also check that we dont add duplicates in the neighbour Add call
        
        // we can also precompute the distance cost between nodes here rather than doing it at runtime
        // 1 unit for horizontal/vertical and 1.414 for diagonal (rough Pythagoras)
        
        for (int row = 0; row < _rowCount; row++)
        {
            for (int column = 0; column < _columnCount; column++)
            {
                int currentNodeIndex = (row * _columnCount) + column;
                AStarNode node = _mapNodes[currentNodeIndex];
                node.SetColumnRow(column, row);

                if (column > 0)
                {
                    AStarNode neighbour = _mapNodes[currentNodeIndex - 1]; // left
                    node.AddNeighbour(neighbour, 1);
                    neighbour.AddNeighbour(node, 1);
                    
                    if (row > 0)
                    {
                        neighbour = _mapNodes[currentNodeIndex - _columnCount - 1]; // top left
                        node.AddNeighbour(neighbour, 1.414f);
                        neighbour.AddNeighbour(node, 1.414f);
                    }

                    if (row < _rowCount-1)
                    {
                        neighbour = _mapNodes[currentNodeIndex + _columnCount - 1]; // bottom left
                        node.AddNeighbour(neighbour, 1.414f);
                        neighbour.AddNeighbour(node, 1.414f);
                    }
                }


                if (row > 0)
                {
                    AStarNode neighbour = _mapNodes[currentNodeIndex - _columnCount]; // top
                    node.AddNeighbour(neighbour, 1);
                    neighbour.AddNeighbour(node, 1);
                }

                if (row < _rowCount-1)
                {
                    AStarNode neighbour = _mapNodes[currentNodeIndex + _columnCount]; // bottom
                    node.AddNeighbour(neighbour, 1);
                    neighbour.AddNeighbour(node, 1);
                }


                if (column < _columnCount-1)
                {
                    AStarNode neighbour = _mapNodes[currentNodeIndex + 1]; // right
                    node.AddNeighbour(neighbour, 1);
                    neighbour.AddNeighbour(node, 1);
                    
                    if (row > 0)
                    {
                        neighbour = _mapNodes[currentNodeIndex - _columnCount]; // top right
                        node.AddNeighbour(neighbour, 1.414f);
                        neighbour.AddNeighbour(node, 1.414f);
                    }

                    if (row < _rowCount-1)
                    {
                        neighbour = _mapNodes[currentNodeIndex + _columnCount + 1]; // bottom right
                        node.AddNeighbour(neighbour, 1.414f);
                        neighbour.AddNeighbour(node, 1.414f);
                    }
                }
            }   
        }
    }

    public void ResetNodes()
    {
        foreach (AStarNode node in _mapNodes)
        {
            node.ResetScores();
        }
    }
}
