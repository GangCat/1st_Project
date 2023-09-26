using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnNuclear : Command
{
    public CommandSpawnNuclear(StructureManager _structureMng, SelectableObjectManager _selMng)
    {
        structureMng = _structureMng;
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.SpawnNuclear(selMng.GetFirstSelectedObjectInList.GetComponent<Structure>().StructureIdx);
    }

    private StructureManager structureMng = null;
    private SelectableObjectManager selMng = null;
}
