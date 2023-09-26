using System.Collections.Generic;
using System;
using UnityEngine;

public class GPT_PathRequestManager : MonoBehaviour
{
    public void Init()
    {
        instance = this;
        pathFinding = GetComponent<GPT_PathFinding>();
        pathFinding.Init(FinishedProcessingPath);
    }

    public static void RequestPath(Vector3 _pathStart, Vector3 _pathEnd, Action<GPT_Node[], bool> _callback)
    {
        SPathRequest newRequest = new SPathRequest(_pathStart, _pathEnd, _callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private struct SPathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<GPT_Node[], bool> callback;

        public SPathRequest(Vector3 _start, Vector3 _end, Action<GPT_Node[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

    private void FinishedProcessingPath(GPT_Node[] path, bool success)
    {
        curPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            curPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;

            // Find the start and end nodes using the quadtree
            GPT_Node startNode = grid.GetNodeFromWorldPoint(curPathRequest.pathStart);
            GPT_Node endNode = grid.GetNodeFromWorldPoint(curPathRequest.pathEnd);

            // Perform A* search within the quadtree
            pathFinding.StartFindPath(startNode, endNode);
        }
    }

    private Queue<SPathRequest> pathRequestQueue = new Queue<SPathRequest>();
    private SPathRequest curPathRequest;

    private static GPT_PathRequestManager instance;
    private GPT_PathFinding pathFinding;
    private GPT_Grid grid;

    private bool isProcessingPath;
}
