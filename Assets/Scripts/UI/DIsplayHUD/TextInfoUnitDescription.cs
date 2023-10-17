using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInfoUnitDescription : TextBase
{
    public override void UpdateText(params object[] _objects)
    {
        myText.text = (string)_objects[0];
    }
}
