using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public override void UpdateCurNode()
    {
        SelectableObjectManager.UpdateEnemyNodeWalkable(transform.position, nodeIdx);
    }

    public override void GetDmg(float _dmg)
    {
        if (statusHp.DecreaseHpAndCheckIsDead(_dmg))
        {
            StopAllCoroutines();
            SelectableObjectManager.ResetEnemyNodeWalkable(transform.position, nodeIdx);
            ArrayEnemyObjectCommand.Use((EEnemyObjectCommand)spawnType, gameObject, myIdx);

            if (Random.Range(0.0f, 100.0f) < 30f)
                Instantiate(powerCorePrefab, transform.position, Quaternion.identity);

            ArrayPauseCommand.Use(EPauseCommand.REMOVE, this);
        }
        //else if (isSelect)
        //{
        //    SelectableObjectManager.UpdateHp(listIdx);
        //}
    }

    protected override void RequestPath(Vector3 _startPos, Vector3 _endPos)
    {
        PF_PathRequestManager.EnemyRequestPath(_startPos, _endPos, OnPathFound);
    }

    [SerializeField]
    private GameObject powerCorePrefab = null;

    private EEnemySpawnType spawnType = EEnemySpawnType.NONE;
    private int myIdx = 0;
}
