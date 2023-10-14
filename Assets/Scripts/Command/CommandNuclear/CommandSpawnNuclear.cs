using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnNuclear : Command
{
    public CommandSpawnNuclear(StructureManager _structureMng)
    {
        structureMng = _structureMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.SpawnNuclear(SelectableObjectManager.GetFirstSelectedObjectInList().GetComponent<Structure>().StructureIdx);
    }

    private StructureManager structureMng = null;
}
