using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayUpgradeInfo : Command
{
    public CommandDisplayUpgradeInfo(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.DisplayUpgradeInfo((EUpgradeType)_objects[0]);
    }

    private UIManager uiMng = null;
}
