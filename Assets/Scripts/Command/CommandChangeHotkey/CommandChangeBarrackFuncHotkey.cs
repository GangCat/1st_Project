using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandChangeBarrackFuncHotkey : Command
{
    public CommandChangeBarrackFuncHotkey(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.ChangeBarrackHotkey((EBarrackFuncKey)_objects[0]);
    }

    private InputManager inputMng = null;
}
