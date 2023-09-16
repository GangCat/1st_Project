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
        // �׸��忡 2���� ��� �迭 ���� �Ҵ�
        grid = new PF_Node[gridSizeX, gridSizeY];
        // �׸����� �߽��� 0,0�� �ִٰ� �����ϰ� ���ϴ��� ��ǥ�� ����.
        Vector3 worldBottomLeft = transform.position - (Vector3.right * (gridWorldSize.x * 0.5f)) - (Vector3.forward * (gridWorldSize.y * 0.5f));

        // grid�� ���ϴܿ������� �ϳ��� ��带 �Ҵ����ִ� �ݺ���
        // ���� 2�� �ݺ����̶� �������� ������ ����
        // �񱳸� �� �����Ѵ�.
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
    /// �ش� Pos�� ��带 ��ȯ�ϴ� �Լ�.
    /// </summary>
    /// <param name="_worldPos"></param>
    /// <returns></returns>
    public PF_Node GetNodeFromWorldPoint(Vector3 _worldPos)
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

    /// <summary>
    /// _curNode�� ������ ��� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="_curNode"></param>
    /// <returns></returns>
    public List<PF_Node> GetNeighbors(PF_Node _curNode)
    {
        List<PF_Node> neighbours = new List<PF_Node>();

        // _curNode�������� 3by3��ġ�� ������ ��ȯ�ϱ� ���� �ݺ���
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
    // �ش� ��尡 �̵� �������� �˱� ���� Ȯ���� ���̾�.
    // ������ ��尡 �ڸ��ϴ� ��ġ�� ���̾� ����ũ�� unWalkableMask�� ������Ʈ�� ������ �ش� ���� unwalkable�� �Ǵ� ���
    [SerializeField]
    private LayerMask unWalkableMask;
    [SerializeField]
    // �׸����� �� ũ��, ���� ũ���� �����ص� �ɵ�. ����� 128*128
    private Vector2 gridWorldSize;
    [SerializeField]
    // ����� ������. ����� ũ�⸦ �����ϴ� ����̴�. ����� 0.5�� �����Ǿ��ִ�.
    private float nodeRadius;

    // diameter = ����
    private float nodeDiameter;
    // size��� �صξ����� �����δ� ���� grid���� ��������� ������ �� ����, �� �����̴�.
    // Node�� 2���� �迭�� grid�� ũ�⸦ �����ϱ� ���� ����
    private int gridSizeX, gridSizeY;

    private PF_Node[,] grid;
}
