using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStructureBaseFunc : CanvasFuncBase
{
    public void Init()
    {
        btnDemolishStructure.onClick.AddListener(
            () =>
            {
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.DEMOLISH);
            });

        gameObject.SetActive(false);
    }

    [SerializeField]
    private Button btnDemolishStructure = null;
}
