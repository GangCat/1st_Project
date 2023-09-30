using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgrade : Command
{
    public CommandUpgrade(StructureManager _structureMng, SelectableObjectManager _selMng, CurrencyManager _curMng)
    {
        structureMng = _structureMng;
        selMng = _selMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        Structure tempStructure = selMng.GetFirstSelectedObjectInList.GetComponent<Structure>();
        EObjectType structureObjType = tempStructure.GetComponent<FriendlyObject>().GetObjectType();
        if (curMng.CanUpgradeSturcture(structureObjType, tempStructure.UpgradeLevel))
        {
            if(structureMng.UpgradeStructure(tempStructure.StructureIdx))
                curMng.UpgradeStructure(structureObjType, tempStructure.UpgradeLevel);
        }
    }

    private SelectableObjectManager selMng = null;
    private StructureManager structureMng = null;
    private CurrencyManager curMng = null;
}
