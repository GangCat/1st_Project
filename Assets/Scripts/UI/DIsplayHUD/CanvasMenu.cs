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
                AudioManager.instance.PlayAudio_UI(objectType);
                ArrayMenuCommand.Use(EMenuCommand.DISPLAY_MENU);
            });
    }

    [SerializeField]
    private Button btnDisplayMenu = null;
    
    private EObjectType objectType;
}
