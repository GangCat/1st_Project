using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradePopulation : Command
{
    public CommandUpgradePopulation(PopulationManager _popMng, CurrencyManager _curMng, SelectableObjectManager _selMng)
    {
        popMng = _popMng;
        curMng = _curMng;
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        if (curMng.CanUpgradeETC(EUpgradeETCType.CURRENT_MAX_POPULATION) && popMng.CanUpgradePopulation())
        {
            curMng.UpgradeETC(EUpgradeETCType.CURRENT_MAX_POPULATION);
            selMng.GetFirstSelectedObjectInList.GetComponent<StructureMainBase>().UpgradeMaxPopulation();
        }
    }

    private PopulationManager popMng = null;
    private CurrencyManager curMng = null;
    private SelectableObjectManager selMng = null;
}
