using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonPatrol : Command
{
    public CommandButtonPatrol(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.OnClickPatrolButton();
    }

    private InputManager inputMng = null;
}
