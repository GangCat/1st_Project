using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSpawnNuclearFunc : CanvasFuncBase
{
    public void Init()
    {
        btnSpawnMissile.onClick.AddListener(
            () =>
            {
                ArrayNuclearCommand.Use(ENuclearCommand.SPAWN_MISSILE);
            });

        gameObject.SetActive(false);
    }

    [SerializeField]
    private Button btnSpawnMissile = null;
}
