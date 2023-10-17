using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildStructure : Command
{
    public CommandBuildStructure(StructureManager _structureMng, InputManager _inputMng, CurrencyManager _curMng)
    {
        structureMng = _structureMng;
        inputMng = _inputMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        EObjectType tempObjType = (EObjectType)_objects[0];
        if (curMng.CanBuildStructure(tempObjType))
        {
            if (tempObjType.Equals(EObjectType.NUCLEAR))
                if (!structureMng.CanBuildNuclear())
                    return;

            structureMng.ShowBluepirnt(tempObjType);
            inputMng.IsBuildOperation = true;
        }
    }

    private StructureManager structureMng = null;
    private InputManager inputMng = null;
    private CurrencyManager curMng = null;
    
    
}
