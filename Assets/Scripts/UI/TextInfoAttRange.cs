using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInfoAttRange : TextInfoBase
{
    public override void UpdateText(params object[] _objects)
    {
        myText.text = string.Format("Attack Range : {0}", (float)_objects[0]);
    }
}
