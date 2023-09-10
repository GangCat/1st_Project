using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildNuclear : Command
{
    public CommandBuildNuclear(BuildManager _buildMng, InputManager _inputMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute()
    {
        buildMng.ShowBluepirnt(ESelectableObjectType.NUCLEAR);
        inputMng.IsBuildOperation = true;
    }

    private BuildManager buildMng = null;
    private InputManager inputMng = null;
}
