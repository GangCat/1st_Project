using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public void Init()
    {
        waveEnemyHolder = GetComponentInChildren<WaveEnemyHolder>().GetTransform();
        mapEnemyHolder = GetComponentInChildren<MapEnemyHolder>().GetTransform();

        memoryPoolWave = new MemoryPool(enemyPrefab, 5, waveEnemyHolder);
        memoryPoolMap = new MemoryPool(enemyPrefab, 5, mapEnemyHolder);
    }

    public void SpawnWaveEnemy(Vector3 _targetPos, int _count)
    {
        StartCoroutine(SpawnWaveEnemyCoroutine(_targetPos, _count));
    }

    public void SpawnMapEnemy(Vector3 _spawnPos, int _count)
    {
        Vector3 spawnPos = _spawnPos + Functions.GetRandomPosition(20f, 0f);
        StartCoroutine(SpawnMapEnemyCoroutine(spawnPos, _count));
    }

    public void DeactivateWaveEnemy(GameObject _removeGo, int _waveEnemyIdx)
    {
        /*GameObject enemyGo = */memoryPoolWave.DeactivatePoolItemWithIdx(_removeGo, _waveEnemyIdx);
        //if (enemyGo == null) return;
        // 레이어 변경
        //enemyGo.layer = LayerMask.NameToLayer("EnemyDead");
    }

    public void DeactivateMapEnemy(GameObject _removeGo, int _mapEnemyIdx)
    {
        memoryPoolMap.DeactivatePoolItemWithIdx(_removeGo, _mapEnemyIdx);
    }

    private IEnumerator SpawnWaveEnemyCoroutine(Vector3 _targetPos, int _count)
    {
        int unitCnt = 0;
        while (unitCnt < _count)
        {
            GameObject enemyGo = memoryPoolWave.ActivatePoolItemWithIdx(waveEnemyIdx, 5, waveEnemyHolder);
            EnemyObject enemyObj = enemyGo.GetComponent<EnemyObject>();
            enemyObj.Position = spawnPos;
            enemyObj.Init();
            enemyObj.Init(EnemyObject.EEnemySpawnType.WAVE_SPAWN, waveEnemyIdx);
            enemyObj.MoveAttack(_targetPos);
            ++waveEnemyIdx;
            ++unitCnt;
            
            if (spawnPos.x >= 55f)
                spawnPos.x = 45f;
            else
                spawnPos.x += 1f;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator SpawnMapEnemyCoroutine(Vector3 _spawnPos, int _count)
    {
        int unitCnt = 0;
        while (unitCnt < _count)
        {
            GameObject enemyGo = memoryPoolMap.ActivatePoolItemWithIdx(mapEnemyIdx, 5, mapEnemyHolder);
            EnemyObject enemyObj = enemyGo.GetComponent<EnemyObject>();
            enemyObj.Position = _spawnPos;
            enemyObj.Init();
            enemyObj.Init(EnemyObject.EEnemySpawnType.MAP_SPAWN, mapEnemyIdx);
            ++mapEnemyIdx;
            ++unitCnt;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private MemoryPool memoryPoolWave = null;
    private MemoryPool memoryPoolMap = null;

    [SerializeField]
    private GameObject enemyPrefab = null;
    [SerializeField]
    private Vector3 spawnPos = Vector3.zero;

    private Transform waveEnemyHolder = null;
    private Transform mapEnemyHolder = null;

    private int waveEnemyIdx = 0;
    private int mapEnemyIdx = 0;
}
