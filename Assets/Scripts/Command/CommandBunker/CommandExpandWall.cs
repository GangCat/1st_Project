using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandExpandWall : Command
{
    public CommandExpandWall(SelectableObjectManager _selMng, StructureManager _buildMng, InputManager _inputMng)
    {
        selMng = _selMng;
        buildMng = _buildMng;
        inputMng = _inputMng;
    }

    public override void Execute()
    {
        SelectableObject bunkerObj = selMng.GetFirstSelectableObjectInList;
        buildMng.ShowBluepirnt(bunkerObj.transform);
        inputMng.IsBuildOperation = true;
    }

    private SelectableObjectManager selMng = null;
    private StructureManager buildMng = null;
    private InputManager inputMng = null;
}
