using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public void MoveToTargetPos(Vector3 _targetPos)
    {
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

    [SerializeField]
    private float moveSpeed = 5f;
}
