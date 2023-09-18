using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
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
                curStructure = Instantiate(turretPrefab, transform).GetComponent<Structure>();
                break;
            case ESelectableObjectType.BUNKER:
                curStructure = Instantiate(bunkerPrefab, transform).GetComponent<Structure>();
                break;
            case ESelectableObjectType.WALL:
                curStructure = Instantiate(wallPrefab, transform).GetComponent<Structure>();
                break;
            case ESelectableObjectType.NUCLEAR:
                curStructure = Instantiate(nuclearPrefab, transform).GetComponent<Structure>();
                break;
            case ESelectableObjectType.BARRACK:
                curStructure = Instantiate(barrackPrefab, transform).GetComponent<Structure>();

                break;
            default:
                break;
        }
        StartCoroutine("ShowBlueprint");
    }

    private IEnumerator ShowBlueprint()
    {
        isBlueprint = true;

        if (curStructure == null) yield break;

        curStructure.Init(grid);
        curStructure.SetColliderEnable(false);

        RaycastHit hit;
        while (true)
        {
            Functions.Picking(1<<LayerMask.NameToLayer("StageFloor"), out hit);
            curNode = grid.GetNodeFromWorldPoint(hit.point);
            curStructure.SetPos(curNode.worldPos);
            
            yield return null;
        }
    }

    public bool CancleBuild()
    {
        StopAllCoroutines();
        curStructure.BuildComplete();
        Destroy(curStructure.gameObject);
        isBlueprint = false;
        return false;
    }

    public bool BuildStructure()
    {
        if (curStructure.IsBuildable)
        {
            StopAllCoroutines();

            curStructure.SetColliderEnable(true);
            curStructure.UpdateNodeUnWalkable();
            curStructure.BuildComplete();
            curStructure.transform.parent = null;
            if(curStructure.GetComponent<SelectableObject>().ObjectType.Equals(ESelectableObjectType.BUNKER))
                curStructure.Init(bunkerIdx++);
            else
                curStructure.Init();
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

    private Structure curStructure = null;
    private PF_Grid grid = null;
    private PF_Node curNode = null;
    private bool isBlueprint = false;

    private int bunkerIdx = 0;
}
