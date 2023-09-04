using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Unit : MonoBehaviour
{
    public Transform targetTr;
    private float speed = 5f;
    private Vector3[] path;
    private int targetIdx;
    private PF_PathRequestManager pathRequest = null;

    private void Awake()
    {
        pathRequest = PF_PathRequestManager.Instance;
    }

    private void Start()
    {
        //PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
            PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] _newPath, bool _pathSuccessful)
    {
        if (_pathSuccessful)
        {
            path = _newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 curWaypoint = path[0];

        while (true)
        {
            if(transform.position == curWaypoint)
            {
                ++targetIdx;
                if (targetIdx >= path.Length)
                {
                    targetIdx = 0;
                    yield break;
                }

                curWaypoint = path[targetIdx];
            }

            transform.position = Vector3.MoveTowards(transform.position, curWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIdx; i < path.Length; ++i)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIdx)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                    Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }
}
