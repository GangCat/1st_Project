using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildConfirm : Command
{
    public CommandBuildConfirm(StructureManager _buildMng, InputManager _inputMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.IsBuildOperation = buildMng.BuildStructure();
    }

    private StructureManager buildMng = null;
    private InputManager inputMng = null;
}
