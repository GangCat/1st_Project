using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMapEnemyDead : Command
{
    public CommandMapEnemyDead(EnemyManager _enemyMng)
    {
        enemyMng = _enemyMng;
    }
    public override void Execute(params object[] _objects)
    {
        enemyMng.DeactivateMapEnemy((GameObject)_objects[0], (int)_objects[1]);
    }

    private EnemyManager enemyMng = null;
}
