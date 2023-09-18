using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBarrack : Structure
{
    public override void Init()
    {
        spawnPoint = transform.position;
        rallyPoint = spawnPoint;
    }

    public void SetRallyPoint(Vector3 _rallyPoint)
    {
        rallyTr = null;
        rallyPoint = _rallyPoint;
    }

    public void SetRallyPoint(Transform _rallyTr)
    {
        rallyPoint = spawnPoint;
        rallyTr = _rallyTr;
    }

    public void SpawnUnit(ESpawnUnitType _unitType)
    {
        if (listUnit.Count >= 5) return;

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
        SelectableObject tempObj = unitGo.GetComponent<SelectableObject>();
        tempObj.Init();

        if(!rallyPoint.Equals(spawnPoint))
            tempObj.MoveByPos(rallyPoint);
        else if (rallyTr != null)
            tempObj.FollowTarget(rallyTr);

        RequestSpawnUnit();
    }

    [Header("-Melee(temp), Range, Rocket(temp)")]
    [SerializeField]
    private float[] spawnUnitDelay = new float[(int)ESpawnUnitType.LENGTH];
    [SerializeField]
    private GameObject[] spawnUnitPrefab = new GameObject[(int)ESpawnUnitType.LENGTH];

    private bool isProcessingSpawnUnit = false;

    private Vector3 spawnPoint = Vector3.zero;
    private Vector3 rallyPoint = Vector3.zero;
    private Transform rallyTr = null;
    private List<ESpawnUnitType> listUnit = new List<ESpawnUnitType>();
}
