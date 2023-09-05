using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// �ð� �����Ϸ��� �߰��� ���ӽ����̽�
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

        // ���� ��ġ�� ���� ���� ��ġ�� ��带 ã�ƿͼ� ����.
        PF_Node startNode = grid.NodeFromWorldPoint(_startPos);
        PF_Node targetNode = grid.NodeFromWorldPoint(_targetPos);

        if (/*startNode.walkable && */targetNode.walkable)
        {
            PF_Heap<PF_Node> openSet = new PF_Heap<PF_Node>(grid.MaxSize);
            // haseSet: key�� ���� value �� ��ü�� key�� �ȴ�.
            // �� value�� �ߺ��� ������� �ʴ´�.
            HashSet<PF_Node> closedSet = new HashSet<PF_Node>();
            openSet.Add(startNode);

            // �ش� ������ openSet�� ���� �� �� ��ΰ� ���� �� Ż���Ѵٴ� �����̴�.
            while (openSet.Count > 0)
            {
                // ���� ���� ���� ��带 �������µ� ����Ʈ�� ù��°�� ��ġ�� ��带 �����´�.
                PF_Node curNode = openSet.RemoveFirstItem();

                // �Ʒ� �ּ�ó���� �ڵ�� Heap�� ����ϸ鼭 ���� ������. Heap���� ���� �켱������ ���� �ָ� firstitem���� ������
                //// openSet�� �ִ� ��� ��� �� ���� fCost�� ���� ��带 �����ϴ� ����(������ ���ϱ� ����)
                //for (int i = 1; i < openSet.Count; ++i)
                //{
                //    // fCost�� �� ���� ��带 ã���� �ش� ���� curNode�� ����
                //    // ���� fCost�� ���� �� hCost(heuristic)�� �� ���� �� ������������ �Ÿ��� �� ����� ��带 ã�Ƶ� �ش� ���� curNode�� ���� 
                //    // �ſ� ������ ����ȭ��� ��. ���߿� 4 5é�Ϳ��� ������ ����(heap �̿�)
                //    if (openSet[i].fCost < curNode.fCost || openSet[i].fCost == curNode.fCost)
                //    {
                //        if (openSet[i].hCost < curNode.hCost)
                //            curNode = openSet[i];
                //    }
                //}
                //openSet.Remove(curNode);
                closedSet.Add(curNode);

                // �����ߴٸ�
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

                    // ���� neighbourNode�� ������ �ִ� gCost ���� �츮�� ������ ����� �ش� ����� gCost���� ũ�� �ش� ����� �ڽ�Ʈ�� ��������� �Ѵ�.
                    // �׸��� �׿� ������� ���� openSet�� ������ �ʴٸ� gCost�� ����ؼ� �־��ְ� openSet�� �־���� �Ѵ�.
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
    /// �ش� ����� �θ� Ÿ�� �ö󰡼� ��θ� ��Ž���ϰ� �������� �ٽ� ������ ����� ��θ� ������.
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
            // ���� �˻��� ��尡 ���ϴ� ������ �����ϸ� ���� ����Ʈ�� ���� ����. ����ȭ
            Vector2 directionNew = new Vector2(_path[i - 1].gridX - _path[i].gridX, _path[i - 1].gridY - _path[i].gridY);
            if (directionNew != directionOld)
                waypoints.Add(_path[i].worldPos);

            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    /// <summary>
    /// nodeA���� nodeB�� ���� �ִܰŸ��� ���Ƿ� ����ؼ� �� ���� ��ȯ�ϴ� �Լ�.
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