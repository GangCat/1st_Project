using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // �ش� ��尡 �̵� �������� �˱� ���� Ȯ���� ���̾�.
    // ������ ��尡 �ڸ��ϴ� ��ġ�� ���̾� ����ũ�� unWalkableMask�� ������Ʈ�� ������ �ش� ���� unwalkable�� �Ǵ� ���
    public LayerMask unWalkableMask;
    // �׸����� �� ũ��, ���� ũ���� �����ص� �ɵ�. ����� 128*128
    public Vector2 gridWorldSize;
    // ����� ������. ����� ũ�⸦ �����ϴ� ����̴�. ����� 0.5�� �����Ǿ��ִ�.
    public float nodeRadius;
    Node[,] grid;

    // diameter = ����
    float nodeDiameter;
    // size��� �صξ����� �����δ� ���� grid���� ��������� ������ �� ����, �� �����̴�.
    // Node�� 2���� �迭�� grid�� ũ�⸦ �����ϱ� ���� ����
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
        // grid�� �߽��� 0�̴ϱ� �� percentX�� 0.5�϶� worldPos.x�� 0�� �ƴ϶� gridWorldSize.x * 0.5f�ϱ� �̷��� �ۼ���.
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
