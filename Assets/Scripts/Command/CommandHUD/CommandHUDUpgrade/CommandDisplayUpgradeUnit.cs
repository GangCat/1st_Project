using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayUpgradeUnit : Command
{
    public CommandDisplayUpgradeUnit(UIManager _uiMng, SelectableObjectManager _selMng)
    {
        uiMng = _uiMng;
        selMng = _selMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.UpgradeUnit((EUnitUpgradeType)_objects[0]);
        selMng.UpdateFuncButton();
    }

    private UIManager uiMng = null;
    private SelectableObjectManager selMng = null;
}
