using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMove : Command
{
    public CommandMove(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }
    public override void Execute()
    {
        inputMng.OnClickMoveButton();
    }

    private InputManager inputMng = null;
}
