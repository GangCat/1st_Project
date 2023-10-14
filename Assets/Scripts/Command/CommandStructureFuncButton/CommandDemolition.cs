using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDemolition : Command
{
    public CommandDemolition(CurrencyManager _curMng)
    {
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        SelectableObjectManager.GetFirstSelectedObjectInList().GetComponent<Structure>().Demolish();
    }

    private CurrencyManager curMng = null;
}
