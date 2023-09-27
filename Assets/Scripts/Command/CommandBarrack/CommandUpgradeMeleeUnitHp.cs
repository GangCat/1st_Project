using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeMeleeUnitHp : Command
{
    public CommandUpgradeMeleeUnitHp(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        if(selMng.GetFirstSelectedObjectInList.GetComponent<StructureBarrack>().UpgradeMeleeUnitHp())
            selMng.UpdateFuncButton();
    }

    private SelectableObjectManager selMng = null;
}
