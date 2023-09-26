using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgrade : Command
{
    public CommandUpgrade(StructureManager _structureMng, SelectableObjectManager _selMng)
    {
        structureMng = _structureMng;
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.UpgradeStructure(selMng.GetFirstSelectedObjectInList.GetComponent<Structure>().StructureIdx);
    }

    private SelectableObjectManager selMng = null;
    private StructureManager structureMng = null;
}
