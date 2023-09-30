using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildConfirm : Command
{
    public CommandBuildConfirm(StructureManager _buildMng, InputManager _inputMng, CurrencyManager _curMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        if (buildMng.BuildStructure())
        {
            inputMng.IsBuildOperation = false;
            curMng.BuildStructure(buildMng.CurStructureType());
        }
    }

    private StructureManager buildMng = null;
    private InputManager inputMng = null;
    private CurrencyManager curMng = null;
}
