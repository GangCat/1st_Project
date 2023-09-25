using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureNuclear : Structure
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        myMissile = GetComponentInChildren<MissileNuclear>();
        myMissile.SetActive(false);
    }

    public void SpawnMissile()
    {
        StartCoroutine("SpawnMissileCoroutine");
    }

    private IEnumerator SpawnMissileCoroutine()
    {
        float buildFinishTime = Time.time + missileProduceDelay;
        while (buildFinishTime > Time.time)
        {
            // ui Ç¥½Ã
            yield return new WaitForSeconds(0.5f);
        }

        SpawnComplete();
    }

    private void SpawnComplete()
    {
        myMissile.SetActive(true);
    }

    public void LaunchNuclear(Vector3 _destPos)
    {
        myMissile.Launch(_destPos);
    }


    [SerializeField]
    private float missileProduceDelay = 0f;

    private MissileNuclear myMissile = null;

}
