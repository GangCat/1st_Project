using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLaunchNuclear : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.LAUNCH_NUCLEAR);
            });
    }
}
