using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCurStateIndicator : TextBase
{
    public void SetPos(float _x, float _y, float _z)
    {
        transform.position = new Vector3(_x, _y, _z);
    }

    public override void UpdateText(params object[] _objects)
    {
        myText.text = (string)_objects[0];
    }
}
