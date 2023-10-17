using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOutAllUnit : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayBunkerCommand.Use(EBunkerCommand.OUT_ALL_UNIT);
            });
    }
}
