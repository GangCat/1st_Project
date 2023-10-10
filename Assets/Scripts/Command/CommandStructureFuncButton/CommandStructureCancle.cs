using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandStructureCancle : Command
{
    public CommandStructureCancle(CurrencyManager _curMng, InputManager _inputMng)
    {
        curMng = _curMng;
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _objects)
    {
        inputMng.CancleRallypoint();
        SelectableObjectManager.GetFirstSelectedObjectInList.GetComponent<Structure>().CancleCurAction();
    }

    private CurrencyManager curMng = null;
    private InputManager inputMng = null;
}
