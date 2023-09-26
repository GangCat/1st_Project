using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWaveEnemyDead : Command
{
    public CommandWaveEnemyDead(EnemyManager _enemyMng)
    {
        enemyMng = _enemyMng;
    }

    public override void Execute(params object[] _objects)
    {
        enemyMng.DeactivateWaveEnemy((GameObject)_objects[0], (int)_objects[1]);
    }

    private EnemyManager enemyMng = null;
}
