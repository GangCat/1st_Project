using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPauseToggle : Command
{
    public CommandPauseToggle(GameManager _gameMng)
    {
        gameMng = _gameMng;
    }

    public override void Execute(params object[] _objects)
    {
        gameMng.TogglePause();
    }

    private GameManager gameMng = null;
}
