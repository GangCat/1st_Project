using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildConfirm : Command
{
    public CommandBuildConfirm(BuildManager _buildMng, InputManager _inputMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute()
    {
        inputMng.IsBuildOperation = buildMng.BuildStructure();
    }

    private BuildManager buildMng = null;
    private InputManager inputMng = null;
}
