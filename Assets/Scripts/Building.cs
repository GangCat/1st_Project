using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool IsBuildable => isBuildable;
    public void Init(PF_Grid _grid)
    {
        oriColor = GetComponentInChildren<MeshRenderer>().material.color;
        mt = GetComponentInChildren<MeshRenderer>().material;
        grid = _grid;
        StartCoroutine("CheckBuildableCoroutine");
    }

    private IEnumerator CheckBuildableCoroutine()
    {
        curNode = grid.NodeFromWorldPoint(transform.position);

        for(int i = 0; i < myXGrid; ++i)
        {
            for(int j = 0; j < myYGrid; ++j)
            {
                if (grid.GetNodeIsWalkable(curNode.gridX + i, curNode.gridY + j))
                    continue;

                UnBuildable();
                isBuildable = false;
                yield break;
            }
        }

        isBuildable = true;
        Buildable();
    }

    public void SetPos(Vector3 _targetPos)
    {
        transform.position = _targetPos;
    }

    public void UpdateNodeUnWalkable()
    {
        curNode = grid.NodeFromWorldPoint(transform.position);

        for (int i = 0; i < myXGrid; ++i)
        {
            for (int j = 0; j < myYGrid; ++j)
                grid.UpdateNodeWalkable(grid.GetNodeWithGrid(curNode.gridX + i, curNode.gridY + j), false);
        }
    }

    public void BuildComplete()
    {
        StopCoroutine("CheckBuildableCoroutine");
    }

    protected void UnBuildable()
    {
        mt.color = Color.red;
    }

    protected void Buildable()
    {
        mt.color = oriColor;
    }

    public void SetColliderEnable(bool _isEnable)
    {
        GetComponent<Collider>().enabled = _isEnable;
    }

    [SerializeField]
    protected int myXGrid = 1;
    [SerializeField]
    protected int myYGrid = 1;

    protected PF_Grid grid = null;
    protected PF_Node curNode = null;
    protected Color oriColor = Color.magenta;
    protected Material mt = null;

    protected bool isBuildable = true;
}
