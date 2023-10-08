using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgrade : Command
{
    public CommandUpgrade(StructureManager _structureMng, CurrencyManager _curMng)
    {
        structureMng = _structureMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        
        Structure tempStructure = SelectableObjectManager.GetFirstSelectedObjectInList.GetComponent<Structure>();
        EObjectType structureObjType = tempStructure.GetComponent<FriendlyObject>().GetObjectType();
        if(structureObjType.Equals(EObjectType.BARRACK))
        {
            StructureBarrack barrack = tempStructure.GetComponent<StructureBarrack>();
            if (barrack.IsProcessingSpawnUnit)
                return;
        }
        else if (curMng.CanUpgradeSturcture(structureObjType, tempStructure.UpgradeLevel))
        {
            if(structureMng.UpgradeStructure(tempStructure.StructureIdx))
                curMng.UpgradeStructure(structureObjType, tempStructure.UpgradeLevel);
        }
    }

    private StructureManager structureMng = null;
    private CurrencyManager curMng = null;
}
