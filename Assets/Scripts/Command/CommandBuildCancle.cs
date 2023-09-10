using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildCancle : Command
{
    public CommandBuildCancle(BuildManager _buildMng, InputManager _inputMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute()
    {
        inputMng.IsBuildOperation = buildMng.CancleBuild();
    }

    private BuildManager buildMng = null;
    private InputManager inputMng = null;
}
