using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandChangeStructureFuncHotkey : Command
{
    public CommandChangeStructureFuncHotkey(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.ChangeStructureFuncHotkey((EStructureFuncKey)_objects[0]);
    }

    private InputManager inputMng = null;
}
