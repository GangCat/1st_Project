using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMenu : MonoBehaviour
{
    public void Init()
    {
        btnDisplayMenu.onClick.AddListener(
            () =>
            {
                ArrayMenuCommand.Use(EMenuCommand.DISPLAY_MENU);
            });
    }

    [SerializeField]
    private Button btnDisplayMenu = null;
}
