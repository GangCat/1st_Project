using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("FriendlyUnit"))
        {

        }
    }
}
