using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnitManager : MonoBehaviour
{
    public void Init(UnitHero _hero)
    {
        hero = _hero;
        hero.Init();
    }

    public void Dead()
    {
        hero.Dead();
        
        StartCoroutine("HeroResurrectionTimeCalcCoroutine");
    }

    private IEnumerator HeroResurrectionTimeCalcCoroutine()
    {
        float remainingTime = resurrectionDelay;
        while(remainingTime > 0)
        {
            ArrayHUDCommand.Use(EHUDCommand.HERO_RESURRECTION_UPDATE, remainingTime);
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        ArrayHUDCommand.Use(EHUDCommand.HERO_RESSURECTION_FINISH);
        Ressurection();
    }

    private void Ressurection()
    {
        hero.Resurrection(resurrectionPos);
    }

    [SerializeField]
    private float resurrectionDelay = 30f;
    [SerializeField]
    private Vector3 resurrectionPos = Vector3.zero;

    private UnitHero hero = null;
}
