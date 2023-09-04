using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public Transform startNode; // 시작 지점
    public Transform targetNode; // 도착 지점

    private List<Region> path; // 최단 경로

    private void Start()
    {
        path = FindPath(startNode.position, targetNode.position);

        if (path != null)
        {
            Debug.Log("Found path!");
            foreach (Region region in path)
            {
                Debug.Log($"Region ID: {region.Id}, X: {region.X}, Y: {region.Y}");
            }
        }
        else
        {
            Debug.Log("No path found.");
        }
    }

    private List<Region> FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        // 시작 및 도착 지점의 Region을 찾습니다.
        Region startRegion = GetRegionAtPosition(startPosition);
        Region targetRegion = GetRegionAtPosition(targetPosition);

        if (startRegion == null || targetRegion == null)
        {
            Debug.LogError("Start or target region not found.");
            return null;
        }

        // A* 알고리즘을 사용하여 최단 경로를 탐색합니다.
        List<Region> openSet = new List<Region>();
        HashSet<Region> closedSet = new HashSet<Region>();
        openSet.Add(startRegion);

        while (openSet.Count > 0)
        {
            Region currentRegion = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentRegion.FCost || (openSet[i].FCost == currentRegion.FCost && openSet[i].HCost < currentRegion.HCost))
                {
                    currentRegion = openSet[i];
                }
            }

            openSet.Remove(currentRegion);
            closedSet.Add(currentRegion);

            if (currentRegion == targetRegion)
            {
                return RetracePath(startRegion, targetRegion);
            }

            foreach (Region neighbor in currentRegion.AdjacentRegions)
            {
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentRegion.GCost + GetDistance(currentRegion, neighbor);

                if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, targetRegion);
                    neighbor.Parent = currentRegion;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        // 도달할 수 없는 경우
        return null;
    }

    private List<Region> RetracePath(Region startRegion, Region endRegion)
    {
        List<Region> path = new List<Region>();
        Region currentRegion = endRegion;

        while (currentRegion != startRegion)
        {
            path.Add(currentRegion);
            currentRegion = currentRegion.Parent;
        }

        path.Reverse();
        return path;
    }

    private Region GetRegionAtPosition(Vector3 position)
    {
        // 실제로 지도 상의 좌표를 올바른 Region으로 매핑해야 합니다.
        // 이 예제에서는 단순히 가장 가까운 Region을 반환합니다.
        float closestDistance = float.MaxValue;
        Region closestRegion = null;

        foreach (Region region in path)
        {
            float distance = Vector3.Distance(position, new Vector3(region.X + region.Width / 2, 0, region.Y + region.Height / 2));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestRegion = region;
            }
        }

        return closestRegion;
    }

    private int GetDistance(Region regionA, Region regionB)
    {
        int dstX = Mathf.Abs(regionA.X - regionB.X);
        int dstY = Mathf.Abs(regionA.Y - regionB.Y);
        return dstX + dstY;
    }
}
