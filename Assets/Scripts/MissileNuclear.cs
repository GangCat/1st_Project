using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileNuclear : MonoBehaviour
{
    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void SetPos(Vector3 _pos)
    {
        transform.localPosition = _pos;
    }

    public void ResetRotate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void Launch(Vector3 _destPos)
    {
        StartCoroutine("LaunchCoroutine", _destPos);
        //Debug.Log(_destPos + "Launch");
        //SetActive(false);
    }

    private IEnumerator LaunchCoroutine(Vector3 _destPos)
    {
        while (transform.position.y < 30f)
        {
            transform.position += Vector3.up * launchSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(_destPos.x, 30f, _destPos.z);
        transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, 0f));
        while(transform.position.y > 0)
        {
            transform.position -= Vector3.up * launchSpeed * Time.deltaTime;
            yield return null;
        }

        Collider[] arrCol = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("SelectableObject"));
        for(int i = 0; i < arrCol.Length; ++i)
            arrCol[i].gameObject.GetComponent<SelectableObject>().GetDmg(150);

        SetActive(false);
    }

    [SerializeField]
    private float attackRange = 0f;
    [SerializeField]
    private float launchSpeed = 0f;
}
