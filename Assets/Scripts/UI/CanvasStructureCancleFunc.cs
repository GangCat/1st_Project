using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStructureCancleFunc : CanvasFunc
{
    public void Init()
    {
        btnStructureCancle.onClick.AddListener(
            () =>
            {
                ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.CANCLE);
            });

        gameObject.SetActive(false);
    }

    [SerializeField]
    private Button btnStructureCancle = null;
}
