using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTurretAttack : IState
{
    public void Start(ref SUnitState _structState)
    {
        myTr = _structState.myTr;
        targetTr = _structState.targetTr;
        attRate = _structState.attRate;
        spawnTr = _structState.missileSpawnTr;
        objectType = _structState.objectType;
    }

    public void Update(ref SUnitState _structState)
    {
        if (targetTr == null) return;
        if (targetTr.gameObject.activeSelf == false) return;
        if (_structState.isPause) return;

        dir = targetTr.position - myTr.position;
        dir.y = 0f;
        myTr.rotation = Quaternion.LookRotation(dir);
        elapsedTime += Time.deltaTime;

        if (elapsedTime > attRate)
        {
            elapsedTime = 0f;
            // 공격 애니메이션 출력
            

            GameObject missile = GameObject.Instantiate(_structState.TurretMissile, spawnTr.position, spawnTr.rotation);
            missile.GetComponent<MissileTurret>().Init(spawnTr.position);

            // 미사일 발사
            Debug.Log("Missile Fire!!");
            AudioManager.instance.PlayAudio_Attack(objectType);
        }
    }
    public void End(ref SUnitState _structState)
    {
        
    }

    private float elapsedTime = 0f;
    private float attRate = 0f;

    private Transform targetTr = null;
    private Transform myTr = null;
    private Transform spawnTr = null;
    private EObjectType objectType;
    
    
    private Vector3 dir = Vector3.zero;
}
