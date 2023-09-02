using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public void Init()
    {
        move = GetComponentInChildren<PlayerMovement>();
        //move.Init();

        statusHp = GetComponentInChildren<StatusHp>();
        //statusHp.Init();

        //StartCoroutine("TestHpSystemCoroutine");
    }

    public void MovePlayerByPicking(Vector3 _pickPos)
    {
        move.MovePlayerByPicking(_pickPos);
    }

    /*
    private IEnumerator TestHpSystemCoroutine()
    {
        yield return new WaitForSeconds(3f);

        statusHp.DecreaseHpAndCheckIsDead(30f);
        yield return new WaitForSeconds(2f);

        statusHp.IncreaseCurHp(20f);
        yield return new WaitForSeconds(2f);

        bool isDead = statusHp.DecreaseHpAndCheckIsDead(200f);
        Debug.Log(isDead);
    }
    */
    



    private PlayerMovement move = null;
    private StatusHp statusHp = null;
}
