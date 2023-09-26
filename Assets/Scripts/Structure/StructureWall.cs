using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureWall : Structure
{
    protected override void UpgradeComplete()
    {
        Debug.Log("UpgradeCompleteWall");
    }

    public override void UpdateNodeWalkable(bool _walkable)
    {
        curNode = grid.GetNodeFromWorldPoint(transform.position);
        int gridX = curNode.gridX;
        int gridY = curNode.gridY;
        int idx = 0;

        if (myGridX > myGridY)
        {
            while (idx < myGridX * myGridY)
            {
                grid.UpdateNodeWalkable(
                    grid.GetNodeWithGrid( (idx % myGridX) * factorGridX + gridX, gridY), _walkable);

                ++idx;
            }
        }
        else
        {
            while (idx < myGridX * myGridY)
            {
                grid.UpdateNodeWalkable(
                    grid.GetNodeWithGrid(gridX, (idx % myGridY) * factorGridY + gridY), _walkable);

                ++idx;
            }
        }
    }

    protected override IEnumerator CheckBuildableCoroutine()
    {
        while (true)
        {
            isBuildable = true;

            curNode = grid.GetNodeFromWorldPoint(transform.position);
            int gridX = curNode.gridX;
            int gridY = curNode.gridY;
            int idx = 0;

            if (myGridX > myGridY)
            {
                while (idx < myGridX * myGridY)
                {
                    if (!grid.GetNodeWithGrid((idx % myGridX) * factorGridX + gridX, gridY).walkable)
                    {
                        isBuildable = false;
                        break;
                    }

                    ++idx;
                }

                SetColor();
                yield return null;
            }
            else
            {
                while (idx < myGridX * myGridY)
                {
                    if (!grid.GetNodeWithGrid(gridX, (idx % myGridY) * factorGridY + gridY).walkable)
                    {
                        isBuildable = false;
                        break;
                    }

                    ++idx;
                }

                SetColor();
                yield return null;
            }
        }
    }
}
