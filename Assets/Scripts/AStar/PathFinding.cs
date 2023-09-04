using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void FindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(_startPos);
        Node targetNode = grid.NodeFromWorldPoint(_targetPos); 
    }
}
