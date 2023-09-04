using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // 해당 노드가 이동 가능한지 알기 위해 확인할 레이어.
    // 생성한 노드가 자리하는 위치에 레이어 마스크가 unWalkableMask인 오브젝트가 있으면 해당 노드는 unwalkable이 되는 방식
    public LayerMask unWalkableMask;
    // 그리드의 총 크기, 맵의 크기라고 생각해도 될듯. 현재는 128*128
    public Vector2 gridWorldSize;
    // 노드의 반지름. 노드의 크기를 결정하는 요소이다. 현재는 0.5로 설정되어있다.
    public float nodeRadius;
    Node[,] grid;

    // diameter = 지름
    float nodeDiameter;
    // size라고 해두었지만 실제로는 각각 grid내에 만들어지는 노드들의 열 개수, 행 개수이다.
    // Node형 2차원 배열인 grid의 크기를 설정하기 위한 변수
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * (gridWorldSize.x * 0.5f)) - (Vector3.forward * (gridWorldSize.y * 0.5f));

        for(int x = 0; x < gridSizeX; ++x)
        {
            for(int y = 0; y < gridSizeY; ++y)
            {
                Vector3 worldPos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPos, nodeRadius, unWalkableMask);
                grid[x, y] = new Node(walkable, worldPos);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 _worldPos)
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
