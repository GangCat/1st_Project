using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDemolition : Command
{
    public CommandDemolition(StructureManager _structureMng)
    {
        structureMng = _structureMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.Demolish(SelectableObjectManager.GetFirstSelectedObjectInList.GetComponent<Structure>().StructureIdx);
    }

    private StructureManager structureMng = null;
}
