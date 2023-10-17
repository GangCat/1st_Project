using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuildNuclear : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayMainbaseCommand.Use(EMainbaseCommnad.BUILD_STRUCTURE, EObjectType.NUCLEAR);
            });
    }
}
