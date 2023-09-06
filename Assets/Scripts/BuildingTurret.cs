using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTurret : MonoBehaviour
{
    public EBuildingType BuildingType => buildingType;

    public void Init()
    {
        buildingType = EBuildingType.Turret;
    }


    [SerializeField]
    private EBuildingType buildingType = EBuildingType.None;
}
