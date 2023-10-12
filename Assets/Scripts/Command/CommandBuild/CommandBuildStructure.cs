using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildStructure : Command
{
    public CommandBuildStructure(StructureManager _buildMng, InputManager _inputMng, CurrencyManager _curMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        if (curMng.CanBuildStructure((EObjectType)_objects[0]))
        {
            buildMng.ShowBluepirnt((EObjectType)_objects[0]);
            inputMng.IsBuildOperation = true;
        }
    }

    private StructureManager buildMng = null;
    private InputManager inputMng = null;
    private CurrencyManager curMng = null;
    
}
