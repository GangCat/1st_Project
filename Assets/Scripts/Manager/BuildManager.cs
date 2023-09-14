using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public void Init(PF_Grid _grid)
    {
        grid = _grid;
    }

    public void ShowBluepirnt(ESelectableObjectType _buildingType)
    {
        if (isBlueprint) return;
        switch (_buildingType)
        {
            case ESelectableObjectType.TURRET:
                curBuilding = Instantiate(turretPrefab, transform).GetComponent<Building>();
                break;
            case ESelectableObjectType.BUNKER:
                curBuilding = Instantiate(bunkerPrefab, transform).GetComponent<Building>();
                break;
            case ESelectableObjectType.WALL:
                curBuilding = Instantiate(wallPrefab, transform).GetComponent<Building>();
                break;
            case ESelectableObjectType.NUCLEAR:
                curBuilding = Instantiate(nuclearPrefab, transform).GetComponent<Building>();
                break;
            case ESelectableObjectType.BARRACK:
                curBuilding = Instantiate(barrackPrefab, transform).GetComponent<Building>();
                break;
            default:
                break;
        }
        StartCoroutine("ShowBlueprint");
    }

    private IEnumerator ShowBlueprint()
    {
        isBlueprint = true;

        if (curBuilding == null) yield break;

        curBuilding.Init(grid);
        curBuilding.SetColliderEnable(false);

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
        curBuilding.BuildComplete();
        Destroy(curBuilding.gameObject);
        isBlueprint = false;
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
            isBlueprint = false;
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
    private GameObject barrackPrefab = null;

    private Building curBuilding = null;
    private PF_Grid grid = null;
    private PF_Node curNode = null;
    private bool isBlueprint = false;
}
