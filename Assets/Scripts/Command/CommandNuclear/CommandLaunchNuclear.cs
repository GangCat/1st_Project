using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLaunchNuclear : Command
{
    public CommandLaunchNuclear(StructureManager _structureMng)
    {
        structureMng = _structureMng;
    }

    public override void Execute(params object[] _objects)
    {
        structureMng.LaunchNuclear((Vector3)_objects[0]);
    }

    private StructureManager structureMng = null;
}
