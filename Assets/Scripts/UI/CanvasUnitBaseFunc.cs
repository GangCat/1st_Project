using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUnitBaseFunc : CanvasFuncBase
{
    public void Init(VoidVoidDelegate _moveBtnCallback)
    {
        gameObject.SetActive(false);

        moveBtn.onClick.AddListener(
            ()=>
            {
                _moveBtnCallback?.Invoke();
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
