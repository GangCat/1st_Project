using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetRallyPoint : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT);
            });
    }
}
