using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureNuclear : Structure
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        myNuclear = GetComponentInChildren<MissileNuclear>();
        myNuclear.SetActive(false);
    }

    public bool IsProcessingSpawnNuclear => isProcessingSpawnNuclear;

    public override void CancleCurAction()
    {
        if (isProcessingUpgrade)
        {
            StopCoroutine("UpgradeCoroutine");
            isProcessingUpgrade = false;
            curUpgradeType = EUpgradeType.NONE;
        }
        else if (isProcessingConstruct)
        {
            StopCoroutine("BuildStructureCoroutine");
            isProcessingConstruct = false;
            ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DEMOLISH_COMPLETE, myStructureIdx);
            DestroyStructure();
        }
        else if (isProcessingDemolish)
        {
            StopCoroutine("DemolishCoroutine");
            isProcessingDemolish = false;
        }
        else if (isProcessingSpawnNuclear)
        {
            StopCoroutine("SpawnNuclearCoroutine");
            isProcessingSpawnNuclear = false;
        }

        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }

    public void SpawnNuclear(VoidNuclearDelegate _spwnCompleteCallback)
    {
        if(!isProcessingSpawnNuclear && !hasNuclear)
            StartCoroutine("SpawnNuclearCoroutine", _spwnCompleteCallback);
    }

    private IEnumerator SpawnNuclearCoroutine(VoidNuclearDelegate _spwnCompleteCallback)
    {
        isProcessingSpawnNuclear = true;
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
        float buildFinishTime = Time.time + nuclearProduceDelay;
        while (buildFinishTime > Time.time)
        {
            // ui Ç¥½Ã
            yield return new WaitForSeconds(0.5f);
        }

        isProcessingSpawnNuclear = false;
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
        SpawnComplete(_spwnCompleteCallback);
    }

    private void SpawnComplete(VoidNuclearDelegate _spwnCompleteCallback)
    {
        hasNuclear = true;
        myNuclear.SetPos(nuclearSpawnPos);
        myNuclear.SetActive(true);
        myNuclear.ResetRotate();
        _spwnCompleteCallback?.Invoke(this);
    }

    public void LaunchNuclear(Vector3 _destPos)
    {
        myNuclear.Launch(_destPos);
        hasNuclear = false;
    }


    [SerializeField]
    private float nuclearProduceDelay = 0f;
    [SerializeField]
    private Vector3 nuclearSpawnPos = Vector3.zero;

    private MissileNuclear myNuclear = null;
    private bool hasNuclear = false;
    private bool isProcessingSpawnNuclear = false;
}
