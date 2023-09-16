using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBarrack : Structure
{
    public void Init(NodeUpdateDelegate _nodeUpdateCallback)
    {
        nodeUpdateCallback = _nodeUpdateCallback;
    }

    public override void Init()
    {
        spawnPoint = transform.position;
        spawnPoint.y += 4f;
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_MELEE, new CommandSpawnUnit(this, ESpawnUnitType.MELEE));
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_RANGE, new CommandSpawnUnit(this, ESpawnUnitType.RANGE));
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_ROCKET, new CommandSpawnUnit(this, ESpawnUnitType.ROCKET));
    }

    public void SpawnUnit(ESpawnUnitType _unitType)
    {
        // ui에 나타내는 내용
        StartCoroutine("SpawnUnitCoroutine", _unitType);
    }

    private IEnumerator SpawnUnitCoroutine(ESpawnUnitType _unitType)
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnUnitDelay[(int)_unitType])
        {
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;

            // ui에 나타내는 내용


        }

        GameObject unitGo = Instantiate(spawnUnitPrefab[(int)_unitType], spawnPoint, Quaternion.identity);
        unitGo.GetComponent<SelectableObject>().Init();
    }


    [SerializeField]
    private float[] spawnUnitDelay = new float[(int)ESpawnUnitType.LENGTH];
    [SerializeField]
    private GameObject[] spawnUnitPrefab = new GameObject[(int)ESpawnUnitType.LENGTH];

    private Vector3 spawnPoint = Vector3.zero;

    private NodeUpdateDelegate nodeUpdateCallback = null;
}
