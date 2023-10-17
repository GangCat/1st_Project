using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInfoAttRate : TextBase
{
    public override void UpdateText(params object[] _objects)
    {
        myText.text = string.Format("AttackRate : {0}", (float)_objects[0]);
    }
}
