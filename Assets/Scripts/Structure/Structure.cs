using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public virtual void Init(PF_Grid _grid)
    {
        oriColor = GetComponentInChildren<MeshRenderer>().material.color;
        mt = GetComponentInChildren<MeshRenderer>().material;
        arrCollider = GetComponentsInChildren<StructureCollider>();

        for (int i = 0; i < arrCollider.Length; ++i)
            arrCollider[i].Init();
        HideHBeam();
        grid = _grid;
        StartCoroutine("CheckBuildableCoroutine");
    }

    public virtual void Init(int _structureIdx)
    {
        GetComponent<SelectableObject>().Init();
        myIdx = _structureIdx;
        upgradeLevel = 1;
        ShowHBeam();
        HideModel();
    }

    public virtual void Init() { }

    public EUpgradeType CurUpgradeType => curUpgradeType;
    public int UpgradeLevel => upgradeLevel;
    public bool IsBuildable => isBuildable;
    public int StructureIdx => myIdx;
    public int GridX => myGridX;
    public int GridY => myGridY;
    public int FactorX => factorGridX;
    public int FactorY => factorGridY;
    public bool IsUnderConstruction { get; private set; }
    public bool IsProcessingUpgrade => isProcessingUpgrade;

    public void SetGrid(int _gridX, int _gridY)
    {
        myGridX = _gridX;
        myGridY = _gridY;
    }

    public void SetFactor(int _factorGridX, int _factorGridY)
    {
        factorGridX = _factorGridX;
        factorGridY = _factorGridY;
    }

    public void SetPos(Vector3 _targetPos)
    {
        transform.position = _targetPos;
    }

    public virtual bool StartUpgrade()
    {
        if (!isProcessingUpgrade && upgradeLevel < StructureManager.UpgradeLimit)
        {
            StartCoroutine("UpgradeCoroutine");
            return true;
        }
        return false;
    }

    protected IEnumerator UpgradeCoroutine()
    {
        isProcessingUpgrade = true;
        curUpgradeType = EUpgradeType.STRUCTURE;
        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.DISPLAY_UPGRADE_INFO, curUpgradeType);

        float buildFinishTime = Time.time + upgradeDelay;
        while (buildFinishTime > Time.time)
        {
            // ui Ç¥½Ã
            yield return new WaitForSeconds(0.5f);
        }
        isProcessingUpgrade = false;
        UpgradeComplete();
    }

    protected virtual void UpgradeComplete()
    {
        curUpgradeType = EUpgradeType.NONE;
        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.FINISH);
        ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
        ++upgradeLevel;
    }

    public virtual void UpdateNodeWalkable(bool _walkable)
    {
        curNode = grid.GetNodeFromWorldPoint(transform.position);
        int gridX = curNode.gridX;
        int gridY = curNode.gridY;
        int idx = 0;
        List<PF_Node> listNode = new List<PF_Node>();

        while (idx < myGridX * myGridY)
        {
            listNode.Add(grid.GetNodeWithGrid((idx % myGridX) * factorGridX + gridX, (idx / myGridY) * factorGridY + gridY));
            grid.UpdateNodeWalkable(listNode[idx], _walkable);

            ++idx;
        }

        if (!_walkable)
            ArrayHUDCommand.Use(EHUDCommand.ADD_STRUCTURE_NODE_TO_MINIMAP, listNode.ToArray());
        else
            ArrayHUDCommand.Use(EHUDCommand.REMOVE_STRUCTURE_NODE_FROM_MINIMAP, listNode.ToArray());
    }

    public virtual void DeactivateUnit(GameObject _removeGo, EUnitType _type) { }

    public void BuildCancle()
    {
        StopCoroutine("CheckBuildableCoroutine");
    }

    public void BuildStart()
    {
        isBuildable = true;
        SetColor();
        IsUnderConstruction = true;
        StopCoroutine("CheckBuildableCoroutine");
        UpdateNodeWalkable(false);
    }

    public virtual void BuildComplete()
    {
        IsUnderConstruction = false;
        HideHBeam();
        ShowModel();
    }

    public void DestroyStructure()
    {
        Destroy(gameObject);
    }

    protected virtual IEnumerator CheckBuildableCoroutine()
    {
        while (true)
        {
            yield return null;

            isBuildable = true;

            curNode = grid.GetNodeFromWorldPoint(transform.position);
            int gridX = curNode.gridX;
            int gridY = curNode.gridY;
            int idx = 0;
            while (idx < myGridX * myGridY)
            {
                if (!grid.GetNodeWithGrid((idx % myGridX) + gridX, (idx / myGridY) + gridY).walkable)
                {
                    isBuildable = false;
                    break;
                }
                ++idx;
            }
            SetColor();
        }
    }

    protected void SetColor()
    {
        mt.color = isBuildable ? oriColor : Color.red;
    }

    private void ShowHBeam()
    {
        for (int i = 0; i < arrCollider.Length; ++i)
            arrCollider[i].ShowHBeam();
    }

    private void HideHBeam()
    {
        for (int i = 0; i < arrCollider.Length; ++i)
            arrCollider[i].HideHBeam();
    }

    private void HideModel()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    private void ShowModel()
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    [SerializeField]
    protected int myGridX = 1;
    [SerializeField]
    protected int myGridY = 1;
    [SerializeField]
    private float upgradeDelay = 0f;

    protected PF_Grid grid = null;
    protected PF_Node curNode = null;
    protected Color oriColor = Color.magenta;
    protected Material mt = null;

    protected StructureCollider[] arrCollider = null;
    protected int factorGridX = 1;
    protected int factorGridY = 1;
    protected int myIdx = -1;
    protected int upgradeLevel = 0;

    protected bool isBuildable = false;
    protected bool isProcessingUpgrade = false;
    protected EUpgradeType curUpgradeType = EUpgradeType.NONE;

}