using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateInfoUI : Command
{
    public CommandUpdateInfoUI(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.UpdateInfo();
    }

    private SelectableObjectManager selMng = null;
}
