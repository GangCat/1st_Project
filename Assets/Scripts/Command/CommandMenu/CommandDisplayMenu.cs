using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplayMenu : Command
{
    public CommandDisplayMenu(CanvasMenuPopup _canvasMenuPopup)
    {
        canvasMenuPopup = _canvasMenuPopup;
    }
    public override void Execute(params object[] _objects)
    {
        canvasMenuPopup.SetActive(true);
    }

    private CanvasMenuPopup canvasMenuPopup = null;
}
