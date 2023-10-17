using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHideMenu : Command
{
    public CommandHideMenu(CanvasMenuPopup _canvasMenuPopup)
    {
        canvasMenuPopup = _canvasMenuPopup;
    }
    public override void Execute(params object[] _objects)
    {
        canvasMenuPopup.SetActive(false);
    }

    private CanvasMenuPopup canvasMenuPopup = null;
}
