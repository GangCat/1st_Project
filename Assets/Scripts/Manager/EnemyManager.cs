using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public void Init(PF_Grid _grid, Vector3 _mainBasePos)
    {
        grid = _grid;
        mainBasePos = _mainBasePos;

        waveEnemyHolder = GetComponentInChildren<WaveEnemyHolder>().GetTransform();
        mapEnemyHolder = GetComponentInChildren<MapEnemyHolder>().GetTransform();

        memoryPoolWave = new MemoryPool(enemyPrefab, 5, waveEnemyHolder);
        memoryPoolMap = new MemoryPool(enemyPrefab, 5, mapEnemyHolder);

        StartCoroutine("WaveControll");
    }

    private IEnumerator WaveControll()
    {
        int bigWaveCnt = 0;
        float bigWaveTime = Time.time + (bigWaveDelay_min * 60f);
        float smallWaveTime = Time.time + (smallWaveDelay_min * 60f);

        while (totalBigWaveCnt > bigWaveCnt)
        {
            while (bigWaveTime > Time.time)
            {
                if (smallWaveCnt < 2 && smallWaveTime < Time.time)
                {
                    SpawnWaveEnemy(arrWaveStartPoint[bigWaveCnt].GetPos, 20 + bigWaveCnt * 10);
                    smallWaveTime = Time.time + (smallWaveDelay_min * 60f);
                    ++smallWaveCnt;
                }

                yield return new WaitForSeconds(1f);
            }

            ++bigWaveCnt;
            for (int i = 0; i < bigWaveCnt; ++i)
            {
                SpawnWaveEnemy(arrWaveStartPoint[i].GetPos, bigWaveCnt * 100);
                bigWaveTime = Time.time + (bigWaveDelay_min * 60f);
                smallWaveTime = Time.time + (smallWaveDelay_min * 60f);
                smallWaveCnt = 0;
            }
        }
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
        GameObject enemyGo = memoryPoolWave.DeactivatePoolItemWithIdx(_removeGo, _waveEnemyIdx);
        if (enemyGo == null) return;
        // 레이어 변경
        enemyGo.layer = LayerMask.GetMask("EnemyDead");
    }

    public void DeactivateMapEnemy(GameObject _removeGo, int _mapEnemyIdx)
    {
        memoryPoolMap.DeactivatePoolItemWithIdx(_removeGo, _mapEnemyIdx);
    }

    private IEnumerator SpawnWaveEnemyCoroutine(Vector3 _spawnPos, int _count)
    {
        int unitCnt = 0;
        while (unitCnt < _count)
        {
            GameObject enemyGo = memoryPoolWave.ActivatePoolItemWithIdx(waveEnemyIdx, 5, waveEnemyHolder);
            EnemyObject enemyObj = enemyGo.GetComponent<EnemyObject>();
            enemyObj.Position = _spawnPos;
            enemyObj.Init();
            enemyObj.Init(EnemyObject.EEnemySpawnType.WAVE_SPAWN, waveEnemyIdx);
            enemyObj.MoveAttack(mainBasePos);
            ++waveEnemyIdx;
            ++unitCnt;

            if (_spawnPos.x >= 55f)
                _spawnPos.x = 45f;
            else
                _spawnPos.x += 1f;

            yield return null;
        }
    }

    private IEnumerator SpawnMapEnemyCoroutine(int _count)
    {
        for (int i = 0; i < arrMapSpawnPoint.Length; ++i)
        {
            int unitCnt = 0;
            while (unitCnt < _count)
            {
                Vector3 spawnPos = arrMapSpawnPoint[i].GetPos + Functions.GetRandomPosition(outerCircleRad, innerCircleRad);
                PF_Node spawnNode = grid.GetNodeFromWorldPoint(spawnPos);
                if (!spawnNode.walkable)
                    continue;

                EnemyObject enemyObj = memoryPoolMap.ActivatePoolItemWithIdx(mapEnemyIdx, 5, mapEnemyHolder).GetComponent<EnemyObject>();
                enemyObj.Position = spawnNode.worldPos;
                enemyObj.Rotate(Random.Range(0, 360));
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
    private EnemyMapSpawnPoint[] arrMapSpawnPoint = null;

    [Header("-Enemy Map Random Spawn(outer > inner)")]
    [SerializeField]
    private float outerCircleRad = 0f;
    [SerializeField]
    private float innerCircleRad = 0f;

    [Header("-Wave Attribute")]
    [SerializeField]
    private float smallWaveDelay_min = 3f;
    [SerializeField]
    private float bigWaveDelay_min = 10f;
    [SerializeField]
    private int totalBigWaveCnt = 3;
    [SerializeField]
    private WaveStartPoint[] arrWaveStartPoint = null;

    private MemoryPool memoryPoolWave = null;
    private MemoryPool memoryPoolMap = null;

    private Transform waveEnemyHolder = null;
    private Transform mapEnemyHolder = null;

    private Vector3 mainBasePos = Vector3.zero;

    private int waveEnemyIdx = 0;
    private int mapEnemyIdx = 0;
    private int smallWaveCnt = 0;

    private bool isBigWaveTurn = false;

    private PF_Grid grid = null;
}
