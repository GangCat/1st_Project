using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeUnitHp : Command
{
    public CommandUpgradeUnitHp(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.GetFirstSelectedObjectInList.GetComponent<StructureBarrack>().UpgradeUnitHp();
    }

    private SelectableObjectManager selMng = null;
}
