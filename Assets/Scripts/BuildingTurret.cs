using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTurret : Building
{
    public void Awake()
    {
        selectObj = GetComponentInChildren<SelectableObject>();
        selectObj.Init();
    }

    private SelectableObject selectObj = null;
}
