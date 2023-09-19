using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// �ð� �����Ϸ��� �߰��� ���ӽ����̽�
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
                // ���� ������ targetNode�� startNode�� ������ ��� pathSuccess�� ���з� �����Ͽ� ��ã�⸦ ���� ���ϰ� ��.
                finishPathFindCallback?.Invoke(arrWayNode, isPathSuccess);
                yield break;
            }

            while (openSet.Count > 0)
                openSet.RemoveFirstItem();
            closedSet.Clear();

            //PF_Heap<PF_Node> openSet = new PF_Heap<PF_Node>(grid.MaxSize);
            //// haseSet: key�� ���� value �� ��ü�� key�� �ȴ�.
            //// �� value�� �ߺ��� ������� �ʴ´�.
            //HashSet<PF_Node> closedSet = new HashSet<PF_Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PF_Node curNode = openSet.RemoveFirstItem();

                closedSet.Add(curNode);

                // �����ߴٸ�
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
    /// �ش� ����� �θ� Ÿ�� �ö󰡼� ��θ� ��Ž���ϰ� �������� �ٽ� ������ ����� ��θ� ������.
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
    /// �̵��ϴ� ��θ� �ε巴�� ������ִ� �Լ�.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    //private Vector3[] SimplifyPath(List<PF_Node> _path)
    //{
    //    List<Vector3> waypoints = new List<Vector3>();
    //    Vector2 directionOld = Vector2.zero;

    //    for (int i = 1; i < _path.Count; ++i)
    //    {
    //        // ���� �˻��� ��尡 ���ϴ� ������ �����ϸ� ���� ����Ʈ�� ���� ����. ����ȭ
    //        Vector2 directionNew = new Vector2(_path[i - 1].gridX - _path[i].gridX, _path[i - 1].gridY - _path[i].gridY);
    //        if (directionNew != directionOld)
    //            waypoints.Add(_path[i].worldPos);

    //        directionOld = directionNew;
    //    }
    //    return waypoints.ToArray();
    //}

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

    private PF_Heap<PF_Node> openSet = null;
    private HashSet<PF_Node> closedSet = new HashSet<PF_Node>();
}