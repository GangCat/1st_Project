using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandIncreaseMaxPopulation : Command
{
    public CommandIncreaseMaxPopulation(PopulationManager _popMng)
    {
        popMng = _popMng;
    }

    public override void Execute(params object[] _objects)
    {
        popMng.UpgradeMaxPopulation();
    }

    private PopulationManager popMng = null;
}
