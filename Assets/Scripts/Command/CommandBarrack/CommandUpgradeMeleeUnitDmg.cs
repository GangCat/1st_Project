using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeMeleeUnitDmg : Command
{
    public CommandUpgradeMeleeUnitDmg(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.GetFirstSelectedObjectInList.GetComponent<StructureBarrack>().UpgradeMeleeUnitDmg();
    }

    private SelectableObjectManager selMng = null;
}
