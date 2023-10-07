using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUnitFollowObject : Command
{
    public CommandUnitFollowObject(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.MoveUnitByPicking((Transform)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
}
