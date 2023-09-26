using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFriendlyDestroyHBeam : Command
{
    public CommandFriendlyDestroyHBeam(StructureManager _structureMng)
    {
        structureMng = _structureMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.DestroyStructure(((GameObject)_objects[0]).GetComponent<Structure>().StructureIdx);
    }

    private StructureManager structureMng = null;
}
