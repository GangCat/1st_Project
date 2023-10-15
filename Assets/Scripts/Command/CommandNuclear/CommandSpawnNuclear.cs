using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSpawnNuclear : Command
{
    public CommandSpawnNuclear(StructureManager _structureMng, CurrencyManager _curMng)
    {
        structureMng = _structureMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        if (curMng.CanSpawnNuclear())
        {
            curMng.SpawnNuclear();
            structureMng.SpawnNuclear(SelectableObjectManager.GetFirstSelectedObjectInList().GetComponent<Structure>().StructureIdx);
        }
    }

    private StructureManager structureMng = null;
    private CurrencyManager curMng = null;
}
