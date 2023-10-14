using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IPauseObserver
{
    public void Init(PF_Grid _grid, Vector3 _mainBasePos)
    {
        ArrayPauseCommand.Use(EPauseCOmmand.REGIST, this);
        grid = _grid;
        mainBasePos = _mainBasePos;

        waveEnemyHolder = GetComponentInChildren<WaveEnemyHolder>().GetTransform();
        mapEnemyHolder = GetComponentInChildren<MapEnemyHolder>().GetTransform();

        memoryPoolWave = new MemoryPool(enemyPrefab, 5, waveEnemyHolder);
        memoryPoolMap = new MemoryPool(enemyPrefab, 5, mapEnemyHolder);
        ArrayHUDCommand.Use(EHUDCommand.INIT_WAVE_TIME, bigWaveDelay_sec);
        SpawnMapEnemy();
        StartCoroutine("WaveControll");
    }

    private IEnumerator WaveControll()
    {
        int bigWaveCnt = 0;
        float bigWaveTimeDelay = 0f;
        float smallWaveTimeDelay = 0f;

        while (bigWaveCnt < totalBigWaveCnt)
        {
            while (bigWaveTimeDelay <= bigWaveDelay_sec)
            {
                while (isPause)
                    yield return null;

                ArrayHUDCommand.Use(EHUDCommand.UPDATE_WAVE_TIME, bigWaveDelay_sec - bigWaveTimeDelay);
                if (smallWaveCnt < 2 && smallWaveTimeDelay >= smallWaveDelay_sec)
                {
                    SpawnWaveEnemy(arrWaveStartPoint[bigWaveCnt].GetPos, 20 + bigWaveCnt * 10);
                    smallWaveTimeDelay = 0f;

                    ++smallWaveCnt;
                }

                yield return new WaitForSeconds(1f);
                bigWaveTimeDelay += 1f;
                smallWaveTimeDelay += 1f;
            }

            ++bigWaveCnt;

            if (bigWaveCnt.Equals(totalBigWaveCnt))
            {
                FinalWaveStart();
                yield break;
            }

            for (int i = 0; i < bigWaveCnt; ++i)
            {
                SpawnWaveEnemy(arrWaveStartPoint[i].GetPos, bigWaveCnt * 100);
                bigWaveTimeDelay = 0f;
                smallWaveTimeDelay = 0f;
                smallWaveCnt = 0;
            }
        }

    }

    private void FinalWaveStart()
    {
        ArrayHUDCommand.Use(EHUDCommand.UPDATE_WAVE_TIME, 0f);
        for (int i = 0; i < totalBigWaveCnt; ++i)
            SpawnWaveEnemy(arrWaveStartPoint[i].GetPos, totalBigWaveCnt * 100);

        EnemyObject[] arrAllMapEnemy = mapEnemyHolder.GetComponentsInChildren<EnemyObject>();
        for (int i = 0; i < arrAllMapEnemy.Length; ++i)
            arrAllMapEnemy[i].MoveAttack(mainBasePos);
    }

    public void SpawnWaveEnemy(Vector3 _targetPos, int _count)
    {
        StartCoroutine(SpawnWaveEnemyCoroutine(_targetPos, _count));
    }

    private void SpawnMapEnemy()
    {
        StartCoroutine("SpawnMapEnemyCoroutine");
    }

    public void DeactivateWaveEnemy(GameObject _removeGo, int _waveEnemyIdx)
    {
        GameObject enemyGo = memoryPoolWave.DeactivatePoolItemWithIdx(_removeGo, _waveEnemyIdx);
        if (enemyGo == null) return;
        // 레이어 변경
        enemyGo.layer = LayerMask.NameToLayer("EnemyDead");
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

    private IEnumerator SpawnMapEnemyCoroutine()
    {
        for (int i = 0; i < arrMapSpawnPoint.Length; ++i)
        {
            int unitCnt = 0;
            while (unitCnt < mapSpawnCnt)
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

    public void CheckPause(bool _isPause)
    {
        isPause = _isPause;
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
    [SerializeField]
    private int mapSpawnCnt = 0;

    [Header("-Wave Attribute")]
    [SerializeField]
    private float smallWaveDelay_sec = 3;
    [SerializeField]
    private float bigWaveDelay_sec = 10;
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
    private bool isPause = false;

    private PF_Grid grid = null;
}
