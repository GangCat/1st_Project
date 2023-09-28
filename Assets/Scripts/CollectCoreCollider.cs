using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoreCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("PowerCore"))
        {
            Destroy(_other.gameObject);
            Debug.Log("Core");
        }
    }
}
