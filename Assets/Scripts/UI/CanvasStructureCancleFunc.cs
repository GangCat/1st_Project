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
                ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.CANCLE_CURRENT_FUNCTION);
            });

        gameObject.SetActive(false);
    }

    public void DisplayCancleButton()
    {
        SetActive(true);
    }

    public void HideCancleButton()
    {
        SetActive(false);
    }

    [SerializeField]
    private Button btnStructureCancle = null;
}
