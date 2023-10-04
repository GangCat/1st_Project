using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInfoUnitName : TextInfoBase
{
    public override void UpdateText(params object[] _objects)
    {
        myText.text = string.Format("{0}", (string)_objects[0]);
    }
}
