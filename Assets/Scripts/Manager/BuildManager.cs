using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public void ShowBlutpirnt(EBuildingType _buildingType)
    {
        Debug.Log(_buildingType);
        //StartCoroutine("ShowBlueprint", _buildingType);
    }

    [SerializeField]
    private GameObject[] buildingPrefabs = null;
}
