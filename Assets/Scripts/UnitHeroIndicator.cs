using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHeroIndicator : MonoBehaviour
{
    public void Init()
    {
        GetComponent<SelectableObject>().Init();
    }
}
