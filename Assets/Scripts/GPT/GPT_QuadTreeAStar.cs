using System.Collections.Generic;
using UnityEngine;

public class GPT_QuadTreeAStar
{
    private GPT_QuadTree quadTree;

    public GPT_QuadTreeAStar(GPT_QuadTree _quadTree)
    {
        quadTree = _quadTree;
    }

    public List<GPT_Node> FindPath(GPT_Node _startNode, GPT_Node _targetNode)
    {
        List<GPT_Node> openSet = new List<GPT_Node>();
        HashSet<GPT_Node> closedSet = new HashSet<GPT_Node>();

        openSet.Add(_startNode);

        while (openSet.Count > 0)
        {
            GPT_Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost ||
                    (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.Equals(_targetNode))
            {
                return RetracePath(_startNode, _targetNode);
            }

            foreach (GPT_Node neighbor in quadTree.RetrieveNodesInRegion(new Rect(currentNode.worldPos.x - 1, currentNode.worldPos.z - 1, 2, 2)))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + CalculateDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = CalculateDistance(neighbor, _targetNode);
                    neighbor.parentNode = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<GPT_Node> RetracePath(GPT_Node _startNode, GPT_Node _endNode)
    {
        List<GPT_Node> path = new List<GPT_Node>();
        GPT_Node currentNode = _endNode;

        while (!currentNode.Equals(_startNode))
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistance(GPT_Node a, GPT_Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);
        return 10 * (distX + distY);
    }
}
