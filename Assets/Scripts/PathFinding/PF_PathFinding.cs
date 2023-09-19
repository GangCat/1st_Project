using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// 시간 측정하려고 추가한 네임스페이스
using System.Diagnostics;
using System;

public class PF_PathFinding : MonoBehaviour
{
    public delegate void FinishPathFindDelegate(PF_Node[] _waypoints, bool _isPathSuccess);

    public void Init(FinishPathFindDelegate _finishPathFindCallback)
    {
        grid = GetComponent<PF_Grid>();
        grid.Init();
        finishPathFindCallback = _finishPathFindCallback;

        openSet = new PF_Heap<PF_Node>(grid.MaxSize);
    }

    public void StartFindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        StartCoroutine(FindPath(_startPos, _targetPos));
    }

    private IEnumerator FindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        PF_Node[] arrWayNode = new PF_Node[0];
        bool isPathSuccess = false;

        PF_Node startNode = grid.GetNodeFromWorldPoint(_startPos);
        PF_Node targetNode = grid.GetNodeFromWorldPoint(_targetPos);

        if (!targetNode.walkable)
            targetNode = grid.GetAccessibleNode(targetNode);
        if (targetNode != null)
        {
            if (startNode.Equals(targetNode))
            {
                // 새로 설정한 targetNode가 startNode와 같아질 경우 pathSuccess를 실패로 전달하여 길찾기를 하지 못하게 함.
                finishPathFindCallback?.Invoke(arrWayNode, isPathSuccess);
                yield break;
            }

            while (openSet.Count > 0)
                openSet.RemoveFirstItem();
            closedSet.Clear();

            //PF_Heap<PF_Node> openSet = new PF_Heap<PF_Node>(grid.MaxSize);
            //// haseSet: key값 없이 value 그 자체로 key가 된다.
            //// 즉 value의 중복을 허용하지 않는다.
            //HashSet<PF_Node> closedSet = new HashSet<PF_Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PF_Node curNode = openSet.RemoveFirstItem();

                closedSet.Add(curNode);

                // 도착했다면
                if (curNode.Equals(targetNode))
                {
                    sw.Stop();
                    //print("path found: " + sw.ElapsedMilliseconds + "ms");
                    isPathSuccess = true;
                    break;
                }

                List<PF_Node> listNeighbor = grid.GetNeighbors(curNode);

                for (int i = 0; i < listNeighbor.Count; ++i)
                {
                    PF_Node neighbor = listNeighbor[i];
                    if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                    int newGCostToNeighbor = curNode.gCost + CalcLowestCostWithNode(curNode, neighbor);

                    if (newGCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newGCostToNeighbor;
                        neighbor.hCost = CalcLowestCostWithNode(neighbor, targetNode);
                        neighbor.parentNode = curNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                        else
                            openSet.UpdateItem(neighbor);
                    }
                }

                //foreach (PF_Node neighborNode in grid.GetNeighbors(curNode))
                //{
                //    if (!neighborNode.walkable || closedSet.Contains(neighborNode)) continue;

                //    int newGCostToNeighbor = curNode.gCost + CalcLowestCostWithNode(curNode, neighborNode);

                //    if (newGCostToNeighbor < neighborNode.gCost || !openSet.Contains(neighborNode))
                //    {
                //        neighborNode.gCost = newGCostToNeighbor;
                //        neighborNode.hCost = CalcLowestCostWithNode(neighborNode, targetNode);
                //        neighborNode.parentNode = curNode;

                //        if (!openSet.Contains(neighborNode))
                //            openSet.Add(neighborNode);
                //        else
                //            openSet.UpdateItem(neighborNode);
                //    }
                //}
            }
        }

        yield return null;

        if (isPathSuccess)
        {
            arrWayNode = RetracePath(startNode, targetNode);
            //UnityEngine.Debug.Log("true");
        }

        finishPathFindCallback?.Invoke(arrWayNode, isPathSuccess);
    }

    /// <summary>
    /// 해당 노드의 부모를 타고 올라가서 경로를 역탐색하고 마지막에 다시 순서를 뒤집어서 경로를 설정함.
    /// </summary>
    /// <param name="_startNode"></param>
    /// <param name="_endNode"></param>
    private PF_Node[] RetracePath(PF_Node _startNode, PF_Node _endNode)
    {
        List<PF_Node> path = new List<PF_Node>();
        PF_Node curNode = _endNode;

        while (!curNode.Equals(_startNode))
        {
            path.Add(curNode);
            curNode = curNode.parentNode;
        }

        //Vector3[] waypoints = SimplifyPath(path);
        path.Reverse();
        return path.ToArray();
    }

    /// <summary>
    /// 이동하는 경로를 부드럽게 만들어주는 함수.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    //private Vector3[] SimplifyPath(List<PF_Node> _path)
    //{
    //    List<Vector3> waypoints = new List<Vector3>();
    //    Vector2 directionOld = Vector2.zero;

    //    for (int i = 1; i < _path.Count; ++i)
    //    {
    //        // 지금 검사할 노드가 향하는 방향이 동일하면 굳이 리스트에 넣지 않음. 최적화
    //        Vector2 directionNew = new Vector2(_path[i - 1].gridX - _path[i].gridX, _path[i - 1].gridY - _path[i].gridY);
    //        if (directionNew != directionOld)
    //            waypoints.Add(_path[i].worldPos);

    //        directionOld = directionNew;
    //    }
    //    return waypoints.ToArray();
    //}

    /// <summary>
    /// nodeA에서 nodeB로 가는 최단거리를 임의로 계산해서 그 값을 반환하는 함수.
    /// 유클리디안 거리 사용
    /// </summary>
    /// <param name="_nodeA"></param>
    /// <param name="_nodeB"></param>
    /// <returns></returns>
    private int CalcLowestCostWithNode(PF_Node _nodeA, PF_Node _nodeB)
    {
        int distX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int distY = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    private PF_Grid grid;

    private FinishPathFindDelegate finishPathFindCallback = null;

    private PF_Heap<PF_Node> openSet = null;
    private HashSet<PF_Node> closedSet = new HashSet<PF_Node>();
}