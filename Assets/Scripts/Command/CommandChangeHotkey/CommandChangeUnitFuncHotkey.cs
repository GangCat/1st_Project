using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandChangeUnitFuncHotkey : Command
{
    public CommandChangeUnitFuncHotkey(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.ChangeUnitHotkey((EUnitFuncKey)_objects[0]);
    }

    private InputManager inputMng = null;
}
