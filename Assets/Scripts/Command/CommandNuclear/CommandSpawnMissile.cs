using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnMissile : Command
{
    public CommandSpawnMissile(StructureManager _structureMng, SelectableObjectManager _selMng)
    {
        structureMng = _structureMng;
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.SpawnMissile(selMng.GetFirstSelectedObjectInList.GetComponent<Structure>().StructureIdx);
    }

    private StructureManager structureMng = null;
    private SelectableObjectManager selMng = null;
}
