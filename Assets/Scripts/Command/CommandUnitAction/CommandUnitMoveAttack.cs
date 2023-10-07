using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUnitMoveAttack : Command
{
    public CommandUnitMoveAttack(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.MoveUnitByPicking((Vector3)_objects[0], true);
    }

    private SelectableObjectManager selMng = null;
}
