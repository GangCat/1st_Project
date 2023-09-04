using System.Collections.Generic;
using UnityEngine;

public class MapDivision : MonoBehaviour
{
    public int mapWidth = 100;       // 전체 지도의 너비
    public int mapHeight = 100;      // 전체 지도의 높이
    public int regionWidth = 20;     // 영역의 너비
    public int regionHeight = 20;    // 영역의 높이
    public LayerMask unWalkableMask; // Walkable 여부를 판단할 레이어 마스크

    private List<Region> regions;    // 분할된 영역 리스트
    private Dictionary<string, List<string>> adjacency; // 인접 관계

    private void Start()
    {
        // 지도 분할 및 인접 관계 설정
        regions = DivideMap(mapWidth, mapHeight, regionWidth, regionHeight);
        adjacency = SetAdjacency(regions);

        // 결과 출력 (디버그용)
#if UNITY_EDITOR
        foreach (Region region in regions)
        {
            Debug.Log($"Region ID: {region.Id}, X: {region.X}, Y: {region.Y}, Width: {region.Width}, Height: {region.Height}, IsWalkable: {region.IsWalkable}");
            Debug.Log("Adjacent Regions: " + string.Join(", ", adjacency[region.Id]));
        }
#endif
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (regions != null)
        {
            foreach (Region region in regions)
            {
                region.DrawGizmos();
            }
        }
    }
#endif
    private List<Region> DivideMap(int mapWidth, int mapHeight, int regionWidth, int regionHeight)
    {
        List<Region> regions = new List<Region>();

        for (int x = 0; x < mapWidth; x += regionWidth)
        {
            for (int y = 0; y < mapHeight; y += regionHeight)
            {
                Vector3 worldPos = new Vector3(x + regionWidth / 2, 0, y + regionHeight / 2);
                bool walkable = !Physics.CheckSphere(worldPos, regionWidth / 2, unWalkableMask);

                if (walkable)
                {
                    Region region = new Region
                    {
                        X = x,
                        Y = y,
                        Width = regionWidth,
                        Height = regionHeight,
                        IsWalkable = walkable
                    };
                    regions.Add(region);
                }
            }
        }

        return regions;
    }

    private Dictionary<string, List<string>> SetAdjacency(List<Region> regions)
    {
        Dictionary<string, List<string>> adjacency = new Dictionary<string, List<string>>();

        foreach (Region region in regions)
        {
            adjacency[region.Id] = new List<string>();

            foreach (Region otherRegion in regions)
            {
                if (IsAdjacent(region, otherRegion))
                {
                    adjacency[region.Id].Add(otherRegion.Id);
                }
            }
        }

        return adjacency;
    }

    private bool IsAdjacent(Region region1, Region region2)
    {
        return
            region1.X + region1.Width == region2.X ||
            region2.X + region2.Width == region1.X ||
            region1.Y + region1.Height == region2.Y ||
            region2.Y + region2.Height == region1.Y;
    }
}

public class Region
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsWalkable { get; set; } // Walkable 여부

    public string Id => $"({X},{Y})";

#if UNITY_EDITOR
    public void DrawGizmos()
    {
        Gizmos.color = IsWalkable ? Color.green : Color.red;
        Gizmos.DrawCube(new Vector3(X + Width / 2, 0, Y + Height / 2), new Vector3(Width, 0.1f, Height));
    }
#endif
}
