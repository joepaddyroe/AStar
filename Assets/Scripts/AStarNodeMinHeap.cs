using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNodeMinHeap
{
    private List<AStarNode> _heapList = new List<AStarNode>();

    public List<AStarNode> HeapList => _heapList;
    
    public AStarNodeMinHeap()
    {
        
    }

    public void Insert(AStarNode node)
    {
        if (_heapList.Count == 0)
        {
            _heapList.Add(node);
        }
        else
        {
            _heapList.Add(node);
            int currentIndex = _heapList.Count - 1;
            bool sorted = false;
            
            while (!sorted)
            {
                int parentIndex = (currentIndex - 1) / 2;
                if (_heapList[currentIndex].FScore < _heapList[parentIndex].FScore)
                {
                    var temp = _heapList[parentIndex];
                    _heapList[parentIndex] = _heapList[currentIndex];
                    _heapList[currentIndex] = temp;
                    currentIndex = parentIndex;
                }
                else
                {
                    sorted = true;
                }
            }
        }
    }
}