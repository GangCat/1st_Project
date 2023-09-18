using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTurret : Structure
{
    public override void Init()
    {
        selectObj = GetComponentInChildren<SelectableObject>();
        selectObj.Init();
        selectObj.SetMyTr(turretHeadTr);
    }

    [SerializeField]
    private Transform turretHeadTr = null;

    private SelectableObject selectObj = null;
}
