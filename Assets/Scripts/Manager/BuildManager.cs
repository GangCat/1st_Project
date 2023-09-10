using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public void Init()
    {
        
    }

    public void ShowBluepirnt(ESelectableObjectType _buildingType)
    {
        Debug.Log(_buildingType);
        switch (_buildingType)
        {
            case ESelectableObjectType.TURRET:
                curBuilding = Instantiate(turretPrefab, transform).GetComponent<Building>();
                curBuilding.Init(grid);
                curBuilding.SetColliderEnable(false);
                StartCoroutine("ShowBlueprint");
                break;
            case ESelectableObjectType.BUNKER:
                curBuilding = Instantiate(bunkerPrefab, transform).GetComponent<Building>();
                curBuilding.Init(grid);
                curBuilding.SetColliderEnable(false);
                StartCoroutine("ShowBlueprint");
                break;
            case ESelectableObjectType.WALL:
                curBuilding = Instantiate(wallPrefab, transform).GetComponent<Building>();
                curBuilding.Init(grid);
                curBuilding.SetColliderEnable(false);
                StartCoroutine("ShowBlueprint");
                break;
            case ESelectableObjectType.NUCLEAR:
                curBuilding = Instantiate(nuclearPrefab, transform).GetComponent<Building>();
                curBuilding.Init(grid);
                curBuilding.SetColliderEnable(false);
                StartCoroutine("ShowBlueprint");
                break;
            default:
                break;
        }
    }

    private IEnumerator ShowBlueprint()
    {
        RaycastHit hit;
        while (true)
        {
            Functions.Picking(1<<LayerMask.NameToLayer("StageFloor"), out hit);
            curNode = grid.NodeFromWorldPoint(hit.point);
            curBuilding.SetPos(curNode.worldPos);

            yield return null;
        }
    }

    public bool CancleBuild()
    {
        StopAllCoroutines();
        Destroy(curTargetBuilding);
        return false;
    }

    public bool BuildStructure()
    {
        if (curBuilding.IsBuildable)
        {
            StopAllCoroutines();

            curBuilding.SetColliderEnable(true);
            curBuilding.UpdateNodeUnWalkable();
            curBuilding.BuildComplete();
            curBuilding.transform.parent = null;
            return false;
        }

        return true;

    }

    [SerializeField]
    private GameObject turretPrefab = null;
    [SerializeField]
    private GameObject bunkerPrefab = null;
    [SerializeField]
    private GameObject wallPrefab = null;
    [SerializeField]
    private GameObject nuclearPrefab = null;
    [SerializeField]
    private PF_Grid grid = null;

    private GameObject curTargetBuilding = null;
    private Building curBuilding = null;
    private PF_Node curNode = null;

}
