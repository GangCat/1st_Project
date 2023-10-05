using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayUpgradeMainbase : Command
{
    public CommandDisplayUpgradeMainbase(UIManager _uiMng, SelectableObjectManager _selMng)
    {
        uiMng = _uiMng;
        selMng = _selMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.UpgradeMainbase((EUpgradeETCType)_objects[0]);
        selMng.UpdateFuncButton();
    }

    private UIManager uiMng = null;
    private SelectableObjectManager selMng = null;
}
