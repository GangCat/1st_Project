using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUnitCancle : Command
{
    public CommandUnitCancle(InputManager _inputMng) 
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.CancleFunc();
    }

    private InputManager inputMng = null;
}
