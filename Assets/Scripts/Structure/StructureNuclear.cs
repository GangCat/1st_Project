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
        upgradeLevel = 3;
    }

    public void UpdateSpawnNuclearInfo()
    {
        ArrayHUDSpawnNuclearCommand.Use(EHUDSpawnNuclearCommand.UPDATE_SPAWN_NUCLEAR_TIME, progressPercent);
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
            ArrayRefundCurrencyCommand.Use(ERefuncCurrencyCommand.BUILD_STRUCTURE, myObj.GetObjectType());
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
            ArrayRefundCurrencyCommand.Use(ERefuncCurrencyCommand.SPAWN_NUCLEAR);
            StopCoroutine("SpawnNuclearCoroutine");
            isProcessingSpawnNuclear = false;
        }

        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }

    public void SpawnNuclear(VoidNuclearDelegate _spwnCompleteCallback)
    {
        if(!isProcessingSpawnNuclear && !hasNuclear)
        {
            StartCoroutine("SpawnNuclearCoroutine", _spwnCompleteCallback);
        }
    }

    private IEnumerator SpawnNuclearCoroutine(VoidNuclearDelegate _spwnCompleteCallback)
    {
        isProcessingSpawnNuclear = true;
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
        float elapsedTime = 0f;
        progressPercent = elapsedTime / nuclearProduceDelay;
        while (progressPercent < 1)
        {
            while (isPause)
                yield return null;

            if (myObj.IsSelect)
                ArrayHUDSpawnNuclearCommand.Use(EHUDSpawnNuclearCommand.UPDATE_SPAWN_NUCLEAR_TIME, progressPercent);
            // ui Ç¥½Ã
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / nuclearProduceDelay;
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
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }


    [SerializeField]
    private float nuclearProduceDelay = 0f;
    [SerializeField]
    private Vector3 nuclearSpawnPos = Vector3.zero;

    private MissileNuclear myNuclear = null;
    private bool hasNuclear = false;
    private bool isProcessingSpawnNuclear = false;
}
