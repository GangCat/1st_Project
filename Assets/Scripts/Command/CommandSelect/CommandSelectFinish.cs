using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSelectFinish : Command
{
    public CommandSelectFinish(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }
    public override void Execute(params object[] _objects)
    {
        selMng.SelectFinish();
    }

    private SelectableObjectManager selMng = null;
}
