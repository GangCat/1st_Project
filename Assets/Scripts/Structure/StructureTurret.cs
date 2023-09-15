using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTurret : Structure
{
    public override void Init(PF_Grid _grid)
    {
        base.Init(_grid);
        selectObj = GetComponentInChildren<SelectableObject>();
        selectObj.Init();
        selectObj.SetMyTr(turretHeadTr);
    }

    [SerializeField]
    private Transform turretHeadTr = null;

    private SelectableObject selectObj = null;
}
