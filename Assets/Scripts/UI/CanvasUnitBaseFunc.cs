using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUnitBaseFunc : CanvasFuncBase
{
    public void Init()
    {
        gameObject.SetActive(false);

        moveBtn.onClick.AddListener(
            ()=>
            {
                ListUnitButtonCommand.Use(1);
            });

        stopBtn.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use(2);
            });

        attackBtn.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use(5);
            });
    }

    [SerializeField]
    private Button moveBtn = null;
    [SerializeField]
    private Button stopBtn = null;
    [SerializeField]
    private Button holdBtn = null;
    [SerializeField]
    private Button PatrolBtn = null;
    [SerializeField]
    private Button attackBtn = null;
}
