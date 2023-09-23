using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTurret : Structure
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        selectObj = GetComponentInChildren<FriendlyObject>();
        selectObj.Init();
        selectObj.SetMyTr(turretHeadTr);
        myIdx = _structureIdx;
    }

    [SerializeField]
    private Transform turretHeadTr = null;

    private FriendlyObject selectObj = null;
}
