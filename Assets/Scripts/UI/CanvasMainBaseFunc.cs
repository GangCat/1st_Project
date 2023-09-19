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
                ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, ESelectableObjectType.TURRET);
            });

        btnBuildBunker.onClick.AddListener(
            () =>
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, ESelectableObjectType.BUNKER);
            });

        btnBuildBarrack.onClick.AddListener(
            () =>
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, ESelectableObjectType.BARRACK);
            });

        btnBuildNuclear.onClick.AddListener(
            () =>
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, ESelectableObjectType.NUCLEAR);
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
