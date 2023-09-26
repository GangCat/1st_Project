using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureRuin : Structure
{
    public override void Init()
    {
        StartCoroutine("AutoDestroyCoroutine");
    }

    private IEnumerator AutoDestroyCoroutine()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
