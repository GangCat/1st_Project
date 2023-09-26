using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHp : MonoBehaviour
{
    public void Init()
    {
        curHp = maxHp;
    }

    /// <summary>
    /// 체력이 0 이하로 떨어지면 true 반환
    /// </summary>
    /// <param name="_dmg"></param>
    /// <returns></returns>
    public bool DecreaseHpAndCheckIsDead(float _dmg)
    {
        curHp -= _dmg;
        return curHp <= 0 ? true : false;
    }

    public void IncreaseCurHp(float _heal)
    {
        curHp += _heal;
        if (curHp > maxHp) curHp = maxHp;
    }

    public void UpgradeHp(float _increaseHp)
    {
        maxHp += _increaseHp;
        curHp += _increaseHp;
    }

    [SerializeField]
    private float curHp = 0f;
    [SerializeField]
    private float maxHp = 100f;
}
