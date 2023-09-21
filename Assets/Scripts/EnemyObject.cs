using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : SelectableObject
{
    [System.Serializable]
    public enum EEnemySpawnType { NONE = -1, WAVE_SPAWN, MAP_SPAWN, LENGTH }

    public void Init(EEnemySpawnType _spawnType, int _myIdx)
    {
        spawnType = _spawnType;
        myIdx = _myIdx;
        gameObject.layer = LayerMask.NameToLayer("SelectableObject");
    }
    public override void GetDmg(float _dmg)
    {
        if (statusHp.DecreaseHpAndCheckIsDead(_dmg))
            ArrayEnemyObjectCommand.Use((EEnemyObjectCommand)spawnType, gameObject, myIdx);
    }

    private EEnemySpawnType spawnType = EEnemySpawnType.NONE;
    private int myIdx = 0;
}
