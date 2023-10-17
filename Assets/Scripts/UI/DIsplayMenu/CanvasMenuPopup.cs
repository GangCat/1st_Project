using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMenuPopup : MonoBehaviour
{
    public void Init()
    {
        imagePauseBack.Init();
        imageMenuPopup.Init();

        btnPause.onClick.AddListener(
            () =>
            {
                ArrayPauseCommand.Use(EPauseCommand.TOGGLE_PAUSE);
            });


        btnGameReturn.onClick.AddListener(
            () =>
            {
                ArrayMenuCommand.Use(EMenuCommand.HIDE_MENU);
            });
    }

    public void SetActive(bool _isActive)
    {
        imageMenuPopup.SetActive(_isActive);
    }

    [SerializeField]
    private ImagePauseBackground imagePauseBack = null;
    [SerializeField]
    private ImageMenuPopup imageMenuPopup = null;
    [SerializeField]
    private Button btnPause = null;
    [SerializeField]
    private Button btnOptions = null;
    [SerializeField]
    private Button btnGameExit = null;
    [SerializeField]
    private Button btnGameReturn = null;
}
