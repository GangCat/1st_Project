using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBarrack : Structure
{
    public override void Init()
    {
        spawnPoint = transform.position;
        // 스폰 포인트를 노드로 설정하기
        // 해당 노드가 walkable이 아니면 다음 노드로 이동.
        // 생성되는 위치는 이 건물 둘러싸도록 설정.
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_MELEE, new CommandSpawnUnit(this, ESpawnUnitType.MELEE));
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_RANGE, new CommandSpawnUnit(this, ESpawnUnitType.RANGE));
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_ROCKET, new CommandSpawnUnit(this, ESpawnUnitType.ROCKET));
    }

    public void SpawnUnit(ESpawnUnitType _unitType)
    {
        listUnit.Add(_unitType);
        RequestSpawnUnit();
        // ui에 나타내는 내용
    }

    private void RequestSpawnUnit()
    {
        if (!isProcessingSpawnUnit && listUnit.Count > 0)
        {
            isProcessingSpawnUnit = true;
            ESpawnUnitType unitType = listUnit[0];
            listUnit.RemoveAt(0);
            StartCoroutine("SpawnUnitCoroutine", unitType);
        }
    }

    private IEnumerator SpawnUnitCoroutine(ESpawnUnitType _unitType)
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnUnitDelay[(int)_unitType])
        {
            // elapsedTime으로 그 게이지 바 표시하기
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
        }

        isProcessingSpawnUnit = false;
        GameObject unitGo = Instantiate(spawnUnitPrefab[(int)_unitType], spawnPoint, Quaternion.identity);
        unitGo.transform.position = SelectableObjectManager.ResetPosition(unitGo.transform.position);
        unitGo.GetComponent<SelectableObject>().Init();
        RequestSpawnUnit();
    }

    [Header("-Melee, Range, Rocket(temp)")]
    [SerializeField]
    private float[] spawnUnitDelay = new float[(int)ESpawnUnitType.LENGTH];
    [SerializeField]
    private GameObject[] spawnUnitPrefab = new GameObject[(int)ESpawnUnitType.LENGTH];

    private bool isProcessingSpawnUnit = false;

    private Vector3 spawnPoint = Vector3.zero;
    private List<ESpawnUnitType> listUnit = new List<ESpawnUnitType>();
}
