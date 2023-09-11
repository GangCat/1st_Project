using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public void Init()
    {
    }

    //public void MoveByTargetPos(Vector3 _targetPos)
    //{
    //    StopMoveCoroutines();
    //    SetPath(_targetPos);
    //}

    public void FollowTarget(Transform _targetTr)
    {
        StopMoveCoroutines();
        StartCoroutine("FollowTargetCoroutine", _targetTr);
    }

    public void Stop()
    {
        StopMoveCoroutines();
    }

    //private IEnumerator UpdateNodeStateCoroutine()
    //{
    //    yield return new WaitForEndOfFrame();
    //    PF_Node prevNode = grid.NodeFromWorldPoint(transform.position);
    //    PF_Node curNode = null;
    //    while (true)
    //    {
    //        curNode = grid.NodeFromWorldPoint(transform.position);
    //        grid.UpdateNodeWalkable(curNode, false);

    //        if(!curNode.Equals(prevNode))
    //            grid.UpdateNodeWalkable(prevNode, true);

    //        prevNode = curNode;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //private void SetPath(Vector3 _targetPos)
    //{
    //    PF_PathRequestManager.RequestPath(transform.position, _targetPos, OnPathFound);
    //}

    //private void OnPathFound(PF_Node[] _newPath, bool _pathSuccessful)
    //{
    //    if (_pathSuccessful)
    //    {
    //        path = _newPath;
    //        targetIdx = 0;
    //        StopCoroutine("FollowPathCoroutine");
    //        StartCoroutine("FollowPathCoroutine");
    //    }
    //}

    private IEnumerator FollowTargetCoroutine(Transform _targetTr)
    {
        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - _targetTr.position) > followOffset)
            {
                //SetPath(_targetTr.position);
            }
            yield return new WaitForSeconds(1f);
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

    //private IEnumerator FollowPathCoroutine()
    //{
    //    PF_Node curWayNode = path[0];
        
    //    //Vector3 moveDir = (curWayNode.worldPos - transform.position).normalized;
    //    //transform.rotation = Quaternion.LookRotation(moveDir);
    //    while (true)
    //    {
    //        if (Vector3.SqrMagnitude(transform.position - curWayNode.worldPos) < 0.01f)
    //        {
    //            ++targetIdx;
    //            if (targetIdx >= path.Length)
    //            {
    //                yield break;
    //            }

    //            curWayNode = path[targetIdx];

    //            if (!curWayNode.walkable)
    //            {
    //                while (!path[path.Length - 1].walkable)
    //                    yield return null;

    //                SetPath(path[path.Length - 1].worldPos);
    //                yield break;
    //            }


    //            //moveDir = (curWayNode.worldPos - transform.position).normalized;
    //            //transform.rotation = Quaternion.LookRotation(curWayNode.worldPos - transform.position);
    //        }

    //        Debug.DrawLine(transform.position, curWayNode.worldPos, Color.red, 1f);

    //        if(Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
    //        {
    //            transform.Translate(Vector3.zero);
    //            yield return new WaitForSeconds(0.5f);
    //            //SetPath(path[path.Length - 1].worldPos);
    //            //yield break;
    //        }

    //        transform.rotation = Quaternion.LookRotation(curWayNode.worldPos - transform.position);
    //        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    //        yield return null;
    //    }
    //}

    //private void OnCollisionEnter(Collision _other)
    //{
    //    if (_other.transform.CompareTag("FriendlyUnit") && path != null)
    //    {
    //        StartCoroutine("ReStartPath");
    //    }
    //}

    //private IEnumerator ReStartPath()
    //{
    //    StopCoroutine("FollowPathCoroutine");

    //    while (!path[path.Length - 1].walkable)
    //        yield return null;

    //    SetPath(path[path.Length - 1].worldPos);
    //}

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
