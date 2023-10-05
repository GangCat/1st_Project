using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHeroFunc : CanvasFunc
{
    public void Init()
    {
        btnLaunchNuclear.onClick.AddListener(
            () =>
            {
                ArrayFuncButtonCommand.Use(EFuncButtonCommand.LAUNCH_NUCLEAR);
            });

        gameObject.SetActive(false);
    }

    [SerializeField]
    private Button btnLaunchNuclear = null;
}
