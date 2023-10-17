using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgradeMeleeUnitDmg : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.MELEE_UNIT_DMG);
            });
    }
}
