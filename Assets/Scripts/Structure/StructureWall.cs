using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureWall : Structure
{
    public override void UpdateNodeUnWalkable()
    {
        curNode = grid.GetNodeFromWorldPoint(transform.position + wallStartPosOffset);
        int gridX = curNode.gridX;
        int gridY = curNode.gridY;
        int idx = 0;

        while(idx < myGridX * myGridY)
        {
            grid.UpdateNodeWalkable(
                grid.GetNodeWithGrid(
                    idx % myGridX + gridX, 
                    idx % myGridY + gridY),
                false);

            ++idx;
        }
    }

    [SerializeField]
    private Vector3 wallStartPosOffset = Vector3.zero;
}
