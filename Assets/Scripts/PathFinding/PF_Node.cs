using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Node : IHeapItem<PF_Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;

    /// <summary>
    /// 시작 노드에서 현재 노드까지의 가중치
    /// </summary>
    public int gCost;
    /// <summary>
    /// 현재 노드에서 타겟 노드까지의 가중치
    /// </summary>
    public int hCost;
    public PF_Node parentNode;

    private int heapIdx;

    public PF_Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int FCost => gCost + hCost;

    public int HeapIdx { get => heapIdx; set => heapIdx = value; }

    public int CompareTo(PF_Node _nodeToCompare)
    {
        int compare = FCost.CompareTo(_nodeToCompare.FCost);
        if (compare == 0)
            compare = hCost.CompareTo(_nodeToCompare.hCost);

        return -compare;
    }

    public void Reset()
    {
        gCost = 0;
        hCost = 0;
        HeapIdx = 0;
    }
}
