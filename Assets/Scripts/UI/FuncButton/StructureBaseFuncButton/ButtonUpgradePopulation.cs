using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgradePopulation : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayPopulationCommand.Use(EPopulationCommand.UPGRADE_MAX_POPULATION);
            });
    }
}
