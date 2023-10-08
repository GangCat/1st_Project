using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandStructureCancle : Command
{
    public CommandStructureCancle(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        SelectableObjectManager.GetFirstSelectedObjectInList.GetComponent<Structure>().CancleCurAction();
    }

    private CurrencyManager curMng = null;
}
