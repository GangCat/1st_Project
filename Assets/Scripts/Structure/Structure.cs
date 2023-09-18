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

    public virtual void Init(int _bunkerIdx = 0) { }

    public bool IsBuildable => isBuildable;

    public void SetColliderEnable(bool _isEnable)
    {
        GetComponent<Collider>().enabled = _isEnable;
    }

    public void SetPos(Vector3 _targetPos)
    {
        transform.position = _targetPos;
    }

    public virtual void UpdateNodeUnWalkable()
    {
        //curNode = grid.GetNodeFromWorldPoint(transform.position);

        //for (int i = 0; i < myGridX; ++i)
        //{
        //    for (int j = 0; j < myGridY; ++j)
        //        grid.UpdateNodeWalkable(grid.GetNodeWithGrid(curNode.gridX + i, curNode.gridY + j), false);
        //}

        curNode = grid.GetNodeFromWorldPoint(transform.position);
        int gridX = curNode.gridX;
        int gridY = curNode.gridY;
        int idx = 0;

        while (idx < myGridX * myGridY)
        {
            grid.UpdateNodeWalkable(
                grid.GetNodeWithGrid(
                    idx % myGridX + gridX,
                    idx / myGridY + gridY),
                false);

            ++idx;
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

            curNode = grid.GetNodeFromWorldPoint(transform.position);

            for (int i = 0; i < myGridX; ++i)
            {
                for (int j = 0; j < myGridY; ++j)
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
    protected int myGridX = 1;
    [SerializeField]
    protected int myGridY = 1;
    [SerializeField]
    protected Vector3 calcNodeOffset = Vector3.zero;

    protected PF_Grid grid = null;
    protected PF_Node curNode = null;
    protected Color oriColor = Color.magenta;
    protected Material mt = null;

    protected bool isBuildable = true;
}
