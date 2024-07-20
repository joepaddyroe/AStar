using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinder
{
    private AStarMap _aStarMap;
    private AStarNodeMinHeap _minHeap;
    
    private AStarNode _startNode;
    private AStarNode _endNode;
    
    private List<AStarNode> _openList = new List<AStarNode>();
    private List<AStarNode> _closedList = new List<AStarNode>();

    public AStarPathfinder()
    {
        
    }
    
    public void SetMap(AStarMap map)
    {
        _aStarMap = map;
    }

    public void SetStartNode(int nodeID)
    {
        _startNode = _aStarMap.MapNodes[nodeID];
    }
    
    public void SetEndNode(int nodeID)
    {
        _endNode = _aStarMap.MapNodes[nodeID];
    }

    private List<AStarNode> RetracePath(AStarNode lastNode)
    {
        List<AStarNode> returnPath = new List<AStarNode>();

        returnPath.Add(lastNode);
        
        while (lastNode != _startNode)
        {
            returnPath.Add(lastNode.Parent);
            lastNode = lastNode.Parent;
        }

        returnPath.Reverse();

        return returnPath;
    }
    
    public List<AStarNode> GetPath()
    {
        //float startTime = Time.realtimeSinceStartup;
        // Debug.Log("Start Time: " + (startTime * 1000) + "ms");
        
        List<AStarNode> returnPath = new List<AStarNode>();
        _openList = new List<AStarNode>();
        _closedList = new List<AStarNode>();
        _minHeap = new AStarNodeMinHeap();
        
        // set the current node as the start node and set its scores
        AStarNode currentNode = _startNode;
        currentNode.GScore = 1;
        currentNode.FScore = Mathf.Infinity;
        
        // add the current node (start node) to the open list
        _openList.Add(currentNode);
        _minHeap.Insert(currentNode);
        
        // set a debug ticker so we dont loop endlessly during development
        int ticker = 5000;
        
        while (_openList.Count > 0 && ticker > 0)
        {
            
            // get the node from the opnList with the smallest fScore

            // slower list sort method
            // _openList.Sort((x, y) => x.FScore.CompareTo(y.FScore));
            // currentNode = _openList[0];

            // faster minHeap sorting method
            currentNode = _minHeap.HeapList[0];
            _minHeap.HeapList.Remove(currentNode);
            
            //check if the current node is the end node
            if (currentNode == _endNode)
            {
                //retrace the path and return out
                //Debug.Log("Path Found!");
                returnPath = RetracePath(currentNode);
                
                //Debug.Log("Time Taken: " + ((Time.realtimeSinceStartup - startTime) * 1000) + "ms");
                
                return returnPath;
            }
            
            // remove this node from the openList for future consideration
            _openList.Remove(currentNode);
            _closedList.Add(currentNode);
            
            for (int i = 0; i < currentNode.Neighbours.Count; i++)
            {
                if (!_closedList.Contains(currentNode.Neighbours[i]) && currentNode.Neighbours[i].IsBlocked == false)
                {
                    // calculate a score for each neighbour
                    float tempGScore = currentNode.GScore + currentNode.GetNodeDistance(i);
                    
                    if (tempGScore < currentNode.Neighbours[i].GScore)
                    {
                        currentNode.Neighbours[i].Parent = currentNode;
                        currentNode.Neighbours[i].GScore = tempGScore;
                        currentNode.Neighbours[i].FScore = tempGScore + _aStarMap.GetHCostBetweenNodes(currentNode.Neighbours[i], _endNode);

                        if (!_openList.Contains(currentNode.Neighbours[i]))
                        {
                            _openList.Add(currentNode.Neighbours[i]);
                            _minHeap.Insert(currentNode.Neighbours[i]);
                        }
                    }
                }
            }
            
            ticker--;
        }

        Debug.Log("Path Not Found!");
        return returnPath;
    }
    
}
