using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandChangeSelectMainbaseHotkey : Command
{
    public CommandChangeSelectMainbaseHotkey(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.ChangeUnitHotkey((EUnitFuncHotkey)_objects[0]);
    }

    private InputManager inputMng = null;
}
