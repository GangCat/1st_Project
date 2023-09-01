using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public void Init()
    {

    }

    public void MovePlayerByPicking(Vector3 _pickPos)
    {
        StopCoroutine("MovePlayerByPickingCoroutine");
        StartCoroutine("MovePlayerByPickingCoroutine", _pickPos);
    }

    private IEnumerator MovePlayerByPickingCoroutine(Vector3 _pickPos)
    {
        Vector3 moveDir = (_pickPos - transform.position).normalized;
        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - _pickPos) < 0.1f)
            {
                transform.position = _pickPos;
                yield break;
            }
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            yield return null;
        }
    }

    private float moveSpeed = 10f;
}
