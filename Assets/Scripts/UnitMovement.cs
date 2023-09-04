using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public void Init()
    {
        //agent = GetComponent<NavMeshAgent>();
        //agent.speed = moveSpeed;
    }
    public void MoveByTargetPos(Vector3 _targetPos)
    {
        StopCoroutine("MoveByMoveVecCoroutine");
        StopCoroutine("MoveByTargetPosCoroutine");
        StopCoroutine("FollowTargetCoroutine");
        StartCoroutine("MoveByTargetPosCoroutine", _targetPos);
    }

    public void FollowTarget(Transform _targetTr)
    {
        StopCoroutine("MoveByMoveVecCoroutine");
        StopCoroutine("MoveByTargetPosCoroutine");
        StopCoroutine("FollowTargetCoroutine");
        StartCoroutine("FollowTargetCoroutine", _targetTr);
    }

    public void MoveByMoveVec(Vector3 _moveVec)
    {
        StopCoroutine("MoveByMoveVecCoroutine");
        StopCoroutine("MoveByTargetPosCoroutine");
        StopCoroutine("FollowTargetCoroutine");
        StartCoroutine("MoveByMoveVecCoroutine", _moveVec);
    }

    private IEnumerator MoveByMoveVecCoroutine(Vector3 _moveVec)
    {
        Vector3 targetPos = transform.position + _moveVec;

        while (true)
        {
            Functions.RotateYaw(transform, Mathf.Atan2(_moveVec.z, _moveVec.x) * Mathf.Rad2Deg);
            //transform.rotation = Quaternion.LookRotation(_moveVec);

            if (Vector3.SqrMagnitude(transform.position - targetPos) < 0.01f)
            {
                transform.position = targetPos;
                //agent.ResetPath();
                yield break;
            }

            transform.position += _moveVec.normalized * moveSpeed * Time.deltaTime;
            //agent.SetDestination(targetPos);
            //if (!agent.hasPath)
            //    yield break;
            yield return null;
        }
    }


    private IEnumerator MoveByTargetPosCoroutine(Vector3 _targetPos)
    {
        Vector3 moveDir = (_targetPos - transform.position).normalized;

        while (true)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.1f);
            transform.rotation = Quaternion.LookRotation(moveDir);

            if (Vector3.SqrMagnitude(transform.position - _targetPos) < 1.5f)
            {
                //agent.ResetPath();
                yield break;
            }

            transform.position += moveDir * moveSpeed * Time.deltaTime;
            //if (Vector3.SqrMagnitude(transform.position - _targetPos) < 1f)
            //{
            //    agent.ResetPath();
            //    yield break;
            //}
            //agent.SetDestination(_targetPos);
            yield return null;
        }
    }

    private IEnumerator FollowTargetCoroutine(Transform _targetTr)
    {
        while (true)
        {
            //if (Vector3.SqrMagnitude(transform.position - _targetTr.position) > followOffset)
            //    agent.SetDestination(_targetTr.position);
            //else
            //    agent.ResetPath();
            //agent.SetDestination(_targetTr.position);

            if (Vector3.SqrMagnitude(transform.position - _targetTr.position) > followOffset)
            {
                Vector3 moveDir = (_targetTr.position - transform.position).normalized;
                
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.1f);
                transform.rotation = Quaternion.LookRotation(_targetTr.position - transform.position);

                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
            yield return null;
        }
    }

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float followOffset = 3f;

    //private NavMeshAgent agent = null;
}
