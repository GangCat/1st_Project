using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageModel : MonoBehaviour
{
    public virtual void Init()
    {
        myImage = GetComponent<Image>();
    }

    public void ChangeSprite(Sprite _sprite)
    {
        myImage.sprite = _sprite;
    }

    protected Image myImage = null;
}
