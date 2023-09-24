using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureRuin : Structure
{
    public override void Init(PF_Grid _grid)
    {
        grid = _grid;
        StartCoroutine("AutoDestroyCoroutine");
    }

    private IEnumerator AutoDestroyCoroutine()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
