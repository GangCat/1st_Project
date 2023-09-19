using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public void Init()
    {
        memoryPool = new MemoryPool(enemyPrefab, 5, transform);
    }

    public void SpawnEnemy()
    {
        GameObject enemyGo = memoryPool.ActivatePoolItem(5, transform);

        EnemyObject enemyObj = enemyGo.GetComponent<EnemyObject>();
        enemyObj.Position = spawnPos;
        enemyObj.Init();
        spawnPos.x += 1f;
    }

    public void SpawnEnemy(Vector3 _targetPos, int _count)
    {
        StartCoroutine(SpawnWaveCoroutine(_targetPos, _count));
    }

    private IEnumerator SpawnWaveCoroutine(Vector3 _targetPos, int _count)
    {
        int unitCnt = 0;
        while (unitCnt < _count)
        {
            GameObject enemyGo = memoryPool.ActivatePoolItem(5, transform);
            EnemyObject enemyObj = enemyGo.GetComponent<EnemyObject>();
            enemyObj.Position = spawnPos;
            enemyObj.Init();
            enemyObj.MoveAttack(_targetPos);
            ++unitCnt;

            yield return null;
        }
    }

    protected void OnPathFound(PF_Node[] _newPath, bool _pathSuccessful)
    {
        if (_pathSuccessful)
        {
            arrPath = _newPath;
            targetIdx = 0;
            UpdateTargetPos();
        }
    }

    protected void UpdateTargetPos()
    {
        curWayNode = arrPath[targetIdx];
    }

    private MemoryPool memoryPool = null;

    [SerializeField]
    private GameObject enemyPrefab = null;
    [SerializeField]
    private Vector3 spawnPos = Vector3.zero;

    protected int targetIdx = 0;
    protected PF_Node[] arrPath = null;
    protected PF_Node curWayNode = null;
}
