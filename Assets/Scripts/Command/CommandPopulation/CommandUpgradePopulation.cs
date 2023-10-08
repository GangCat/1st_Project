using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradePopulation : Command
{
    public CommandUpgradePopulation(PopulationManager _popMng, CurrencyManager _curMng)
    {
        popMng = _popMng;
        curMng = _curMng;
    }

    public override void Execute(params object[] _objects)
    {
        StructureMainBase main = SelectableObjectManager.GetFirstSelectedObjectInList.GetComponent<StructureMainBase>();

        if (curMng.CanUpgradeETC(EUpgradeETCType.CURRENT_MAX_POPULATION) && popMng.CanUpgradePopulation() && !main.IsProcessingUpgrade)
        {
            curMng.UpgradeETC(EUpgradeETCType.CURRENT_MAX_POPULATION);
            main.UpgradeMaxPopulation();
        }
    }

    private PopulationManager popMng = null;
    private CurrencyManager curMng = null;
}
