using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHero : MonoBehaviour
{
    public void Init()
    {
        myObj = GetComponent<FriendlyObject>();
        statusHp = GetComponent<StatusHp>();
        myObj.Init();
    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }

    public void Ressurection(Vector3 _ressurectionPos)
    {
        transform.position = _ressurectionPos;
        gameObject.SetActive(true);
        myObj.SetIdleState();
        statusHp.Init();
    }

    private FriendlyObject myObj = null;
    private StatusHp statusHp = null;
}
