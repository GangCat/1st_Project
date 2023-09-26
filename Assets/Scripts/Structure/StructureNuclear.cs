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
        float buildFinishTime = Time.time + NuclearProduceDelay;
        while (buildFinishTime > Time.time)
        {
            // ui Ç¥½Ã
            yield return new WaitForSeconds(0.5f);
        }

        SpawnComplete(_spwnCompleteCallback);
    }

    private void SpawnComplete(VoidNuclearDelegate _spwnCompleteCallback)
    {
        myNuclear.SetActive(true);
        _spwnCompleteCallback?.Invoke(this);
    }

    public void LaunchNuclear(Vector3 _destPos)
    {
        Debug.Log(StructureIdx);
        myNuclear.Launch(_destPos);
        hasNuclear = false;
    }


    [SerializeField]
    private float NuclearProduceDelay = 0f;

    private MissileNuclear myNuclear = null;
    private bool hasNuclear = false;
}
