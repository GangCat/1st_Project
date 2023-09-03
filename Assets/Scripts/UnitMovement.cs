using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public void MoveToTargetPos(Vector3 _targetPos)
    {
        StopCoroutine("FollowToTargetTrCoroutine");
        StopCoroutine("MoveToTargetPosCoroutine");
        StartCoroutine("MoveToTargetPosCoroutine", _targetPos);
    }

    private IEnumerator MoveToTargetPosCoroutine(Vector3 _targetPos)
    {
        Vector3 moveDir = (_targetPos - transform.position).normalized;
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.1f);
            if (Vector3.SqrMagnitude(transform.position - _targetPos) < 0.01f)
            {
                transform.position = _targetPos;
                yield break;
            }
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            yield return null;
        }
    }

    public void FollowToTargetTr(Transform _targetTr)
    {
        StopCoroutine("MoveToTargetPosCoroutine");
        StopCoroutine("FollowToTargetTrCoroutine");
        StartCoroutine("FollowToTargetTrCoroutine", _targetTr);
    }

    private IEnumerator FollowToTargetTrCoroutine(Transform _targetTr)
    {
        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - _targetTr.position) > followOffset)
            {
                Vector3 moveDir = (_targetTr.position - transform.position).normalized;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.1f);
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float followOffset = 3f;
}
