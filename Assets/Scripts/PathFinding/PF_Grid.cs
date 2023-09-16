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
                    Gizmos.DrawCube(node.worldPos, new Vector3(0.8f, 0.1f, 0.8f));
                }
            }
        }
    }


    [SerializeField]
    private bool displayUnwalkableNodeGizmos;
    // 해당 노드가 이동 가능한지 알기 위해 확인할 레이어.
    // 생성한 노드가 자리하는 위치에 레이어 마스크가 unWalkableMask인 오브젝트가 있으면 해당 노드는 unwalkable이 되는 방식
    [SerializeField]
    private LayerMask unWalkableMask;
    [SerializeField]
    // 그리드의 총 크기, 맵의 크기라고 생각해도 될듯. 현재는 128*128
    private Vector2 gridWorldSize;
    [SerializeField]
    // 노드의 반지름. 노드의 크기를 결정하는 요소이다. 현재는 0.5로 설정되어있다.
    private float nodeRadius;

    // diameter = 지름
    private float nodeDiameter;
    // size라고 해두었지만 실제로는 각각 grid내에 만들어지는 노드들의 열 개수, 행 개수이다.
    // Node형 2차원 배열인 grid의 크기를 설정하기 위한 변수
    private int gridSizeX, gridSizeY;

    private PF_Node[,] grid;
}
