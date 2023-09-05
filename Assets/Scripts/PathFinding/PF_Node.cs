using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Node : IHeapItem<PF_Node>
{
    public int HeapIdx { get => heapIdx; set => heapIdx = value; }

    public bool walkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;

    // ���� ��忡�� ���� �������� ����ġ
    public int gCost;
    // ���� ��忡�� Ÿ�� �������� ����ġ
    public int hCost;
    public int FCost => gCost + hCost;


    public PF_Node parentNode;

    private int heapIdx;

    public PF_Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int CompareTo(PF_Node _nodeToCompare)
    {
        int compare = FCost.CompareTo(_nodeToCompare.FCost);
        if (compare == 0)
            compare = hCost.CompareTo(_nodeToCompare.hCost);

        return -compare;
    }
}
