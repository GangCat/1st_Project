using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHeroFunc : CanvasFuncBase
{
    public void Init()
    {
        btnLaunchNuclear.onClick.AddListener(
            () =>
            {
                ArrayUnitButtonCommand.Use(EUnitButtonCommand.LAUNCH_NUCLEAR);
            });

        gameObject.SetActive(false);
    }

    [SerializeField]
    private Button btnLaunchNuclear = null;
}
