using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRallypoint : Command
{
    public CommandRallypoint(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.OnClickRallyPointButton();
    }

    InputManager inputMng = null;
}
