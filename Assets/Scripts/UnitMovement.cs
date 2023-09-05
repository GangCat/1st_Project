using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public void Init()
    {
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        StopCoroutine("FollowTargetCoroutine");
        SetPath(_targetPos);
    }

    public void FollowTarget(Transform _targetTr)
    {
        StopCoroutine("FollowTargetCoroutine");
        StartCoroutine("FollowTargetCoroutine", _targetTr);
    }

    private void SetPath(Vector3 _targetPos)
    {
        PF_PathRequestManager.RequestPath(transform.position, _targetPos, OnPathFound);
    }

    private void OnPathFound(Vector3[] _newPath, bool _pathSuccessful)
    {
        if (_pathSuccessful)
        {
            path = _newPath;
            targetIdx = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 curWaypoint = path[0];

        while (true)
        {
            if (transform.position == curWaypoint)
            {
                ++targetIdx;
                if (targetIdx >= path.Length)
                {
                    yield break;
                }

                curWaypoint = path[targetIdx];
            }

            transform.position = Vector3.MoveTowards(transform.position, curWaypoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator FollowTargetCoroutine(Transform _targetTr)
    {
        while (true)
        {

            if (Vector3.SqrMagnitude(transform.position - _targetTr.position) > followOffset)
            {
                Vector3 moveDir = (_targetTr.position - transform.position).normalized;

                transform.rotation = Quaternion.LookRotation(_targetTr.position - transform.position);

                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIdx; i < path.Length; ++i)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.4f);

                if (i == targetIdx)
                    Gizmos.DrawLine(transform.position, path[i]);
                else
                    Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float followOffset = 3f;

    private int targetIdx;

    private Vector3[] path;
}
