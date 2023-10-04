using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandIncreaseCurPopulation : Command
{
    public CommandIncreaseCurPopulation(PopulationManager _popMng)
    {
        popMng = _popMng;
    }

    public override void Execute(params object[] _objects)
    {
        popMng.SpawnUnit((EUnitType)_objects[0]);
    }

    private PopulationManager popMng = null;
}
