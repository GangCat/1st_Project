using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMainBaseFunc : CanvasFuncBase
{
    public void Init()
    {
        btnBuildTurret.onClick.AddListener(
            () =>
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, EObjectType.TURRET);
            });

        btnBuildBunker.onClick.AddListener(
            () =>
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, EObjectType.BUNKER);
            });

        btnBuildBarrack.onClick.AddListener(
            () =>
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, EObjectType.BARRACK);
            });

        btnBuildNuclear.onClick.AddListener(
            () =>
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, EObjectType.NUCLEAR);
            });

        gameObject.SetActive(false);
    }


    [SerializeField]
    private Button btnBuildTurret = null;
    [SerializeField]
    private Button btnBuildBunker = null;
    [SerializeField]
    private Button btnBuildBarrack = null;
    [SerializeField]
    private Button btnBuildNuclear = null;
}
