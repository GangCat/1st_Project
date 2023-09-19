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

    public void ShowBluepirnt(Transform _bunkerTr)
    {
        if (isBlueprint) return;

        curStructure = Instantiate(wallPrefab, transform).GetComponent<Structure>();
        StartCoroutine("ShowWallBlueprint", _bunkerTr);
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

    private IEnumerator ShowWallBlueprint(Transform _bunkerTr)
    {
        isBlueprint = true;

        if (curStructure == null) yield break;

        curStructure.Init(grid);
        curStructure.SetColliderEnable(false);
        Vector3 bunkerPos = _bunkerTr.position;
        Vector3 wallPos = Vector3.zero;
        float angle = 0f;

        RaycastHit hit;
        while (true)
        {
            Functions.Picking(1 << LayerMask.NameToLayer("StageFloor"), out hit);
            angle = Functions.CalcAngleToTarget(bunkerPos, hit.point);
            wallPos = bunkerPos;

            // 각도에 맞게 회전시키기
            // left -135 ~ 135
            if (angle > 135 || angle < -135)
            {
                curStructure.SetGrid(4, 1);
                curStructure.SetFactor(-1, 1);
                curStructure.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                wallPos.x = bunkerPos.x - 1;
                curStructure.SetPos(wallPos);
            }
            // up 135 ~ 45
            else if (angle > 45)
            {
                curStructure.SetGrid(1, 4);
                curStructure.SetFactor(1, 1);
                curStructure.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                wallPos.z = bunkerPos.z + 1;
                curStructure.SetPos(wallPos);
            }
            // right 45 ~ -45
            else if (angle > -45)
            {
                curStructure.SetGrid(4, 1);
                curStructure.SetFactor(1, 1);
                curStructure.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                wallPos.x = bunkerPos.x + 1;
                curStructure.SetPos(wallPos);
            }
            // down -45 ~ -135
            else
            {
                curStructure.SetGrid(1, 4);
                curStructure.SetFactor(1, -1);
                curStructure.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                wallPos.z = bunkerPos.z - 1;
                curStructure.SetPos(wallPos);
            }

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
}
