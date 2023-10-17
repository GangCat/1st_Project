using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConfirmChangeBarrackFuncHotkey : Command
{
    public CommandConfirmChangeBarrackFuncHotkey(CanvasBarrackFunc _canvasBarrackFunc)
    {
        canvasBarrackFunc = _canvasBarrackFunc;
    }

    public override void Execute(params object[] _objects)
    {
        canvasBarrackFunc.ChangeHotkey((int)_objects[0], (KeyCode)_objects[1]);
    }

    private CanvasBarrackFunc canvasBarrackFunc = null;
}
