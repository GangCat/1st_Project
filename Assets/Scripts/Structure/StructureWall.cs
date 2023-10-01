using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureWall : Structure
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        upgradeHpCmd = new CommandUpgradeStructureHP(GetComponent<StatusHp>());
    }
    protected override void UpgradeComplete()
    {
        base.UpgradeComplete();
        upgradeHpCmd.Execute(upgradeHpAmount);
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
                    grid.GetNodeWithGrid((idx % myGridX) * factorGridX + gridX, (idx / myGridX) * factorGridY + gridY), _walkable);

                ++idx;
            }
        }
        else
        {
            while (idx < myGridX * myGridY)
            {
                grid.UpdateNodeWalkable(
                    grid.GetNodeWithGrid((idx / myGridY) * factorGridX + gridX, (idx % myGridY) * factorGridY + gridY), _walkable);

                ++idx;
            }
        }
    }

    protected override IEnumerator CheckBuildableCoroutine()
    {
        while (true)
        {
            curNode = grid.GetNodeFromWorldPoint(transform.position);
            int gridX = curNode.gridX;
            int gridY = curNode.gridY;
            int idx = 0;
            isBuildable = true;

            if (myGridX > myGridY)
            {
                while (idx < myGridX * myGridY)
                {
                    if (!grid.GetNodeWithGrid((idx % myGridX) * factorGridX + gridX, (idx / myGridX) * factorGridY + gridY).walkable)
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
                    if (!grid.GetNodeWithGrid((idx / myGridY) * factorGridX + gridX, (idx % myGridY) * factorGridY + gridY).walkable)
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

    [SerializeField]
    private float upgradeHpAmount = 0f;

    private CommandUpgradeStructureHP upgradeHpCmd = null;
}
