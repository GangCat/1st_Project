using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgradeEnergySupply : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayCurrencyCommand.Use(ECurrencyCommand.UPGRADE_ENERGY_SUPPLY);
            });
    }
}
