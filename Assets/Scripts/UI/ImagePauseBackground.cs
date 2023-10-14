using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePauseBackground : MonoBehaviour, IPauseObserver
{
    public void Init()
    {
        ArrayPauseCommand.Use(EPauseCOmmand.REGIST, this);
        gameObject.SetActive(false);
    }
    public void CheckPause(bool _isPause)
    {
        gameObject.SetActive(_isPause);
    }
}
