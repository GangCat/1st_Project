using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonLaunchNuclear : Command
{
    public CommandButtonLaunchNuclear(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }
    public override void Execute(params object[] _objects)
    {
        inputMng.OnClickLaunchNuclearButton();
    }

    private InputManager inputMng = null;
}
