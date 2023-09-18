using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerInTrigger : MonoBehaviour
{
    public void Init(int _bunkerIdx)
    {
        bunkerIdx = _bunkerIdx;
    }

    public SelectableObject GetCurObj()
    {
        return unitObj;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("FriendlyUnit"))
        {
            unitObj = _other.GetComponent<SelectableObject>();
            if (unitObj.IsBunker)
                ListBunkerCommand.Use(EBunkerCommand.IN_UNIT, bunkerIdx);
        }
    }

    private int bunkerIdx = 0;
    private SelectableObject unitObj = null;
}
