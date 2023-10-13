using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRegistPauseObserver : Command
{
    public CommandRegistPauseObserver(GameManager _gameMng)
    {
        gameMng = _gameMng;
    }

    public override void Execute(params object[] _objects)
    {
        gameMng.RegisterPauseObserver((IPauseObserver)_objects[0]);
    }

    private GameManager gameMng = null;
}
