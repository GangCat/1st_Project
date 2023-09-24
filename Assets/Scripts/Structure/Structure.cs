using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public virtual void Init(PF_Grid _grid)
    {
        oriColor = GetComponentInChildren<MeshRenderer>().material.color;
        mt = GetComponentInChildren<MeshRenderer>().material;
        grid = _grid;
        StartCoroutine("CheckBuildableCoroutine");
    }

    public virtual void Init(int _structureIdx)
    {
        GetComponent<SelectableObject>().Init();
        myIdx = _structureIdx;
    }

    public bool IsBuildable => isBuildable;
    public int StructureIdx => myIdx;
    public int GridX => myGridX;
    public int GridY => myGridY;
    public int FactorX => factorGridX;
    public int FactorY => factorGridY;

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

    public virtual void UpdateNodeWalkable(bool _walkable)
    {
        curNode = grid.GetNodeFromWorldPoint(transform.position);
        int gridX = curNode.gridX;
        int gridY = curNode.gridY;
        int idx = 0;

        while (idx < myGridX * myGridY)
        {
            grid.UpdateNodeWalkable(
                grid.GetNodeWithGrid(
                    (idx % myGridX) * factorGridX + gridX,
                    (idx / myGridY) * factorGridY + gridY),
                _walkable);

            ++idx;
        }
    }

    public virtual void DeactivateUnit(GameObject _removeGo, ESpawnUnitType _type) { }

    public void BuildComplete()
    {
        StopCoroutine("CheckBuildableCoroutine");
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
                if (!grid.GetNodeWithGrid((idx % myGridX) * factorGridX + gridX, (idx / myGridY) * factorGridY + gridY).walkable)
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


    [SerializeField]
    protected int myGridX = 1;
    [SerializeField]
    protected int myGridY = 1;

    protected PF_Grid grid = null;
    protected PF_Node curNode = null;
    protected Color oriColor = Color.magenta;
    protected Material mt = null;

    protected int factorGridX = 1;
    protected int factorGridY = 1;
    protected int myIdx = -1;

    protected bool isBuildable = true;
}
