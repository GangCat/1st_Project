using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// �ð� �����Ϸ��� �߰��� ���ӽ����̽�
using System.Diagnostics;
using System;
using UnityEditor.Experimental.GraphView;

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
        StartCoroutine(FindPath(_startPos, _targetPos));
    }

    private IEnumerator FindPath(Vector3 _startPos, Vector3 _targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] arrWaypoint = new Vector3[0];
        bool isPathSuccess = false;

        // ���� ��ġ�� ���� ���� ��ġ�� ��带 ã�ƿͼ� ����.
        PF_Node startNode = grid.NodeFromWorldPoint(_startPos);
        PF_Node targetNode = grid.NodeFromWorldPoint(_targetPos);

        // Ÿ���� walkable�� �ƴϸ� �켱�� ���ϰ� ������ walkable�ΰͱ����� ������
        //while(!targetNode.walkable)
        //{
        //    List<PF_Node> listNeighborNode = grid.GetNeighbours(targetNode);
        //    foreach (PF_Node node in listNeighborNode)
        //    {
        //        if(node.walkable)
        //        {
        //            targetNode = node;
        //            break;
        //        }
        //    }
        //}

        if (targetNode.walkable)
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
                if (curNode.Equals(targetNode))
                {
                    sw.Stop();
                    print("path found: " + sw.ElapsedMilliseconds + "ms");
                    isPathSuccess = true;
                    break;
                }

                foreach (PF_Node neighborNode in grid.GetNeighbors(curNode))
                {
                    if (!neighborNode.walkable || closedSet.Contains(neighborNode)) continue;

                    int newGCostToNeighbor = curNode.gCost + CalcLowestCostWithNode(curNode, neighborNode);

                    // ���� neighbourNode�� ������ �ִ� gCost ���� �츮�� ������ ����� �ش� ����� gCost���� ũ�� �ش� ����� �ڽ�Ʈ�� ��������� �Ѵ�.
                    // �׸��� �׿� ������� ���� openSet�� ������ �ʴٸ� gCost�� ����ؼ� �־��ְ� openSet�� �־���� �Ѵ�.
                    if (newGCostToNeighbor < neighborNode.gCost || !openSet.Contains(neighborNode))
                    {
                        neighborNode.gCost = newGCostToNeighbor;
                        neighborNode.hCost = CalcLowestCostWithNode(neighborNode, targetNode);
                        neighborNode.parentNode = curNode;

                        if (!openSet.Contains(neighborNode))
                            openSet.Add(neighborNode);
                        else
                            openSet.UpdateItem(neighborNode);
                    }
                }
            }
        }

        yield return null;

        if (isPathSuccess)
        {
            arrWaypoint = RetracePath(startNode, targetNode);
            UnityEngine.Debug.Log("true");
        }

        finishPathFindCallback?.Invoke(arrWaypoint, isPathSuccess);
    }

    /// <summary>
    /// �ش� ����� �θ� Ÿ�� �ö󰡼� ��θ� ��Ž���ϰ� �������� �ٽ� ������ ����� ��θ� ������.
    /// </summary>
    /// <param name="_startNode"></param>
    /// <param name="_endNode"></param>
    private Vector3[] RetracePath(PF_Node _startNode, PF_Node _endNode)
    {
        List<PF_Node> path = new List<PF_Node>();
        PF_Node curNode = _endNode;

        while (!curNode.Equals(_startNode))
        {
            path.Add(curNode);
            curNode = curNode.parentNode;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
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
    /// ��Ŭ����� �Ÿ� ���
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
}