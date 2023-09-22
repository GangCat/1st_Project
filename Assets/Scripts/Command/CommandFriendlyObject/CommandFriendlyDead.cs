using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFriendlyDead : Command
{
    public CommandFriendlyDead(StructureManager _structureMng, SelectableObjectManager _selMng)
    {
        structureMng = _structureMng;
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.DeactivateUnit((GameObject)_objects[0], (ESpawnUnitType)_objects[1], (int)_objects[2]);
        selMng.RemoveUnitAtList(((GameObject)_objects[0]).GetComponent<FriendlyObject>());
    }

    private StructureManager structureMng = null;
    private SelectableObjectManager selMng = null;
}
