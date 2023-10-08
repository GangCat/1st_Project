using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCancleFunc : CanvasFunc
{
    public void Init()
    {
        cancleBtn.onClick.AddListener(
            () =>
            {
                ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.CANCLE);
                gameObject.SetActive(false);
            });

        gameObject.SetActive(false);
    }

    [SerializeField]
    private Button cancleBtn = null;
}
