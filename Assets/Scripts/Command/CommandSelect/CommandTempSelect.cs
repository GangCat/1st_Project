using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTempSelect : Command
{
    public CommandTempSelect(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }
    public override void Execute(params object[] _objects)
    {
        selMng.AddSelectedObject((SelectableObject)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
}
