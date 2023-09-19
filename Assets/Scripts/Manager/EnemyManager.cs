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

    private MemoryPool memoryPool = null;

    [SerializeField]
    private GameObject enemyPrefab = null;
    [SerializeField]
    private Vector3 spawnPos = Vector3.zero;
}
