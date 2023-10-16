using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour, IPauseObserver
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
        myObj = GetComponent<FriendlyObject>();
        myObj.Init();
        myStructureIdx = _structureIdx;
        upgradeLevel = 1;
        ShowHBeam();
        HideModel();

        ArrayPauseCommand.Use(EPauseCOmmand.REGIST, this);
    }

    public virtual void Init() { }

    public EUpgradeType CurUpgradeType => curUpgradeType;
    public int UpgradeLevel => upgradeLevel;
    public bool IsBuildable => isBuildable;
    public bool IsProcessingUpgrade => isProcessingUpgrade;
    public bool IsProcessingDemolish => isProcessingDemolish;
    public bool IsProcessingConstruct => isProcessingConstruct;
    public int StructureIdx => myStructureIdx;
    public int GridX => myGridX;
    public int GridY => myGridY;
    public int FactorX => factorGridX;
    public int FactorY => factorGridY;

    public virtual void CancleCurAction()
    {
        if(isProcessingUpgrade)
        {
            StopCoroutine("UpgradeCoroutine");
            isProcessingUpgrade = false;
            ArrayRefundCurrencyCommand.Use(ERefuncCurrencyCommand.UPGRADE_STRUCTURE, myObj.GetObjectType(), upgradeLevel);
            curUpgradeType = EUpgradeType.NONE;
        }
        else if (isProcessingConstruct)
        {
            StopCoroutine("BuildStructureCoroutine");
            isProcessingConstruct = false;
            ArrayRefundCurrencyCommand.Use(ERefuncCurrencyCommand.BUILD_STRUCTURE, myObj.GetObjectType());
            ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DEMOLISH_COMPLETE, myStructureIdx);
            DestroyStructure();
        }
        else if (isProcessingDemolish)
        {
            StopCoroutine("DemolishCoroutine");
            isProcessingDemolish = false;
        }


        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }

    public void UpdateConstructInfo()
    {
        ArrayHUDConstructCommand.Use(EHUDConstructCommand.UPDATE_CONSTRUCT_STRUCTURE, myObj.GetObjectType());
        ArrayHUDConstructCommand.Use(EHUDConstructCommand.UPDATE_CONSTRUCT_TIME, progressPercent);
    }

    public void UpdateDemolishInfo()
    {
        ArrayHUDConstructCommand.Use(EHUDConstructCommand.UPDATE_DEMOLISH_STRUCTURE, myObj.GetObjectType());
        ArrayHUDConstructCommand.Use(EHUDConstructCommand.UPDATE_DEMOLISH_TIME, progressPercent);
    }

    public void UpdateUpgradeInfo()
    {
        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.UPDATE_UPGRADE_TIME, progressPercent);
    }

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
            Debug.Log("structure" + upgradeLevel);
            Debug.Log("Limit" + StructureManager.UpgradeLimit);
            StartCoroutine("UpgradeCoroutine");
            return true;
        }
        Debug.Log("structure" + upgradeLevel);
        Debug.Log("Limit" + StructureManager.UpgradeLimit);
        return false;
    }

    protected IEnumerator UpgradeCoroutine()
    {
        isProcessingUpgrade = true;
        curUpgradeType = EUpgradeType.STRUCTURE;
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);

        float elapsedTime = 0f;
        progressPercent = elapsedTime / upgradeDelay;
        while (progressPercent < 1)
        {
            while (isPause)
                yield return null;
            // ui 표시
            if (myObj.IsSelect)
                ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.UPDATE_UPGRADE_TIME, elapsedTime / upgradeDelay);
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / upgradeDelay;
        }
        isProcessingUpgrade = false;
        UpgradeComplete();
    }

    protected virtual void UpgradeComplete()
    {
        curUpgradeType = EUpgradeType.NONE;
        if(myObj.IsSelect)
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

    public void BuildCancle()
    {
        StopCoroutine("CheckBuildableCoroutine");
    }

    public void BuildStart(float _buildDelay)
    {
        isBuildable = true;
        SetColor();
        isProcessingConstruct = true;
        if (myObj.IsSelect)
        {
            UpdateConstructInfo();
            ArrayHUDConstructCommand.Use(EHUDConstructCommand.UPDATE_CONSTRUCT_STRUCTURE, myObj.GetObjectType());
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
        }
        UpdateNodeWalkable(false);
        StopCoroutine("CheckBuildableCoroutine");
        StartCoroutine("BuildStructureCoroutine", _buildDelay);
    }

    protected IEnumerator BuildStructureCoroutine(float _buildDelay)
    {
        float elapsedTime = 0f;
        progressPercent = elapsedTime / _buildDelay;
        while (progressPercent <= 1)
        {
            while (isPause)
                yield return null;

            if (myObj.IsSelect)
                ArrayHUDConstructCommand.Use(EHUDConstructCommand.UPDATE_CONSTRUCT_TIME, progressPercent);
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / _buildDelay;
        }

        BuildComplete();
    }

    protected virtual void BuildComplete()
    {
        isProcessingConstruct = false;
        HideHBeam();
        ShowModel();
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }

    public virtual void Demolish()
    {
        isProcessingDemolish = true;
        if (myObj.IsSelect)
        {
            UpdateDemolishInfo();
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
        }
        StartCoroutine("DemolishCoroutine");
    }

    protected IEnumerator DemolishCoroutine()
    {
        float elapsedTime = 0f;
        progressPercent = elapsedTime / demolishDelay;

        while (progressPercent <= 1f)
        {
            while (isPause)
                yield return null;

            if (myObj.IsSelect)
                ArrayHUDConstructCommand.Use(EHUDConstructCommand.UPDATE_DEMOLISH_TIME, progressPercent);
            // ui 표시
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / demolishDelay;
        }
        ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DEMOLISH_COMPLETE, myStructureIdx);
        DestroyStructure();
    }

    public void DestroyStructure()
    {
        isProcessingDemolish = false;
        UpdateNodeWalkable(true);
        FriendlyObject fObj = GetComponent<FriendlyObject>();
        fObj.unSelect();
        ArraySelectCommand.Use(ESelectCommand.REMOVE_FROM_LIST, fObj);
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

    protected virtual void HideModel()
    {
        modelGo.SetActive(false);
    }

    protected virtual void ShowModel()
    {
        modelGo.SetActive(true);
    }

    public void CheckPause(bool _isPause)
    {
        isPause = _isPause;
    }

    [SerializeField]
    protected int myGridX = 1;
    [SerializeField]
    protected int myGridY = 1;
    [SerializeField]
    protected float upgradeDelay = 0f;
    [SerializeField]
    protected float demolishDelay = 4f;
    [SerializeField]
    protected GameObject modelGo = null;

    protected PF_Grid grid = null;
    protected PF_Node curNode = null;
    protected Color oriColor = Color.magenta;
    protected Material mt = null;

    protected StructureCollider[] arrCollider = null;
    protected int factorGridX = 1;
    protected int factorGridY = 1;
    protected int myStructureIdx = -1;
    protected int upgradeLevel = 0;

    protected float progressPercent = 0f;

    protected bool isBuildable = false;
    protected bool isProcessingUpgrade = false;
    protected bool isProcessingDemolish = false;
    protected bool isProcessingConstruct = false;
    protected EUpgradeType curUpgradeType = EUpgradeType.NONE;
    protected FriendlyObject myObj = null;

    protected bool isPause = false;
}