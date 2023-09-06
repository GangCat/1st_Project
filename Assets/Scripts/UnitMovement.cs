using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public void Init()
    {
        StartCoroutine("UpdateNodeWalkableCoroutine");
        StartCoroutine("UpdateRigidbodyVelocityCoroutine");
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        StopMoveCoroutines();
        SetPath(_targetPos);
    }

    public void FollowTarget(Transform _targetTr)
    {
        StopMoveCoroutines();
        StartCoroutine("FollowTargetCoroutine", _targetTr);
    }

    private IEnumerator UpdateNodeWalkableCoroutine()
    {
        yield return new WaitForEndOfFrame();
        PF_Node prevNode = grid.NodeFromWorldPoint(transform.position);
        PF_Node curNode = null;
        while (true)
        {
            curNode = grid.NodeFromWorldPoint(transform.position);
            grid.UpdateNodeWalkable(curNode, false);

            if(!curNode.Equals(prevNode))
                grid.UpdateNodeWalkable(prevNode, true);

            prevNode = curNode;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator UpdateRigidbodyVelocityCoroutine()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        while (true)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            yield return null;
        }
    }

    private void SetPath(Vector3 _targetPos)
    {
        //if (!Physics.Linecast(transform.position, _targetPos))
        //{
        //    StopMoveCoroutines();
        //    StartCoroutine("MoveToTargetPosDirect", _targetPos);
        //}
        //else
            PF_PathRequestManager.RequestPath(transform.position, _targetPos, OnPathFound);
    }

    private void OnPathFound(PF_Node[] _newPath, bool _pathSuccessful)
    {
        if (_pathSuccessful)
        {
            path = _newPath;
            targetIdx = 0;
            StopMoveCoroutines();
            StartCoroutine("FollowPathCoroutine");
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

    //private IEnumerator MoveToTargetPosDirect(Vector3 _targetPos)
    //{
    //    Vector3 moveDir = (_targetPos - transform.position).normalized;
    //    transform.rotation = Quaternion.LookRotation(moveDir);
    //    while (true)
    //    {
    //        if (Vector3.SqrMagnitude(transform.position - _targetPos) < 0.1f)
    //            yield break;

    //        transform.position += moveDir * moveSpeed * Time.deltaTime;

    //        yield return null;
    //    }
    //}

    private IEnumerator FollowPathCoroutine()
    {
        PF_Node curWayNode = path[0];
        //Vector3 moveDir = (curWayNode.worldPos - transform.position).normalized;
        //transform.rotation = Quaternion.LookRotation(moveDir);
        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - curWayNode.worldPos) < 0.01f)
            {
                ++targetIdx;
                if (targetIdx >= path.Length)
                    yield break;

                curWayNode = path[targetIdx];

                if (!curWayNode.walkable)
                {
                    SetPath(path[path.Length - 1].worldPos);
                    yield break;
                }
                //moveDir = (curWayNode.worldPos - transform.position).normalized;
                //transform.rotation = Quaternion.LookRotation(curWayNode.worldPos - transform.position);
            }
            transform.rotation = Quaternion.LookRotation(curWayNode.worldPos - transform.position);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void StopMoveCoroutines()
    {
        StopCoroutine("FollowPathCoroutine");
        StopCoroutine("FollowTargetCoroutine");
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIdx; i < path.Length; ++i)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i].worldPos, Vector3.one * 0.4f);

                if (i == targetIdx)
                    Gizmos.DrawLine(transform.position, path[i].worldPos);
                else
                    Gizmos.DrawLine(path[i - 1].worldPos, path[i].worldPos);
            }
        }
    }

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float followOffset = 3f;

    [Header("-Test")]
    [SerializeField]
    private PF_Grid grid;

    private int targetIdx;

    private PF_Node[] path;

}
