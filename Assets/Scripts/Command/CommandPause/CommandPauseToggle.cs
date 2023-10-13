using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPauseToggle : Command
{
    public CommandPauseToggle(GameManager _gameMng, InputManager _inputMng, StructureManager _structureMng)
    {
        gameMng = _gameMng;
        inputMng = _inputMng;
        structureMng = _structureMng;
    }

    public override void Execute(params object[] _objects)
    {
        gameMng.TogglePause();
        if(inputMng.IsBuildOperation)
            inputMng.IsBuildOperation = structureMng.CancleBuild();
    }

    private GameManager gameMng = null;
    private InputManager inputMng = null;
    private StructureManager structureMng = null;
}
