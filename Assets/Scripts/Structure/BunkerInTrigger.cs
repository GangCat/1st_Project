using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerInTrigger : MonoBehaviour
{
    public SelectableObject GetCurObj()
    {
        return unitObj;
    }

    public void ResetObj()
    {
        unitObj = null;
    }

    public void Init(int _capa)
    {
        capacity = _capa;
    }

    public void UpdateCapacity(int _capa)
    {
        capacity = _capa;
    }

    public void UpdateUnitCnt(int _cnt)
    {
        curUnitCnt = _cnt;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("FriendlyUnit"))
        {
            unitObj = _other.GetComponent<SelectableObject>();
            if (unitObj.IsBunker && curUnitCnt < capacity)
                ArrayBunkerCommand.Use(EBunkerCommand.IN_UNIT);
            else
            {
                unitObj.IsBunker = false;
                unitObj = null;
            }
        }
    }

    private SelectableObject unitObj = null;
    private int capacity = 0;
    private int curUnitCnt = 0;
}
