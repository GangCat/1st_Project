using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFinishUpgrade : Command
{
    public CommandFinishUpgrade(UIManager _uiMng)
    {
        uiMng = _uiMng;
    }
    public override void Execute(params object[] _objects)
    {
        uiMng.UpgradeFinish();
    }

    private UIManager uiMng = null;
}
