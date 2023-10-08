using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDemolishComplete : Command
{
    public CommandDemolishComplete(StructureManager _structureMng)
    {
        structureMng = _structureMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.DestroyStructure((int)_objects[0]);
    }

    private StructureManager structureMng = null;
}
