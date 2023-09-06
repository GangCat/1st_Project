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
        StopAllCoroutines();
        SetPath(_targetPos);
    }

    public void FollowTarget(Transform _targetTr)
    {
        StopAllCoroutines();
        StartCoroutine("FollowTargetCoroutine", _targetTr);
    }

    private void SetPath(Vector3 _targetPos)
    {
        if (!Physics.Linecast(transform.position, _targetPos))
        {
            StopAllCoroutines();
            StartCoroutine("MoveToTargetPosDirect", _targetPos);
        }
        else
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

    private IEnumerator FollowTargetCoroutine(Transform _targetTr)
    {
        Vector3 moveDir = Vector3.zero;

        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - _targetTr.position) > followOffset)
            {
                moveDir = (_targetTr.position - transform.position).normalized;

                transform.rotation = Quaternion.LookRotation(_targetTr.position - transform.position);

                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
            yield return null;
        }
    }

    private IEnumerator MoveToTargetPosDirect(Vector3 _targetPos)
    {
        Vector3 moveDir = (_targetPos - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(moveDir);
        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - _targetPos) < 0.1f)
                yield break;

            transform.position += moveDir * moveSpeed * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 curWaypoint = path[0];
        Vector3 moveDir = curWaypoint - transform.position;

        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - curWaypoint) < 0.01f)
            {
                ++targetIdx;
                if (targetIdx >= path.Length)
                    yield break;

                curWaypoint = path[targetIdx];
                moveDir = curWaypoint - transform.position;
                transform.rotation = Quaternion.LookRotation(curWaypoint - transform.position);
            }

            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
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
    private PF_Node[] pathNode;
}
