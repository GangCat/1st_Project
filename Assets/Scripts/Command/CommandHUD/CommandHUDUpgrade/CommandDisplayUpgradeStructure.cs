using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayUpgradeStructure : Command
{
    public CommandDisplayUpgradeStructure(UIManager _uiMng, SelectableObjectManager _selMng)
    {
        selMng = _selMng;
        uiMng = _uiMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.UpgradeStructure();
        selMng.UpdateFuncButton();
    }

    private UIManager uiMng = null;
    private SelectableObjectManager selMng = null;
}
