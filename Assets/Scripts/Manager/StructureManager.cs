using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public enum EStructureType { NONE = -1, TURRET, BUNKER, BARRACK, NUCLEAR, WALL, LENGTH }
    public void Init(PF_Grid _grid, StructureMainBase _mainBase)
    {
        grid = _grid;
        dicStructure = new Dictionary<int, Structure>();
        listNuclearStructure = new List<StructureNuclear>();
        dicStructure.Add(0, _mainBase);
        ++structureIdx;
    }

    public static int UpgradeLimit
    {
        get => upgradeLimit;
        set 
        {
            if (value < 4)
                upgradeLimit = value;
            else
                upgradeLimit = 3;
        }
    }

    public EObjectType CurStructureType()
    {
        return curStructureObjType;
    }

    public void ShowBluepirnt(EObjectType _buildingType)
    {
        if (isBlueprint)
        {
            Destroy(curStructure.gameObject);
            StopAllCoroutines();
        }

        switch (_buildingType)
        {
            case EObjectType.TURRET:
                {
                    curStructureType = EStructureType.TURRET;
                    curStructureObjType = EObjectType.TURRET;
                    curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.TURRET], transform).GetComponent<Structure>();
                }
                break;
            case EObjectType.BUNKER:
                {
                    curStructureType = EStructureType.BUNKER;
                    curStructureObjType = EObjectType.BUNKER;
                    curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.BUNKER], transform).GetComponent<Structure>();
                }
                break;
            case EObjectType.NUCLEAR:
                {
                    curStructureType = EStructureType.NUCLEAR;
                    curStructureObjType = EObjectType.NUCLEAR;
                    curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.NUCLEAR], transform).GetComponent<Structure>();
                }
                break;
            case EObjectType.BARRACK:
                {
                    curStructureType = EStructureType.BARRACK;
                    curStructureObjType = EObjectType.BARRACK;
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
        curStructureObjType = EObjectType.WALL;
        curStructure = Instantiate(arrBlueprintPrefab[(int)EStructureType.WALL], transform).GetComponent<Structure>();
        StartCoroutine("ShowWallBlueprint", _bunkerTr);
    }

    public void Demolish(int _structureIdx)
    {
        StartCoroutine("DemolishCoroutine", _structureIdx);
    }

    private IEnumerator DemolishCoroutine(int _structureIdx)
    {
        float demolishTime = Time.time + 4f;
        while (demolishTime > Time.time)
        {
            // ui 표시
            yield return new WaitForSeconds(0.5f);
        }

        DestroyStructure(_structureIdx);
    }

    public void DestroyStructure(int _structureIdx)
    {
        Structure structure = null;
        dicStructure.TryGetValue(_structureIdx, out structure);
        InstantiateRuin(structure);
        structure.UpdateNodeWalkable(true);
        dicStructure.Remove(_structureIdx);
        if (structure.GetComponent<FriendlyObject>().GetObjectType().Equals(EObjectType.NUCLEAR))
            listNuclearStructure.Remove(structure.GetComponent<StructureNuclear>());
        structure.DestroyStructure();
    }

    private IEnumerator ShowBlueprint()
    {
        isBlueprint = true;

        if (curStructure == null) yield break;

        curStructure.Init(grid);

        RaycastHit hit;
        while (true)
        {
            Functions.Picking(1 << LayerMask.NameToLayer("StageFloor"), out hit);
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
        int xLength = curStructure.GridX;
        int yLength = curStructure.GridY;

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
                curStructure.SetGrid(xLength, yLength);
                curStructure.SetFactor(-1, -1);
                curStructure.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                wallPos.x = bunkerPos.x - 0.5f;
                wallPos.z = bunkerPos.z + 0.5f;
                curStructure.SetPos(wallPos);
            }
            // up 135 ~ 45
            else if (angle > 45)
            {
                curStructure.SetGrid(yLength, xLength);
                curStructure.SetFactor(-1, 1);
                curStructure.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                wallPos.x = bunkerPos.x + 0.5f;
                wallPos.z = bunkerPos.z + 1;
                curStructure.SetPos(wallPos);
            }
            // right 45 ~ -45
            else if (angle > -45)
            {
                curStructure.SetGrid(xLength, yLength);
                curStructure.SetFactor(1, 1);
                curStructure.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                wallPos.x = bunkerPos.x + 1;
                curStructure.SetPos(wallPos);
            }
            // down -45 ~ -135
            else
            {
                curStructure.SetGrid(yLength, xLength);
                curStructure.SetFactor(1, -1);
                curStructure.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                wallPos.z = bunkerPos.z - 0.5f;
                curStructure.SetPos(wallPos);
            }

            yield return null;
        }
    }

    public bool CancleBuild()
    {
        StopAllCoroutines();
        curStructure.BuildCancle();
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

            return true;
        }
        return false;
    }

    private void StopBuildCoroutine()
    {
        StopCoroutine("ShowWallBlueprint");
        StopCoroutine("ShowBlueprint");
    }

    private IEnumerator BuildStructureCoroutine()
    {
        Structure newStructure = Instantiate(arrStructurePrefab[(int)curStructureType], curStructure.transform.position, curStructure.transform.rotation).GetComponent<Structure>();
        Destroy(curStructure.gameObject);
        newStructure.Init(grid);
        newStructure.Init(structureIdx);
        dicStructure.Add(structureIdx, newStructure);
        ++structureIdx;
        if (curStructureType.Equals(EStructureType.WALL))
        {
            newStructure.SetGrid(curStructure.GridX, curStructure.GridY);
            newStructure.SetFactor(curStructure.FactorX, curStructure.FactorY);
        }
        newStructure.BuildStart();
        isBlueprint = false;

        float buildFinishTime = Time.time + buildDelay[(int)curStructureType];
        while (buildFinishTime > Time.time)
        {
            // ui 표시
            yield return new WaitForSeconds(0.5f);
        }

        if (newStructure != null)
        {
            newStructure.BuildComplete();
            newStructure.transform.parent = transform;
        }

        ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_COMPLETE);
    }

    #region Ruin
    private void InstantiateRuin(Structure _structure)
    {
        StructureCollider[] arrCol = _structure.GetComponentsInChildren<StructureCollider>();

        for (int i = 0; i < arrCol.Length; ++i)
        {
            GameObject ruinGo = Instantiate(ruinPrefab, arrCol[i].transform.position, Quaternion.identity);
            //grid.UpdateNodeWalkable(grid.GetNodeFromWorldPoint(ruinGo.transform.position),true);
            ruinGo.GetComponent<Structure>().Init();
        }
    }
    #endregion

    public void DeactivateUnit(GameObject _removeGo, ESpawnUnitType _unitType, int _barrackIdx)
    {
        Structure barrack = null;
        dicStructure.TryGetValue(_barrackIdx, out barrack);
        barrack.DeactivateUnit(_removeGo, _unitType);
    }

    public void SpawnNuclear(int _structureIdx)
    {
        Structure nuclearStructure = null;
        dicStructure.TryGetValue(_structureIdx, out nuclearStructure);

        if (nuclearStructure != null)
        {
            StructureNuclear nuclear = nuclearStructure.GetComponent<StructureNuclear>();
            nuclear.SpawnNuclear(SpawnNuclearComplete);
        }
    }

    private void SpawnNuclearComplete(StructureNuclear _nuclear)
    {
        listNuclearStructure.Add(_nuclear);
    }

    public void LaunchNuclear(Vector3 _destPos)
    {
        if (listNuclearStructure.Count > 0)
        {
            listNuclearStructure[0].LaunchNuclear(_destPos);
            listNuclearStructure.RemoveAt(0);
        }
    }

    public bool UpgradeStructure(int _structureIdx)
    {
        Structure structure = null;
        dicStructure.TryGetValue(_structureIdx, out structure);
        return structure.StartUpgrade();
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

    [Header("-Build Delay(TURRET, BUNKER, BARRACK, NUCLEAR, WALL)")]
    [SerializeField]
    private float[] buildDelay = new float[(int)EStructureType.LENGTH];

    private Dictionary<int, Structure> dicStructure = null;
    private List<StructureNuclear> listNuclearStructure = null;
    private Structure curStructure = null;
    private EObjectType curStructureObjType = EObjectType.NONE;
    private EStructureType curStructureType = EStructureType.NONE;
    private PF_Grid grid = null;
    private PF_Node curNode = null;

    private bool isBlueprint = false;
    private int structureIdx = 0;

    private static int upgradeLimit = 1;
}
