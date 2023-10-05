using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonAttack : Command
{
    public CommandButtonAttack(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.OnClickAttackButton();
    }

    private InputManager inputMng = null;
}