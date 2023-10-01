using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GPT_Grid : MonoBehaviour
{
    public int MaxSize => gridSizeX * gridSizeY;

    private GPT_QuadTree quadTree = null;

    public void Init()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

        // Initialize the quadtree with the entire grid bounds
        quadTree = new GPT_QuadTree(new Rect(transform.position.x, transform.position.z, gridWorldSize.x, gridWorldSize.y), 16);
    }

    private void CreateGrid()
    {
        // �׸��忡 2���� ��� �迭 ���� �Ҵ�
        grid = new GPT_Node[gridSizeX, gridSizeY];
        // �׸����� �߽��� 0,0�� �ִٰ� �����ϰ� ���ϴ��� ��ǥ�� ����.
        Vector3 worldBottomLeft = transform.position - (Vector3.right * (gridWorldSize.x * 0.5f)) - (Vector3.forward * (gridWorldSize.y * 0.5f));

        // grid�� ���ϴܿ������� �ϳ��� ��带 �Ҵ����ִ� �ݺ���
        // ���� 2�� �ݺ����̶� �������� ������ ����
        // �񱳸� �� �����Ѵ�.
        //int idx = 0;
        int maxNodeCount = gridSizeX * gridSizeY;
        int idxX = 0;
        int idxY = 0;
        for (int idx = 0; idx < maxNodeCount; idx++)
        {
            idxX = idx % gridSizeX;
            idxY = idx / gridSizeY;
            Vector3 worldPos = worldBottomLeft + Vector3.right * (idxX * nodeDiameter + nodeRadius) + Vector3.forward * (idxY * nodeDiameter + nodeRadius);
            bool walkable = !Physics.CheckSphere(worldPos, nodeRadius, unWalkableMask);
            GPT_Node node = new GPT_Node(walkable, worldPos, idxX, idxY);
            grid[idxX, idxY] = node;

            // Insert the node into the quadtree
            quadTree.Insert(node);
        }
    }

    /// <summary>
    /// �ش� Pos�� ��带 ��ȯ�ϴ� �Լ�.
    /// </summary>
    /// <param name="_worldPos"></param>
    /// <returns></returns>
    public GPT_Node GetNodeFromWorldPoint(Vector3 _worldPos)
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
    public List<GPT_Node> GetNeighbors(GPT_Node _curNode)
    {
        List<GPT_Node> neighbours = new List<GPT_Node>();

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

    public GPT_Node GetAccessibleNode(GPT_Node _targetNode)
    {
        Queue<GPT_Node> queue = new Queue<GPT_Node>();
        HashSet<GPT_Node> visited = new HashSet<GPT_Node>();

        queue.Enqueue(_targetNode);

        while (queue.Count > 0)
        {
            GPT_Node currentNode = queue.Dequeue();

            if (currentNode.walkable)
            {
                return currentNode;
            }

            List<GPT_Node> neighbors = GetNeighbors(currentNode);
            foreach (GPT_Node neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return null;
    }

    public void UpdateNodeWalkable(GPT_Node _node, bool _isWalkable)
    {
        _node.walkable = _isWalkable;
    }

    public bool GetNodeIsWalkable(int _gridX, int _gridY)
    {
        return grid[_gridX, _gridY].walkable;
    }

    public GPT_Node GetNodeWithGrid(int _gridX, int _gridY)
    {
        return grid[_gridX, _gridY];
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));
        if (grid != null && displayUnwalkableNodeGizmos)
        {
            foreach (GPT_Node node in grid)
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

    private GPT_Node[,] grid;
}
