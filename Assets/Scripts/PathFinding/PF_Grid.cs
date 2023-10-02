using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PF_Grid : MonoBehaviour
{
    public int MaxSize => gridSizeX * gridSizeY;

    public void Init()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        // 그리드에 2차원 노드 배열 공간 할당
        grid = new PF_Node[gridSizeX, gridSizeY];
        // 그리드의 중심이 0,0에 있다고 가정하고 좌하단의 좌표를 구함.
        Vector3 worldBottomLeft = transform.position - (Vector3.right * (gridWorldSize.x * 0.5f)) - (Vector3.forward * (gridWorldSize.y * 0.5f));

        // grid에 좌하단에서부터 하나씩 노드를 할당해주는 반복문
        // 위에 2중 반복문이랑 비교했을때 장점이 뭘까
        // 비교를 좀 적게한다.
        int idx = 0;
        int maxNodeCount = gridSizeX * gridSizeY;
        int idxX = 0;
        int idxY = 0;

        while (idx < maxNodeCount)
        {
            idxX = idx % gridSizeX;
            idxY = idx / gridSizeY;
            Vector3 worldPos = worldBottomLeft + Vector3.right * (idxX * nodeDiameter + nodeRadius) + Vector3.forward * (idxY * nodeDiameter + nodeRadius);
            bool walkable = !Physics.CheckSphere(worldPos, nodeRadius, unWalkableMask);
            grid[idxX, idxY] = new PF_Node(walkable, worldPos, idxX, idxY);
            ++idx;
        }
    }

    /// <summary>
    /// 해당 Pos의 노드를 반환하는 함수.
    /// </summary>
    /// <param name="_worldPos"></param>
    /// <returns></returns>
    public PF_Node GetNodeFromWorldPoint(Vector3 _worldPos)
    {
        // grid의 중심이 0이니까 즉 percentX가 0.5일때 worldPos.x가 0이 아니라 gridWorldSize.x * 0.5f니까 이렇게 작성됨.
        float percentX = (_worldPos.x + gridWorldSize.x * 0.5f) / gridWorldSize.x;
        float percentY = (_worldPos.z + gridWorldSize.y * 0.5f) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    /// <summary>
    /// _curNode와 인접한 모든 노드들을 반환하는 함수
    /// </summary>
    /// <param name="_curNode"></param>
    /// <returns></returns>
    public List<PF_Node> GetNeighbors(PF_Node _curNode)
    {
        List<PF_Node> neighbours = new List<PF_Node>();

        // _curNode기준으로 3by3위치의 노드들을 반환하기 위한 반복문
        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (x == 0 && y == 0) continue;

                int checkX = _curNode.gridX + x;
                int checkY = _curNode.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }

    //public PF_Node GetAccessibleNode(PF_Node _targetNode)
    //{
    //    queueNotVisitedNode.Clear();
    //    hashSetVisitedNode.Clear();

    //    queueNotVisitedNode.Enqueue(_targetNode);

    //    while (queueNotVisitedNode.Count > 0)
    //    {
    //        listNeighborNode.Clear();
    //        PF_Node currentNode = queueNotVisitedNode.Dequeue();

    //        if (currentNode.walkable)
    //        {
    //            return currentNode;
    //        }

    //        listNeighborNode = GetNeighbors(currentNode);
    //        foreach (PF_Node neighbor in listNeighborNode)
    //        {
    //            if (!hashSetVisitedNode.Contains(neighbor))
    //            {
    //                hashSetVisitedNode.Add(neighbor);
    //                queueNotVisitedNode.Enqueue(neighbor);
    //            }
    //        }
    //    }

    //    return null;
    //}


    public PF_Node GetAccessibleNodeWithoutTargetNode(PF_Node _targetNode)
    {
        queueNotVisitedNode.Clear();
        hashSetVisitedNode.Clear();

        listNeighborNode.Clear();

        listNeighborNode = GetNeighbors(_targetNode);
        foreach (PF_Node neighbor in listNeighborNode)
        {
            if (!hashSetVisitedNode.Contains(neighbor))
            {
                hashSetVisitedNode.Add(neighbor);
                queueNotVisitedNode.Enqueue(neighbor);
            }
        }

        while (queueNotVisitedNode.Count > 0)
        {
            listNeighborNode.Clear();
            PF_Node currentNode = queueNotVisitedNode.Dequeue();

            if (currentNode.walkable)
            {
                return currentNode;
            }

            listNeighborNode = GetNeighbors(currentNode);
            foreach (PF_Node neighbor in listNeighborNode)
            {
                if (!hashSetVisitedNode.Contains(neighbor))
                {
                    hashSetVisitedNode.Add(neighbor);
                    queueNotVisitedNode.Enqueue(neighbor);
                }
            }
        }

        return null;
    }

    public void UpdateNodeWalkable(PF_Node _node, bool _isWalkable)
    {
        _node.walkable = _isWalkable;
    }

    public bool GetNodeIsWalkable(int _gridX, int _gridY)
    {
        return grid[_gridX, _gridY].walkable;
    }

    public PF_Node GetNodeWithGrid(int _gridX, int _gridY)
    {
        return grid[_gridX, _gridY];
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));
        if (grid != null && displayUnwalkableNodeGizmos)
        {
            foreach (PF_Node node in grid)
            {
                if (!node.walkable)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(node.worldPos, new Vector3(nodeRadius * 2, 0.1f, nodeRadius * 2));
                }
            }
        }
    }


    [SerializeField]
    private bool displayUnwalkableNodeGizmos;
    [SerializeField]
    private LayerMask unWalkableMask;
    [SerializeField]
    private Vector2 gridWorldSize;
    [SerializeField]
    private float nodeRadius;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private Queue<PF_Node> queueNotVisitedNode = new Queue<PF_Node>();
    private HashSet<PF_Node> hashSetVisitedNode = new HashSet<PF_Node>();
    private List<PF_Node> listNeighborNode = new List<PF_Node>();

    private PF_Node[,] grid;
}
