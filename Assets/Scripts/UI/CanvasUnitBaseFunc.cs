using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUnitBaseFunc : CanvasFuncBase
{
    public void Init()
    {
        gameObject.SetActive(false);

        btnMove.onClick.AddListener(
            ()=>
            {
                ListUnitButtonCommand.Use(1);
            });

        btnStop.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use(2);
            });

        btnHold.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use(3);
            });

        btnAttack.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use(5);
            });

    }

    [SerializeField]
    private Button btnMove = null;
    [SerializeField]
    private Button btnStop = null;
    [SerializeField]
    private Button btnHold = null;
    [SerializeField]
    private Button btnPatrol = null;
    [SerializeField]
    private Button btnAttack = null;
}
