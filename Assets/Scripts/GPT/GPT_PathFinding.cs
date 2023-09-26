using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GPT_PathFinding : MonoBehaviour
{
    public delegate void FinishPathFindDelegate(GPT_Node[] _waypoints, bool _isPathSuccess);

    public void Init(FinishPathFindDelegate _finishPathFindCallback)
    {
        grid = GetComponent<GPT_Grid>();
        grid.Init();
        finishPathFindCallback = _finishPathFindCallback;

        Rect quadTreeBounds = new Rect(new Vector2(-100f, -100f), new Vector2(200f, 200f));
        int maxNodesPerQuad = 16;
        quadTree = new GPT_QuadTree(quadTreeBounds, maxNodesPerQuad);
    }

    public void StartFindPath(GPT_Node _startNode, GPT_Node _targetNode)
    {
        StartCoroutine(FindPath(_startNode, _targetNode));
    }

    private IEnumerator FindPath(GPT_Node _startNode, GPT_Node _targetNode)
    {
        bool isPathSuccess = false;

        // Perform A* pathfinding using the quadtree
        List<GPT_Node> path = QuadTreeAStar(_startNode, _targetNode);

        if (path != null)
        {
            isPathSuccess = true;
        }

        yield return null;

        finishPathFindCallback?.Invoke(path.ToArray(), isPathSuccess);
    }

    private List<GPT_Node> QuadTreeAStar(GPT_Node _startNode, GPT_Node _targetNode)
    {
        // Perform A* pathfinding within the quadtree
        List<GPT_Node> path = new List<GPT_Node>();

        GPT_QuadTreeAStar quadTreeAStar = new GPT_QuadTreeAStar(quadTree);
        path = quadTreeAStar.FindPath(_startNode, _targetNode);

        return path;
    }

    private GPT_Grid grid;

    private FinishPathFindDelegate finishPathFindCallback = null;

    private GPT_QuadTree quadTree;
}