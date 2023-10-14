using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandExpandWall : Command
{
    public CommandExpandWall(StructureManager _buildMng, InputManager _inputMng)
    {
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _value)
    {
        FriendlyObject bunkerObj = SelectableObjectManager.GetFirstSelectedObjectInList();
        buildMng.ShowBluepirnt(bunkerObj.transform);
        inputMng.IsBuildOperation = true;
    }

    private StructureManager buildMng = null;
    private InputManager inputMng = null;
}
