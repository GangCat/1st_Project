using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStructureLevel : TextBase
{
    public override void UpdateText(params object[] _objects)
    {
        myText.text = string.Format("LV.{0}", (string)_objects[0]);
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }
}
