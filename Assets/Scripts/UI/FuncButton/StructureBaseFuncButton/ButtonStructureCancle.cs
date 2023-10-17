using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStructureCancle : FuncButtonBase
{
    public override void Init()
    {
        GetComponent<Button>().onClick.AddListener(
            () =>
            {
                ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.CANCLE_CURRENT_FUNCTION);
            });
    }
}
