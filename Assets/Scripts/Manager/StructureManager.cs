using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    private enum EStructureType { NONE = -1, TURRET, BUNKER, BARRACK, NUCLEAR, WALL, LENGTH }
    public void Init(PF_Grid _grid)
    {
        grid = _grid;
        dicStructure = new Dictionary<int, Structure>();
    }

    public void ShowBluepirnt(ESelectableObjectType _buildingType)
    {
        if (isBlueprint)
        {
            Destroy(curStructure.gameObject);
            StopAllCoroutines();
        }

        switch (_buildingType)
        {
            case ESelectableObjectType.TURRET:
                {
                    curStructureType = EStructureType.TURRET;
                    curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.TURRET], transform).GetComponent<Structure>();
                }
                break;
            case ESelectableObjectType.BUNKER:
                {
                    curStructureType = EStructureType.BUNKER;
                    curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.BUNKER], transform).GetComponent<Structure>();
                }
                break;
            case ESelectableObjectType.NUCLEAR:
                {
                    curStructureType = EStructureType.NUCLEAR;
                    curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.NUCLEAR], transform).GetComponent<Structure>();
                }
                break;
            case ESelectableObjectType.BARRACK:
                {
                    curStructureType = EStructureType.BARRACK;
                    curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.BARRACK], transform).GetComponent<Structure>();
                }
                break;
            default:
                break;
        }
        StartCoroutine("ShowBlueprint");
    }

    public void ShowBluepirnt(Transform _bunkerTr)
    {
        curStructureType = EStructureType.WALL;
        curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.WALL], transform).GetComponent<Structure>();
        StartCoroutine("ShowWallBlueprint", _bunkerTr);
    }

    public void DestroyStructure(int _structureIdx)
    {
        Structure structure = null;
        dicStructure.TryGetValue(_structureIdx, out structure);
        InstantiateRuin(_structureIdx);
        structure.DestroyStructure();
    }

    public void DestroyStructure()
    {
        isStructureDestroy = true;
    }

    private IEnumerator ShowBlueprint()
    {
        isBlueprint = true;

        if (curStructure == null) yield break;

        curStructure.Init(grid);

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
            StopBuildCoroutine();

            StartCoroutine("BuildStructureCoroutine");

            return false;
        }
        return true;
    }

    private void StopBuildCoroutine()
    {
        StopCoroutine("ShowWallBlueprint");
        StopCoroutine("ShowBlueprint");
    }

    private IEnumerator BuildStructureCoroutine()
    {
        Structure newStructure = Instantiate(arrStructurePrefab[(int)curStructureType], curStructure.transform.position, Quaternion.identity).GetComponent<Structure>();
        dicStructure.Add(structureIdx, newStructure);
        ++structureIdx;
        newStructure.gameObject.SetActive(false);
        List<GameObject> listHBeam = new List<GameObject>();
        InstantiateHBeam(listHBeam);
        Destroy(curStructure.gameObject);
        isBlueprint = false;

        float buildFinishTime = Time.time + buildDelay[(int)curStructureType];
        while (buildFinishTime > Time.time)
        {
            // ui 표시
            if (isStructureDestroy)
            {
                DestroyHBeam(listHBeam);
                Destroy(newStructure.gameObject);
                isStructureDestroy = false;
                yield break;
            }

            yield return null;
        }

        DestroyHBeam(listHBeam);
        //curStructure.DestroyHBeam();
        //newStructure.SetColliderEnable(true);
        newStructure.gameObject.SetActive(true);
        newStructure.Init(grid);
        newStructure.Init(structureIdx);
        newStructure.BuildComplete();
        newStructure.UpdateNodeWalkable(false);
        newStructure.transform.parent = transform;
        
        yield return null;
    }

    #region HBeam
    private void InstantiateHBeam(List<GameObject> _listHBeam)
    {
        curNode = grid.GetNodeFromWorldPoint(curStructure.transform.position);
        int gridX = curNode.gridX;
        int gridY = curNode.gridY;
        int structureGirdX = curStructure.GridX;
        int structureGirdY = curStructure.GridY;
        int StructureFactorX = curStructure.FactorX;
        int StructureFactorY = curStructure.FactorY;
        int idx = 0;

        while (idx < structureGirdX * structureGirdY)
        {
            PF_Node curRuinNode = grid.GetNodeWithGrid(idx % structureGirdX * StructureFactorX + gridX, idx / structureGirdY * StructureFactorY + gridY);
            _listHBeam.Add(Instantiate(HBeamPrefab, curRuinNode.worldPos, Quaternion.identity));
            _listHBeam[idx].GetComponent<Structure>().Init(grid);
            _listHBeam[idx].GetComponent<Structure>().Init(grid);
            grid.UpdateNodeWalkable(curRuinNode, false);
            ++idx;
        }
    }

    private void DestroyHBeam(List<GameObject> _listHBeam)
    {
        for (int i = 0; i < _listHBeam.Count; ++i)
        {
            _listHBeam[i].GetComponent<Structure>().UpdateNodeWalkable(false);
            Destroy(_listHBeam[i]);
        }
        _listHBeam.Clear();
    }
    #endregion

    #region Ruin
    private void InstantiateRuin(int _structureIdx)
    {
        Structure tempStructure = null;
        dicStructure.TryGetValue(_structureIdx, out tempStructure);
        curNode = grid.GetNodeFromWorldPoint(tempStructure.transform.position);
        int gridX = curNode.gridX;
        int gridY = curNode.gridY;
        int stuructureGridX = tempStructure.GridX;
        int stuructureGridY = tempStructure.GridY;
        int StructureFactorX = tempStructure.FactorX;
        int StructureFactorY = tempStructure.FactorY;
        int idx = 0;

        while (idx < stuructureGridX * stuructureGridY)
        {
            PF_Node curRuinNode = grid.GetNodeWithGrid(idx % stuructureGridX * StructureFactorX + gridX, idx / stuructureGridY * StructureFactorY + gridY);
            Instantiate(ruinPrefab, curRuinNode.worldPos, Quaternion.identity).GetComponent<Structure>().Init(grid);
            grid.UpdateNodeWalkable(curRuinNode, true);
            ++idx;
        }
    }
    #endregion
    public void DeactivateUnit(GameObject _removeGo, ESpawnUnitType _unitType, int _barrackIdx)
    {
        Structure barrack = null;
        dicStructure.TryGetValue(_barrackIdx, out barrack);
        barrack.DeactivateUnit(_removeGo, _unitType);
    }


    [Header("-StructurePrefab(TURRET, BUNKER, BARRACK, NUCLEAR, WALL)")]
    [SerializeField]
    private GameObject[] arrStructurePrefab = null;

    [Header("-BlueprintPrefab(TURRET, BUNKER, BARRACK, NUCLEAR, WALL)")]
    [SerializeField]
    private GameObject[] arrBlueprintPrefab = null;

    [Header("-OtherPrefab")]
    [SerializeField]
    private GameObject ruinPrefab = null;
    [SerializeField]
    private GameObject HBeamPrefab = null;

    [Header("-Build Delay(TURRET, BUNKER, BARRACK, NUCLEAR, WALL)")]
    [SerializeField]
    private float[] buildDelay = new float[(int)EStructureType.LENGTH];

    private Dictionary<int, Structure> dicStructure = null;
    private EStructureType curStructureType = EStructureType.NONE;
    private Structure curStructure = null;
    private PF_Grid grid = null;
    private PF_Node curNode = null;

    private bool isBlueprint = false;
    private bool isStructureDestroy = false;
    private int structureIdx = 0;
}
