using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTurret : Structure
{
    public override void Init()
    {
        selectObj = GetComponentInChildren<FriendlyObject>();
        selectObj.Init();
        selectObj.SetMyTr(turretHeadTr);
    }

    [SerializeField]
    private Transform turretHeadTr = null;

    private FriendlyObject selectObj = null;
}
