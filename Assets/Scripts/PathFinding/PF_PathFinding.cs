using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// 시간 측정하려고 추가한 네임스페이스
using System.Diagnostics;
using System;

public class PF_PathFinding : MonoBehaviour
{
    public delegate void FinishPathFindDelegate(Vector3[] _waypoints, bool _ispathSuccess);

    public void Init(FinishPathFindDelegate _finishPathFindCallback)
    {
        grid = GetComponent<PF_Grid>();
        grid.Init();
        finishPathFindCallback = _finishPathFindCallback;
    }

    public void StartFindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        if (pathFindCoroutine != null)
            StopCoroutine(pathFindCoroutine);
        pathFindCoroutine = StartCoroutine(FindPath(_startPos, _targetPos));
    }

    private IEnumerator FindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        // 시작 위치의 노드와 도착 위치의 노드를 찾아와서 저장.
        PF_Node startNode = grid.NodeFromWorldPoint(_startPos);
        PF_Node targetNode = grid.NodeFromWorldPoint(_targetPos);

        if (/*startNode.walkable && */targetNode.walkable)
        {
            PF_Heap<PF_Node> openSet = new PF_Heap<PF_Node>(grid.MaxSize);
            // haseSet: key값 없이 value 그 자체로 key가 된다.
            // 즉 value의 중복을 허용하지 않는다.
            HashSet<PF_Node> closedSet = new HashSet<PF_Node>();
            openSet.Add(startNode);

            // 해당 조건은 openSet이 없을 때 즉 경로가 없을 때 탈출한다는 조건이다.
            while (openSet.Count > 0)
            {
                // 먼저 현재 비교할 노드를 가져오는데 리스트의 첫번째에 위치한 노드를 가져온다.
                PF_Node curNode = openSet.RemoveFirstItem();

                // 아래 주석처리된 코드는 Heap을 사용하면서 쓸모가 없어짐. Heap에서 가장 우선순위가 높은 애를 firstitem으로 꺼내줌
                //// openSet에 있는 모든 노드 중 가장 fCost가 적은 노드를 제거하는 내용(꺼내서 비교하기 위함)
                //for (int i = 1; i < openSet.Count; ++i)
                //{
                //    // fCost가 더 적은 노드를 찾으면 해당 노드로 curNode를 변경
                //    // 또한 fCost가 같을 때 hCost(heuristic)가 더 낮은 즉 도착지점과의 거리가 더 가까운 노드를 찾아도 해당 노드로 curNode를 변경 
                //    // 매우 끔찍한 최적화라고 함. 나중에 4 5챕터에서 수정할 예정(heap 이용)
                //    if (openSet[i].fCost < curNode.fCost || openSet[i].fCost == curNode.fCost)
                //    {
                //        if (openSet[i].hCost < curNode.hCost)
                //            curNode = openSet[i];
                //    }
                //}
                //openSet.Remove(curNode);
                closedSet.Add(curNode);

                // 도착했다면
                if (curNode == targetNode)
                {
                    sw.Stop();
                    print("path found: " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach (PF_Node neighbourNode in grid.GetNeighbours(curNode))
                {
                    if (!neighbourNode.walkable || closedSet.Contains(neighbourNode)) continue;

                    int newGCostToNeighbour = curNode.gCost + GetDistance(curNode, neighbourNode);

                    // 지금 neighbourNode가 가지고 있는 gCost 값이 우리가 실제로 계산한 해당 노드의 gCost보다 크면 해당 노드의 코스트를 갱신해줘야 한다.
                    // 그리고 그와 상관없이 아직 openSet에 들어가있지 않다면 gCost를 계산해서 넣어주고 openSet에 넣어줘야 한다.
                    if (newGCostToNeighbour < neighbourNode.gCost || !openSet.Contains(neighbourNode))
                    {
                        neighbourNode.gCost = newGCostToNeighbour;
                        neighbourNode.hCost = GetDistance(neighbourNode, targetNode);
                        neighbourNode.parentNode = curNode;

                        if (!openSet.Contains(neighbourNode))
                            openSet.Add(neighbourNode);
                        else
                            openSet.UpdateItem(neighbourNode);
                    }
                }
            }
        }

        yield return null;
        grid.ResetNode();

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            UnityEngine.Debug.Log("true");
        }

        finishPathFindCallback?.Invoke(waypoints, pathSuccess);
    }

    /// <summary>
    /// 해당 노드의 부모를 타고 올라가서 경로를 역탐색하고 마지막에 다시 순서를 뒤집어서 경로를 설정함.
    /// </summary>
    /// <param name="_startNode"></param>
    /// <param name="_endNode"></param>
    private Vector3[] RetracePath(PF_Node _startNode, PF_Node _endNode)
    {
        //Stack<PF_Node> stack = new Stack<PF_Node>();
        List<PF_Node> path = new List<PF_Node>();
        PF_Node curNode = _endNode;

        while (curNode != _startNode)
        {
            //stack.Push(curNode);
            path.Add(curNode);
            curNode = curNode.parentNode;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
        //grid.path = path;
    }

    private Vector3[] SimplifyPath(List<PF_Node> _path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < _path.Count; ++i)
        {
            // 지금 검사할 노드가 향하는 방향이 동일하면 굳이 리스트에 넣지 않음. 최적화
            Vector2 directionNew = new Vector2(_path[i - 1].gridX - _path[i].gridX, _path[i - 1].gridY - _path[i].gridY);
            if (directionNew != directionOld)
                waypoints.Add(_path[i].worldPos);

            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    /// <summary>
    /// nodeA에서 nodeB로 가는 최단거리를 임의로 계산해서 그 값을 반환하는 함수.
    /// </summary>
    /// <param name="_nodeA"></param>
    /// <param name="_nodeB"></param>
    /// <returns></returns>
    private int GetDistance(PF_Node _nodeA, PF_Node _nodeB)
    {
        int distX = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int distY = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    private PF_Grid grid;

    private FinishPathFindDelegate finishPathFindCallback = null;
    private Coroutine pathFindCoroutine = null;
}