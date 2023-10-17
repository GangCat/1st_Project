using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInfoMaxHp : TextBase
{
    public override void UpdateText(params object[] _objects)
    {
        myText.text = string.Format("MaxHp : {0}", (int)_objects[0]);
    }
}
