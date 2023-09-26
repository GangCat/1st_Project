using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpgradeBuffRatio : Command
{ 
    public CommandUpgradeBuffRatio(StructureBunker _bunker)
    {
        bunker = _bunker;
    }

    public override void Execute(params object[] _objects)
    {
        bunker.UpgradeBuffRatio();
    }

    private StructureBunker bunker = null;
}
