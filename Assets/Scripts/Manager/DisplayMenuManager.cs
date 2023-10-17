using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMenuManager : MonoBehaviour
{
    public void Init()
    {
        canvasMenuPopup = GetComponentInChildren<CanvasMenuPopup>();

        canvasMenuPopup.Init();

        ArrayMenuCommand.Add(EMenuCommand.DISPLAY_MENU, new CommandDisplayMenu(canvasMenuPopup));
        ArrayMenuCommand.Add(EMenuCommand.HIDE_MENU, new CommandHideMenu(canvasMenuPopup));
    }

    private CanvasMenuPopup canvasMenuPopup = null;
}
