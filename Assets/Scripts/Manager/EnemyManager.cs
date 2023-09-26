using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public void Init(PF_Grid _grid)
    {
        grid = _grid;

        waveEnemyHolder = GetComponentInChildren<WaveEnemyHolder>().GetTransform();
        mapEnemyHolder = GetComponentInChildren<MapEnemyHolder>().GetTransform();

        memoryPoolWave = new MemoryPool(enemyPrefab, 5, waveEnemyHolder);
        memoryPoolMap = new MemoryPool(enemyPrefab, 5, mapEnemyHolder);
    }

    public void SpawnWaveEnemy(Vector3 _targetPos, int _count)
    {
        StartCoroutine(SpawnWaveEnemyCoroutine(_targetPos, _count));
    }

    public void SpawnMapEnemy(int _count)
    {
        StartCoroutine("SpawnMapEnemyCoroutine", _count);
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

            yield return null;
        }
    }

    private IEnumerator SpawnMapEnemyCoroutine(int _count)
    {
        for (int i = 0; i < arrEnemySpawnPoint.Length; ++i)
        {
            int unitCnt = 0;
            while (unitCnt < _count)
            {
                Vector3 spawnPos = arrEnemySpawnPoint[i].GetPos + Functions.GetRandomPosition(outerCircleRad, innerCircleRad);
                PF_Node spawnNode = grid.GetNodeFromWorldPoint(spawnPos);
                if (!spawnNode.walkable)
                    continue;

                EnemyObject enemyObj = memoryPoolMap.ActivatePoolItemWithIdx(mapEnemyIdx, 5, mapEnemyHolder).GetComponent<EnemyObject>();
                enemyObj.Position = spawnNode.worldPos;
                enemyObj.Rotate(Random.Range(0,360));
                enemyObj.Init();
                enemyObj.Init(EnemyObject.EEnemySpawnType.MAP_SPAWN, mapEnemyIdx);
                ++mapEnemyIdx;
                ++unitCnt;
            }
            yield return null;
        }
    }

    [SerializeField]
    private GameObject enemyPrefab = null;
    [SerializeField]
    private Vector3 spawnPos = Vector3.zero;
    [SerializeField]
    private EnemySpawnPoint[] arrEnemySpawnPoint = null;

    [Header("-Enemy Map Random Spawn(outer > inner)")]
    [SerializeField]
    private float outerCircleRad = 0f;
    [SerializeField]
    private float innerCircleRad = 0f;

    private MemoryPool memoryPoolWave = null;
    private MemoryPool memoryPoolMap = null;

    private Transform waveEnemyHolder = null;
    private Transform mapEnemyHolder = null;

    private int waveEnemyIdx = 0;
    private int mapEnemyIdx = 0;

    private PF_Grid grid = null;
}
