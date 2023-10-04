using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuildComplete : Command
{
    public CommandBuildComplete(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.UpdateFuncButton();
    }

    private SelectableObjectManager selMng = null;
}
