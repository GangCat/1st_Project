using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPT_Node : IHeapItem_GPT<GPT_Node>
{
    public int HeapIdx { get => heapIdx; set => heapIdx = value; }

    public Vector3 worldPos;
    public bool walkable;
    public int gridX;
    public int gridY;

    /// <summary>
    /// ���� ��忡�� ���� �������� ����ġ
    /// </summary>
    public int gCost;
    /// <summary>
    /// ���� ��忡�� Ÿ�� �������� ����ġ
    /// </summary>
    public int hCost;
    public int FCost => gCost + hCost;

    public GPT_Node parentNode;
    private int heapIdx;

    public GPT_Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int CompareTo(GPT_Node _nodeToCompare)
    {
        int compare = FCost.CompareTo(_nodeToCompare.FCost);
        if (compare == 0)
            compare = hCost.CompareTo(_nodeToCompare.hCost);

        return -compare;
    }
}
