using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCollectPowerCore : Command
{
    public CommandCollectPowerCore(GameManager _gm)
    {
        gm = _gm;
    }

    public override void Execute(params object[] _objects)
    {
        
    }

    private GameManager gm = null;
}
