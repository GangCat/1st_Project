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
                ListBuildCommand.Use(2);
            });

        btnBuildBunker.onClick.AddListener(
            () =>
            {
                ListBuildCommand.Use(3);
            });

        btnBuildWall.onClick.AddListener(
            () =>
            {
                ListBuildCommand.Use(4);
            });

        btnBuildNuclear.onClick.AddListener(
            () =>
            {
                ListBuildCommand.Use(5);
            });

        gameObject.SetActive(false);
    }



    [SerializeField]
    private Button btnBuildTurret = null;
    [SerializeField]
    private Button btnBuildBunker = null;
    [SerializeField]
    private Button btnBuildWall = null;
    [SerializeField]
    private Button btnBuildNuclear = null;
}
