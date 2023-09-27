using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeRangedUnitHp : Command
{
    public CommandUpgradeRangedUnitHp(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.GetFirstSelectedObjectInList.GetComponent<StructureBarrack>().UpgradeRangedUnitHp();
    }

    private SelectableObjectManager selMng = null;
}
