using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRallypoint : Command
{
    public CommandRallypoint(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute()
    {
        inputMng.OnClickRallyPointButton();
    }

    InputManager inputMng = null;
}
