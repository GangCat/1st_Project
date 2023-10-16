using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandChangeBuildFuncHotkey : Command
{
    public CommandChangeBuildFuncHotkey(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.ChangeBuildHotkey((EBuildFuncKey)_objects[0]);
    }

    private InputManager inputMng = null;
}
