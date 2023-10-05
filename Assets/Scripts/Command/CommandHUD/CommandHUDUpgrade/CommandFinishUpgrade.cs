using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFinishUpgrade : Command
{
    public CommandFinishUpgrade(UIManager _uiMng, SelectableObjectManager _selMng)
    {
        uiMng = _uiMng;
        selMng = _selMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.UpgradeFinish();
        selMng.UpdateFuncButton();
    }

    private UIManager uiMng = null;
    private SelectableObjectManager selMng = null;
}
