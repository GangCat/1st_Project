using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCompleteSpawnUnit : Command
{
    public CommandCompleteSpawnUnit(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.SpawnUnit((EUnitType)_objects[0], (Vector3)_objects[1], (Vector3)_objects[2], (Transform)_objects[3]);
    }

    private SelectableObjectManager selMng = null;
}
