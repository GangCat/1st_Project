using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildCancle : Command
{
    public CommandBuildCancle(StructureManager _structureMng, InputManager _inputMng)
    {
        structureMng = _structureMng;
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.IsBuildOperation = structureMng.CancleBuild();
    }

    private StructureManager structureMng = null;
    private InputManager inputMng = null;
}
