using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConfirmChangeUnitFuncHotkey : Command
{
    public CommandConfirmChangeUnitFuncHotkey(CanvasUnitBaseFunc _canvasUnitFunc)
    {
        canvasUnitFunc = _canvasUnitFunc;
    }
    public override void Execute(params object[] _objects)
    {
        canvasUnitFunc.ChangeHotkey((EUnitFuncKey)_objects[0], (KeyCode)_objects[1]);
    }

    private CanvasUnitBaseFunc canvasUnitFunc = null;
}
