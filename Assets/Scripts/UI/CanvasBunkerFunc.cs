using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBunkerFunc : CanvasFuncBase
{
    public void Init()
    {
        btnOutOneUnit.onClick.AddListener(
            () =>
            {
                ArrayBunkerCommand.Use(EBunkerCommand.OUT_ONE_UNIT);
            });

        btnOutAllUnit.onClick.AddListener(
            () =>
            {
                ArrayBunkerCommand.Use(EBunkerCommand.OUT_ALL_UNIT);
            });

        btnBuildWall.onClick.AddListener(
            () =>
            {
                ArrayBunkerCommand.Use(EBunkerCommand.EXPAND_WALL);
            });

        gameObject.SetActive(false);
    }



    [SerializeField]
    private Button btnOutOneUnit = null;
    [SerializeField]
    private Button btnOutAllUnit = null;
    [SerializeField]
    private Button btnBuildWall = null;
}
