using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildCancle : Command
{
    public CommandBuildCancle(StructureManager _buildMng, InputManager _inputMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.IsBuildOperation = buildMng.CancleBuild();
    }

    private StructureManager buildMng = null;
    private InputManager inputMng = null;
}
