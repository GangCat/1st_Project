using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRemovePauseObserver : Command
{
    public CommandRemovePauseObserver(GameManager _gameMng)
    {
        gameMng = _gameMng;
    }

    public override void Execute(params object[] _objects)
    {
        gameMng.RemovePauseObserver((IPauseObserver)_objects[0]);
    }

    private GameManager gameMng = null;
}
