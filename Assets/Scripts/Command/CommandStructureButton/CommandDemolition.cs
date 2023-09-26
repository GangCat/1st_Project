using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDemolition : Command
{
    public CommandDemolition(StructureManager _structureMng, SelectableObjectManager _selMng)
    {
        structureMng = _structureMng;
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.Demolish(selMng.GetFirstSelectedObjectInList.GetComponent<Structure>().StructureIdx);
    }

    private StructureManager structureMng = null;
    private SelectableObjectManager selMng = null;
}
