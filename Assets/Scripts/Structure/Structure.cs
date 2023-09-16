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

    public virtual void Init() { }

    public bool IsBuildable => isBuildable;

    public void SetColliderEnable(bool _isEnable)
    {
        GetComponent<Collider>().enabled = _isEnable;
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

    protected IEnumerator CheckBuildableCoroutine()
    {
        while (true)
        {
            isBuildable = true;

            curNode = grid.NodeFromWorldPoint(transform.position);

            for (int i = 0; i < myXGrid; ++i)
            {
                for (int j = 0; j < myYGrid; ++j)
                {
                    if (grid.GetNodeIsWalkable(curNode.gridX + i, curNode.gridY + j))
                        continue;

                    isBuildable = false;
                }
            }

            SetColor();
            yield return null;
        }
    }

    protected void SetColor()
    {
        mt.color = isBuildable ? oriColor : Color.red;
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
