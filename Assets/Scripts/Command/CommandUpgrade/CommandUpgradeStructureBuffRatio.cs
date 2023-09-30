using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeStructureBuffRatio : Command
{ 
    public CommandUpgradeStructureBuffRatio(StructureBunker _bunker)
    {
        bunker = _bunker;
    }

    public override void Execute(params object[] _objects)
    {
        bunker.UpgradeBuffRatio();
    }

    private StructureBunker bunker = null;
}
