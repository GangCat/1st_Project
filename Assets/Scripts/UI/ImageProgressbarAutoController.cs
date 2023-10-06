using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProgressbarAutoController : ImageProgressbar
{
    public void Init(UnitInfoContainer _container)
    {
        container = _container;
    }

    public void UpdateHp()
    {
        StartCoroutine("UpdateHpCoroutine");
    }

    public void StopUpdateHp()
    {
        StopAllCoroutines();
    }

    private IEnumerator UpdateHpCoroutine()
    {
        while (true)
        {
            UpdateLength(container.curHpPercent);
            yield return new WaitForSeconds(updateRate);
        }
    }

    [SerializeField]
    private float updateRate = 0.1f;

    private UnitInfoContainer container = null;
}
