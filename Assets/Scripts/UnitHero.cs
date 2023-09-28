using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHero : MonoBehaviour
{
    public void Init()
    {
        GetComponent<SelectableObject>().Init();
    }
}
