using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTempUnselect : Command
{
    public CommandTempUnselect(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }
    public override void Execute(params object[] _objects)
    {
        selMng.RemoveSelectedObject((SelectableObject)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
}
