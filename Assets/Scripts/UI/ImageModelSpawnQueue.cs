using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageModelSpawnQueue : ImageModel
{
    public Sprite GetCurSprite()
    {
        return myImage.sprite;
    }

    public void Clear()
    {
        myImage.sprite = null;
    }
}
