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
        Structure curStructure = SelectableObjectManager.GetFirstSelectedObjectInList.GetComponent<Structure>();

        if (curStructure.IsProcessingUpgrade)
            curStructure.CancleUpgrade();
        else if (curStructure.IsProcessingConstruct)
            curStructure.CancleConstruct();
        else if (curStructure.IsProcessingDemolish)
            curStructure.CancleDemolish();
    }

    private CurrencyManager curMng = null;
}
