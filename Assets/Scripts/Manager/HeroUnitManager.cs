using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnitManager : MonoBehaviour
{
    public void Init(UnitHero _hero)
    {
        hero = _hero;
        hero.Init();
#if UNITY_EDITOR
        StartCoroutine("DisplayHeroStateCoroutine");
#endif
    }

    public void DebugMode(bool _isDebugMode)
    {
        if (_isDebugMode)
            StartCoroutine("DisplayHeroStateCoroutine");
        else
            StopCoroutine("DisplayHeroStateCoroutine");
    }

#if UNITY_EDITOR
    public static void UpdateCurState(EState _state)
    {
        curState = _state;
    }

    private IEnumerator DisplayHeroStateCoroutine()
    {
        while (true)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(hero.transform.position);
            ArrayDebugModeCommand.Use(EDebugModeCommand.MOVE_STATE_INDICATOR, screenPos, curState);

            yield return null;
        }
    }
#endif
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
    private static EState curState = EState.NONE;
}
