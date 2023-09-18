using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBunker : Structure
{
    public override void Init(int _bunkerIdx = 0)
    {
        trigger = GetComponentInChildren<BunkerInTrigger>();
        trigger.Init(_bunkerIdx);
        warpPos = transform.position;
        warpPos.y += 5f;

        ListBunkerCommand.Add(EBunkerCommand.IN_UNIT, new CommandInUnit(this), _bunkerIdx);
        ListBunkerCommand.Add(EBunkerCommand.OUT_ONE_UNIT, new CommandOutOneUnit(), _bunkerIdx);
        ListBunkerCommand.Add(EBunkerCommand.OUT_ALL_UNIT, new CommandOutAllUnit(), _bunkerIdx);
    }

    public void InUnit()
    {
        if (curUnitCnt < capacity)
        {
            SelectableObject curObj = trigger.GetCurObj();
            if (curObj.ObjectType.Equals(ESelectableObjectType.UNIT_HERO)) return;

            curObj.Position = warpPos;
            curObj.transform.parent = transform;
            curObj.Hold();
            curObj.SetLayer(1 << LayerMask.GetMask("UnitInBunker"));
            curObj.SetAttackDmg(0.3f);
            curObj.SetAttackRange(0.3f);
            ++curUnitCnt;
        }
    }

    [SerializeField, Range(0,1)]
    private float buffRatio = 0f;

    private BunkerInTrigger trigger = null;
    private Vector3 warpPos = Vector3.zero;

    private int capacity = 1;
    private int curUnitCnt = 0;
}
