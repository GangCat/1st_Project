using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildStructure : Command
{
    public CommandBuildStructure(StructureManager _buildMng, InputManager _inputMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        buildMng.ShowBluepirnt((EObjectType)_objects[0]);
        inputMng.IsBuildOperation = true;
    }

    private StructureManager buildMng = null;
    private InputManager inputMng = null;
}
