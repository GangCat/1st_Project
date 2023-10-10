using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PF_PathRequestManager : MonoBehaviour
{
    public void Init(float _gridWorldSizeX, float _gridWorldSizeY)
    {
        instance = this;
        pathFinding = GetComponent<PF_PathFinding>();
        pathFinding.Init(FinishedProcessingPath, _gridWorldSizeX, _gridWorldSizeY);
    }

    public static void RequestPath(Vector3 _pathStart, Vector3 _pathEnd, Action<PF_Node[], bool> _callback)
    {
        SPathRequest newRequest = new SPathRequest(_pathStart, _pathEnd, _callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }


    private struct SPathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<PF_Node[], bool> callback;

        public SPathRequest(Vector3 _start, Vector3 _end, Action<PF_Node[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

    private void FinishedProcessingPath(PF_Node[] path, bool success)
    {
        curPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    

    private void TryProcessNext()
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            curPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(curPathRequest.pathStart, curPathRequest.pathEnd);
        }
    }


    private Queue<SPathRequest> pathRequestQueue = new Queue<SPathRequest>();
    private SPathRequest curPathRequest;

    private static PF_PathRequestManager instance;
    private PF_PathFinding pathFinding;

    private bool isProcessingPath;
}
