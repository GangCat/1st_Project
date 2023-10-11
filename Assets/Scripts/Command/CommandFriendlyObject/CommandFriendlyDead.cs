using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFriendlyDead : Command
{
    public CommandFriendlyDead(StructureManager _structureMng, SelectableObjectManager _selMng, PopulationManager _popMng)
    {
        structureMng = _structureMng;
        selMng = _selMng;
        popMng = _popMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.DeactivateUnit((GameObject)_objects[0], (EUnitType)_objects[1], (FriendlyObject)_objects[2]);
        //structureMng.DeactivateUnit((GameObject)_objects[0], (EUnitType)_objects[1], (int)_objects[2]);
        //selMng.RemoveUnitAtList((FriendlyObject)_objects[3]);
        popMng.UnitDead((EUnitType)_objects[1]);
    }

    private StructureManager structureMng = null;
    private SelectableObjectManager selMng = null;
    private PopulationManager popMng = null;
}
