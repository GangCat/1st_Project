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

        SelectableObject enemyObj = enemyGo.GetComponent<SelectableObject>();
        enemyObj.Init();
        enemyObj.Position = spawnPos;
    }





    MemoryPool memoryPool = null;

    [SerializeField]
    private GameObject enemyPrefab = null;
    [SerializeField]
    private Vector3 spawnPos = Vector3.zero;
}
