using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHp : MonoBehaviour
{
    public void Init()
    {
        curHp = maxHp;
    }

    public bool DecreaseHpAndCheckIsDead(float _dmg)
    {
        curHp -= _dmg;
        return curHp < 0 ? true : false;
    }

    public void IncreaseCurHp(float _heal)
    {
        curHp += _heal;
        if (curHp > maxHp) curHp = maxHp;
    }


    [SerializeField]
    private float curHp = 0f;
    [SerializeField]
    private float maxHp = 100f;
}
