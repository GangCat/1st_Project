using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerInTrigger : MonoBehaviour
{
    public void Init(Transform _bunkerTr)
    {
        bunkerTr = _bunkerTr;
    }

    public FriendlyObject GetCurObj()
    {
        return unitObj;
    }

    public void ResetObj()
    {
        unitObj = null;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("FriendlyUnit"))
        {
            unitObj = _other.GetComponent<FriendlyObject>();

            if (unitObj.TargetBunker != null && unitObj.TargetBunker.Equals(bunkerTr))
                ArrayBunkerCommand.Use(EBunkerCommand.IN_UNIT, unitObj);
            else
                unitObj = null;
        }
    }

    private FriendlyObject unitObj = null;
    private Transform bunkerTr = null;
}
