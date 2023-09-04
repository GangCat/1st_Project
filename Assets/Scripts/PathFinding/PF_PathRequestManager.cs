using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PF_PathRequestManager : MonoBehaviour
{
    public delegate void FinishPathFindingDelegate();


    private struct SPathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public SPathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

    private Queue<SPathRequest> pathRequestQueue = new Queue<SPathRequest>();
    private SPathRequest curPathRequest;

    public static PF_PathRequestManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<PF_PathRequestManager>();
                if(instance == null)
                {
                    GameObject pf_PathRequestmanager = new GameObject("PF_PathRequestManager");
                    instance = pf_PathRequestmanager.AddComponent<PF_PathRequestManager>();
                }
            }
            return instance;
        }
    }

    private static PF_PathRequestManager instance;
    private PF_PathFinding pathFinding;

    private bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PF_PathFinding>();
        pathFinding.Init(FinishedProcessingPath);
    }

    private PF_PathRequestManager() { }


    public static void RequestPath(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback)
    {
        SPathRequest newRequest = new SPathRequest(_pathStart, _pathEnd, _callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
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

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        curPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

}
