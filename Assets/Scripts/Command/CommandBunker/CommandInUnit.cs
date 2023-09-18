using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInUnit : Command
{
    public CommandInUnit(StructureBunker _bunker)
    {
        bunker = _bunker;
    }

    public override void Execute()
    {
        bunker.InUnit();
    }

    StructureBunker bunker = null;
}
