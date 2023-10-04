using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoreCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("PowerCore"))
        {
            ArrayCurrencyCommand.Use(ECurrencyCommand.COLLECT_CORE, (uint)20);
            Destroy(_other.gameObject);
        }
    }
}
