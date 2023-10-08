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

    public void SpawnNuclear(VoidNuclearDelegate _spwnCompleteCallback)
    {
        if(!hasNuclear)
            StartCoroutine("SpawnNuclearCoroutine", _spwnCompleteCallback);
    }

    private IEnumerator SpawnNuclearCoroutine(VoidNuclearDelegate _spwnCompleteCallback)
    {
        hasNuclear = true;
        float buildFinishTime = Time.time + nuclearProduceDelay;
        while (buildFinishTime > Time.time)
        {
            // ui Ç¥½Ã
            yield return new WaitForSeconds(0.5f);
        }

        SpawnComplete(_spwnCompleteCallback);
    }

    private void SpawnComplete(VoidNuclearDelegate _spwnCompleteCallback)
    {
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
}
