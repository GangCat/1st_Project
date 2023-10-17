using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawnNuclear : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayNuclearCommand.Use(ENuclearCommand.SPAWN_NUCLEAR);
            });
    }
}
