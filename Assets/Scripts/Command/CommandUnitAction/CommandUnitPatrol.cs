using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUnitPatrol : Command
{
    public CommandUnitPatrol(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }
    public override void Execute(params object[] _objects)
    {
        selMng.Patrol((Vector3)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
}
