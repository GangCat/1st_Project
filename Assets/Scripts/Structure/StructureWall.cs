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
        List<PF_Node> listNode = new List<PF_Node>();

        if (myGridX > myGridY)
        {
            while (idx < myGridX * myGridY)
            {
                listNode.Add(grid.GetNodeWithGrid((idx % myGridX) * factorGridX + gridX, (idx / myGridX) * factorGridY + gridY));
                grid.UpdateNodeWalkable(listNode[idx], _walkable);

                ++idx;
            }
        }
        else
        {
            while (idx < myGridX * myGridY)
            {
                listNode.Add(grid.GetNodeWithGrid((idx % myGridX) * factorGridX + gridX, (idx / myGridX) * factorGridY + gridY));
                grid.UpdateNodeWalkable(listNode[idx], _walkable);

                ++idx;
            }
        }

        if (!_walkable)
            ArrayHUDCommand.Use(EHUDCommand.ADD_STRUCTURE_NODE_TO_MINIMAP, listNode.ToArray());
        else
            ArrayHUDCommand.Use(EHUDCommand.REMOVE_STRUCTURE_NODE_FROM_MINIMAP, listNode.ToArray());
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
