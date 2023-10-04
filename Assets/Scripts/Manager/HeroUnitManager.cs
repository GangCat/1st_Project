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
        
        StartCoroutine("HeroRessurectionTimeCalcCoroutine");
    }

    private IEnumerator HeroRessurectionTimeCalcCoroutine()
    {
        float remainingTime = ressurectionDelay;
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
        hero.Ressurection(ressurectionPos);
    }

    [SerializeField]
    private float ressurectionDelay = 30f;
    [SerializeField]
    private Vector3 ressurectionPos = Vector3.zero;

    private UnitHero hero = null;
}
