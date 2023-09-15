using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildTurret : Command
{
    public CommandBuildTurret(StructureManager _buildMng, InputManager _inputMng) 
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute()
    {
        buildMng.ShowBluepirnt(ESelectableObjectType.TURRET);
        inputMng.IsBuildOperation = true;
    }

    private StructureManager buildMng = null;
    private InputManager inputMng = null;
}
